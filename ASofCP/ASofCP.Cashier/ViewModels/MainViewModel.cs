using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Models.Base;
using ASofCP.Cashier.ViewModels.Base;
using ASofCP.Cashier.ViewModels.ChildViewModels;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;
using it.q02.asocp.api.data;
using UseAbilities.IoC.Attributes;
using UseAbilities.IoC.Stores;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels
{
    public class MainViewModel : UtilViewModel
    {
        private static IEnumerable<ICashVoucherItem> _cashVoucherToPrint;
        private int _backupIndex;
        private Dictionary<int, CashVoucher<ICashVoucherItem>> _backup = new Dictionary<int, CashVoucher<ICashVoucherItem>>();
        private Cheque _cheque;
        private int _currentOrder;

        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        public MainViewModel()
        {
            var resultCashVoucher = new CashVoucher<ICashVoucherItem>();
            UpdateResultCashVoucher(resultCashVoucher);
            
            CurrentDateTime = DateTime.Now;
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += delegate
                {
                    CurrentDateTime = DateTime.Now;
                };

            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            dispatcherTimer.Start();
        }
        // ReSharper restore DoNotCallOverridableMethodsInConstructor

        [InjectedProperty]
        public IStore<ModuleSettings> SettingsStore { get; set; }

        public virtual GroupContentList CollectionServices { get; set; }
        public virtual double Total { get; set; }
        public virtual ICollectionView ResultCashVoucher { get; set; }
        public virtual int? NewCount { get; set; }

        public virtual String CurrentTicketSeries
        {
            get { return CurrentRollInfo.NotNull() ? CurrentRollInfo.Series : "Неопределено"; }
            //set { CurrentRollInfo.Series = value; }
        }

        public virtual long CurrentTicketNumber
        {
            get { return CurrentRollInfo.NotNull() ? CurrentRollInfo.NextTicket : -1; } 
            set { if (CurrentRollInfo.NotNull()) CurrentRollInfo.NextTicket = value; }
        }
        public virtual DateTime CurrentDateTime { get; set; }
        public virtual DateTime OpenDate { get; set; }
        public virtual bool IsShowErrorMessage { get; set; }
        public virtual String RightErrorMessage { get; set; }
        public virtual string PosTitle { get; set; }
        public virtual string User { get; set; }

        public virtual long TicketsLeft
        {
            get { return CurrentRollInfo.NotNull() ? CurrentRollInfo.TicketsLeft : -1; } 
            set { CurrentRollInfo.TicketsLeft = value; }
        }

        public Shift CurrentShift { get; set; }

        public virtual RollInfo CurrentRollInfo { get; set; }

        #region SelectedVoucherItem
        private ICashVoucherItem _selectedPreviewVoucherItem;
        private ICashVoucherItem _selectedVoucherItem;
        public virtual ICashVoucherItem SelectedVoucherItem
        {
            get
            {
                return _selectedVoucherItem;
            }
            set
            {
                _selectedPreviewVoucherItem = _selectedVoucherItem;
                _selectedVoucherItem = value;
                NewCount = value.NotNull() ? value.Count : (int?) null;
            }
        }
        #endregion

        #region SelectedParkService
        private ParkService _selectedParkService;
        public virtual ParkService SelectedParkService
        {
            get
            {
                return _selectedParkService;
            }
            set
            {
                _selectedParkService = value;
                if (_selectedParkService.IsNull() || !_selectedParkService.IsFinal) return;

                if (_cheque.IsNull())
                {
                    OpenDate = DateTime.Now;
                    _cheque = new Cheque
                    {
                        OpenDate = OpenDate,
                        Shift = CurrentShift
                    };
                }

                ICashVoucherItem item = new CashVoucherItem(_selectedParkService);
                var cashVoucherItem = (CashVoucher<ICashVoucherItem>) ResultCashVoucher.SourceCollection;

                _currentOrder++;
                item.Order = _currentOrder;
                cashVoucherItem.Add(item);
                ResultCashVoucher.MoveCurrentToLast();
                SelectedVoucherItem = cashVoucherItem.Get(item);
                Total = cashVoucherItem.GetTotal();

                ResultCashVoucher.Refresh();
            }
        }
        #endregion

        #region CalculateCommand
        private ICommand _calculateCommand;
        public ICommand CalculateCommand
        {
            get
            {
                return _calculateCommand ?? (_calculateCommand = new RelayCommand(param => OnCalculateCommand(), can => ValidateCalculateCommand()));
            }
        }

        private bool ValidateCalculateCommand()
        {
            return Total > 0 && CurrentRollInfo != null && CurrentRollInfo.IsActiveOnStation;
        }

        private void OnCalculateCommand()
        {
            //IsShowErrorMessage = false;
            //if (CurrentRollInfo == null || !CurrentRollInfo.IsActiveOnStation)
            //{
            //    RightErrorMessage = "Бабина с билетами не определена или не активировна!";
            //    IsShowErrorMessage = true;
            //    return;
            //}

            _cashVoucherToPrint = (CashVoucher<ICashVoucherItem>)ResultCashVoucher.SourceCollection;

            var ticketsNeed = Math.Abs(TicketsLeft - _cashVoucherToPrint.Sum(item => item.Count));
            if (ticketsNeed < 0)
            {
                var informationViewModel = ObserveWrapperHelper.GetInstance().Resolve<InformationViewModel>();
                informationViewModel.Count = ticketsNeed;
                informationViewModel.CurrentRollInfo = CurrentRollInfo;
                informationViewModel.Show();
                informationViewModel.Closed += delegate(object senderD, RollInfoEventArgs args)
                {
                    if (args != null) CurrentRollInfo = args.RollInfo ?? CurrentRollInfo;

                    ResolvePaymentViewModel();
                };
            }
            else ResolvePaymentViewModel();
        }

        private void ResolvePaymentViewModel()
        {
            IsShowErrorMessage = false;
            var paymentViewModel = ObserveWrapperHelper.GetInstance().Resolve<PaymentViewModel>();
            paymentViewModel.Total = Total;
            paymentViewModel.Show();
            paymentViewModel.PaymentReached += delegate(object sender, PaymentEventArgs args)
            {
                if (!args.PaymentType.HasValue) return;

                _cheque.MoneyType = (short)args.PaymentType.Value;
                _chequeRows = new List<ChequeRow>();

                PrintTickets();
            };
        }
        

        #endregion

        #region LoadedCommand
        protected override void OnLoadedCommand()
        {
            //
        }
        #endregion

        #region AcceptCommand
        private ICommand _acceptCommand;
        public ICommand AcceptCommand
        {
            get
            {
                return _acceptCommand ?? (_acceptCommand = new RelayCommand(param => OnAcceptCommand(), can => ValidateAccept()));
            }
        }

        private void OnAcceptCommand()
        {
            BackupCashVoucher();

            // ReSharper disable PossibleInvalidOperationException
            SelectedVoucherItem.Count = NewCount.Value;
            // ReSharper restore PossibleInvalidOperationException

            CalculateTotal();
            ResultCashVoucher.Refresh();
        }

        private bool ValidateAccept()
        {
            return NewCount.HasValue && NewCount != SelectedVoucherItem.Count;
        }
        #endregion

        #region RemoveCommand
        private ICommand _removeCommand;

        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand ?? (_removeCommand = new RelayCommand(param => OnRemoveCommand(), can => ValidatRemove()));
            }
        }

        private void OnRemoveCommand()
        {
            BackupCashVoucher();
            ((CashVoucher<ICashVoucherItem>) ResultCashVoucher.SourceCollection).Remove(SelectedVoucherItem);
            SelectedVoucherItem = _selectedPreviewVoucherItem;
            CalculateTotal();
            ResultCashVoucher.Refresh();
        }

        private bool ValidatRemove()
        {
            return SelectedVoucherItem.NotNull();
        }
        #endregion

        #region UndoCashVoucherStateCommand
        private ICommand _undoCashVoucherStateCommand;
        public ICommand UndoCashVoucherStateCommand
        {
            get
            {
                return _undoCashVoucherStateCommand ?? (_undoCashVoucherStateCommand = new RelayCommand(param => OnUndoCashVoucherStateCommand(), can => ValidatUndo()));
            }
        }

        private bool _isPreviewUndo;
        private void OnUndoCashVoucherStateCommand()
        {
            if (!_isPreviewUndo && !_isPreviewRedo) 
                BackupCashVoucher();
            else _isPreviewRedo = false;

            _backupIndex = CalculateBackupIndex(_backupIndex - 1);
            UpdateResultCashVoucher(_backup[_backupIndex]);
            _isPreviewUndo = true;
        }

        private bool ValidatUndo()
        {
            var backupIndex = CalculateBackupIndex(_backupIndex - 1);
            var isNotFullStack = _backup.Count < backupIndex;
            //return !(isNotFullStack && _isPreviewUndo) && _backup.ContainsKey(isNotFullStack ? CalculateBackupIndex(backupIndex + 1) : backupIndex);
            return !_isPreviewUndo && _backup.ContainsKey(CalculateBackupIndex(isNotFullStack ? CalculateBackupIndex(backupIndex + 1) : backupIndex));
        }
        #endregion

        #region RedoCashVoucherStateCommand
        private ICommand _redoCashVoucherStateCommand;
        public ICommand RedoCashVoucherStateCommand
        {
            get
            {
                return _redoCashVoucherStateCommand ?? (_redoCashVoucherStateCommand = new RelayCommand(param => OnRedoCashVoucherStateCommand(), can => ValidatRedo()));
            }
        }

        private bool _isPreviewRedo;

        private void OnRedoCashVoucherStateCommand()
        {
            _isPreviewUndo = false;
            _isPreviewRedo = true;
            _backupIndex = CalculateBackupIndex(_backupIndex + 1);
            UpdateResultCashVoucher(_backup[_backupIndex]);
        }

        private bool ValidatRedo()
        {
            return _backup.ContainsKey(CalculateBackupIndex(_backupIndex + 1));
        }
        #endregion

        #region ChangeRollCommand
        private ICommand _changeRollCommand;
        public ICommand ChangeRollCommand
        {
            get
            {
                return _changeRollCommand ?? (_changeRollCommand = new RelayCommand(param => OnChangeRollCommand(), can => ValidateChangeRollCommand()));
            }
        }

        private void OnChangeRollCommand()
        {
            IsShowErrorMessage = false;

            var rollInfoViewModelA = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModelA.Mode = ChildWindowMode.ChangeRoll;
            rollInfoViewModelA.CurrentRollInfo = CurrentRollInfo;
            rollInfoViewModelA.Show();
            rollInfoViewModelA.Closed += delegate(object senderA, RollInfoEventArgs argsA)
            {
                if (argsA == null) throw new NullReferenceException("Информация о смене и бабине не определена!");
                CurrentRollInfo = argsA.RollInfo ?? CurrentRollInfo;
                OnPropertyChanged(() => CurrentTicketNumber);
                OnPropertyChanged(() => CurrentTicketSeries);
                OnPropertyChanged(() => TicketsLeft);
            };
        }

        private bool ValidateChangeRollCommand()
        {
            return CurrentRollInfo != null && CurrentRollInfo.IsActiveOnStation;
        }
        #endregion

        #region CloseShiftCommand
        private ICommand _closeShiftCommand;
        public ICommand CloseShiftCommand
        {
            get
            {
                return _closeShiftCommand ?? (_closeShiftCommand = new RelayCommand(param => OnCloseShiftCommand(), can => ValidateCloseShiftCommand()));
            }
        }

        private void OnCloseShiftCommand()
        {
            var rollInfoViewModel = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModel.Mode = ChildWindowMode.CloseShift;
            rollInfoViewModel.CurrentRollInfo = CurrentRollInfo;
            rollInfoViewModel.Show();
            rollInfoViewModel.Closed += delegate(object sender, RollInfoEventArgs args)
            {
                if (args == null) return;

                var loginViewModel = ObserveWrapperHelper.GetInstance().Resolve<LoginViewModel>();
                loginViewModel.Show();
                Close();
                Dispose();
            };
        }

        private bool ValidateCloseShiftCommand()
        {
            return CurrentShift != null && CurrentShift.Active && CurrentRollInfo != null && CurrentRollInfo.IsActiveOnStation;
        }

        #endregion

        public void OpenSession()
        {
            //TODO: Переопределить Show
            Show();
            var rollInfoViewModel = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModel.Mode = ChildWindowMode.OpenShift;
            rollInfoViewModel.Show();
            rollInfoViewModel.Closed += delegate(object sender, RollInfoEventArgs args)
            {
                if (args == null || args.RollInfo == null)
                {
                    var loginViewModel = ObserveWrapperHelper.GetInstance().Resolve<LoginViewModel>();
                    loginViewModel.Show();
                    Close();
                    Dispose();
                    return;
                }

                CurrentRollInfo = args.RollInfo;
                CurrentShift = args.Shift;
                User = CurrentShift.CashierName;
                OnPropertyChanged(() => CurrentTicketNumber);
                OnPropertyChanged(() => CurrentTicketSeries);
                OnPropertyChanged(() => TicketsLeft);

                var collectionServices = new GroupContentList();
                var attractions = BaseAPI.getAttractionsFromGroup(new AttractionGroupInfo());
                if (attractions.IsNullOrEmpty()) return;
                collectionServices.AddRange(attractions.OrderBy(i => i.DisplayName).Select(attraction => new ParkService(attraction)));

                CollectionServices = collectionServices;
            };
        }

        #region Private Methods
        private void BackupCashVoucher()
        {
            if (_isPreviewUndo || _isPreviewRedo)
            {
                _isPreviewUndo = false;
                _isPreviewRedo = false;
                _backupIndex = 0;
                _backup = new Dictionary<int, CashVoucher<ICashVoucherItem>>();
            }
            _backupIndex = CalculateBackupIndex(_backupIndex + 1);
            var backupValue = ((CashVoucher<ICashVoucherItem>)ResultCashVoucher.SourceCollection).Clone();
            if (_backup.ContainsKey(_backupIndex)) _backup[_backupIndex] = backupValue;
            else _backup.Add(_backupIndex, backupValue);
        }

        private static int CalculateBackupIndex(int targetBackupIndex)
        {
            if (targetBackupIndex > 0 && targetBackupIndex < 11) return targetBackupIndex;
            return targetBackupIndex <= 0 ? 10 : 1;
        }

        private void CalculateTotal()
        {
            var cashVoucherItem = (CashVoucher<ICashVoucherItem>)ResultCashVoucher.SourceCollection;
            Total = cashVoucherItem.GetTotal();
        }

        private void UpdateResultCashVoucher(IEnumerable cashVoucher)
        {
            var view = CollectionViewSource.GetDefaultView(cashVoucher);
            if (view == null) return;
            view.Filter = null;

            ResultCashVoucher = view;
            if (ResultCashVoucher.SortDescriptions.IsEmpty())
                ResultCashVoucher.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Descending));
            ResultCashVoucher.Refresh();
        }

        private List<ChequeRow> _chequeRows; 
        private void CreateChequeRow(bool printed, DateTime printDate, string barcode, AttractionInfo attraction)
        {
            _chequeRows.Add(new ChequeRow
            {
                Printed = printed,
                PrintDate = printDate,
                TicketBarCode = barcode,
                TicketNumber = CurrentTicketNumber - 1,
                Attraction = attraction,
                TicketRoll = CurrentRollInfo
            });
        }

        private void CloseCheque()
        {
            _cheque.CloseDate = DateTime.Now;
            _cheque.Rows = _chequeRows.ToArray();
            BaseAPI.createCheque(_cheque);

            UpdateResultCashVoucher(new CashVoucher<ICashVoucherItem>());
            Total = 0;
            _cheque = null;
            _currentOrder = 0;
            _cashVoucherToPrint = null;
        }

        private void PrintTickets()
        {
            var settings = SettingsStore.Load();
            foreach (var item in _cashVoucherToPrint.Where(item => item.Count >= 1 && !item.IsPrinted))
            {
                if(item.Count == 1)
                {
                    if (!ProcessingPrint(settings.PrinterName, item)) return;
                    continue;
                }

                var i = 0;
                do
                {
                    i++;
                    if (!ProcessingPrint(settings.PrinterName, item)) return;
                } while (item.Count > i);
            }

            if(_cashVoucherToPrint.All(item=> item.IsPrinted)) CloseCheque();
        }

        private bool ProcessingPrint(string printerName, ICashVoucherItem item)
        {
            var barcode = PrepareBarcode(CurrentTicketNumber);
            var printed = SendToPrint(printerName, item, barcode);
            CreateChequeRow(printed.IsSuccess, DateTime.Now, barcode, item.AttractionInfo);
            item.IsPrinted = printed.IsSuccess;

            if (!printed.IsNeedNewTicketRoll) return !printed.HasError;
            
            NeedChangeRoll();
            return false;
        }

        private static String PrepareBarcode(long num)
        {
            var curTicketNum = num.ToString(CultureInfo.InvariantCulture);
            var curTicketLength = curTicketNum.Length;
            return curTicketLength > 13
                               ? curTicketNum.Remove(13, curTicketLength)
                               : curTicketNum.Insert(curTicketLength - 1, "0000000000000".Remove(0, 13 - curTicketLength));
        }

        private PrintResult SendToPrint(String printerName, ICashVoucherItem item, String barcode)
        {
            IsShowErrorMessage = false;
            var pathToTemplate = SettingsStore.Load().PathToTemplate;
            try
            {
                if (DebugHelper.IsPrintEnabled)
                    RawPrinterHelper.SendStringToPrinter(printerName, ZebraHelper.LoadAndFillTemplate(pathToTemplate, CurrentDateTime.Date.ToString("dd.MM.yyyy"), item.Price.ToString(CultureInfo.InvariantCulture), item.PrintTitle, "", barcode));
            }
            catch (Exception e)
            {
                IsShowErrorMessage = true;
                RightErrorMessage = e.Message;
                return PrintResult.Failure;
            }
            finally
            {
                CurrentTicketNumber++;
                TicketsLeft--;
            }

            if (TicketsLeft <= 25)
            {
                RightErrorMessage = String.Format("Заканчиваются билеты! Осталось: {0}", TicketsLeft);
                IsShowErrorMessage = true;
            }

            return TicketsLeft > 0 ? PrintResult.Success : PrintResult.SuccessAndNeedNewTicketRoll;
        }

        private void NeedChangeRoll()
        {
            var rollInfoViewModelA = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModelA.Mode = ChildWindowMode.NeedNewRoll;
            rollInfoViewModelA.CurrentRollInfo = CurrentRollInfo;
            rollInfoViewModelA.Show();
            rollInfoViewModelA.Closed += delegate(object sender, RollInfoEventArgs args)
            {
                if (args == null || args.RollInfo.IsNull()) throw new NullReferenceException("Информация о смене и бабине не определена!");
                CurrentRollInfo = args.RollInfo;
                PrintTickets(); 
            };
        }
        #endregion

        protected override void OnSettingsCommand()
        {
            var settingsVM = ObserveWrapperHelper.GetInstance().Resolve<SettingsViewModel>();
            settingsVM.Show();
        }
    }
}
