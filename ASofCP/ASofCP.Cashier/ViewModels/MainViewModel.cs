using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
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
    public class MainViewModel : ApplicationViewModel
    {
        private int _backupIndex;
        private Dictionary<int, CashVoucher<ICashVoucherItem>> _backup = new Dictionary<int, CashVoucher<ICashVoucherItem>>();
        private Cheque _cheque;
        private int _currentOrder;

        public MainViewModel()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            //CollectionServices = TestDataHelper.GetParkServices();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
            var resultCashVoucher = new CashVoucher<ICashVoucherItem>();
            UpdateResultCashVoucher(resultCashVoucher);
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            CurrentDateTime = DateTime.Now;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += delegate
                {
                    CurrentDateTime = DateTime.Now;
                };

            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            dispatcherTimer.Start();
        }

        [InjectedProperty]
        public IReadStore<ModuleSettings> SettingsStore{get;set;}

        public virtual GroupContentList CollectionServices { get; set; }
        public virtual double Total { get; set; }
        public virtual ICollectionView ResultCashVoucher { get; set; }
        public virtual int? NewCount { get; set; }
        public virtual String CurrentTicketSeries { get; set; }
        public virtual long CurrentTicketNumber { get; set; }
        public virtual DateTime CurrentDateTime { get; set; }
        public virtual DateTime OpenDate { get; set; }
        public virtual bool IsShowErrorMessage { get; set; }
        public virtual String RightErrorMessage { get; set; }
        public virtual string PosTitle { get; set; }
        public virtual string User { get; set; }
        public virtual long TicketsLeft { get; set; }

        public Shift CurrentShift { get; set; }

        private RollInfo _currentRollInfo;
        public virtual RollInfo CurrentRollInfo
        {
            get { return _currentRollInfo; }
            set
            {
                _currentRollInfo = value;
                if (value.IsNull()) return;
                CurrentTicketSeries = value.Series;
                CurrentTicketNumber = value.NextTicket;
                TicketsLeft = value.TicketsLeft;
            }
        }

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
                // ReSharper disable PossibleMultipleEnumeration
                _currentOrder++;
                item.Order = _currentOrder;
                cashVoucherItem.Add(item);
                ResultCashVoucher.MoveCurrentToLast();
                SelectedVoucherItem = cashVoucherItem.Get(item);
                Total = cashVoucherItem.GetTotal();
                // ReSharper restore PossibleMultipleEnumeration
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
                return _calculateCommand ?? (_calculateCommand = new RelayCommand(param => OnCalculateCommand(), can => Total > 0));
            }
        }

        private void OnCalculateCommand()
        {
            var cashVoucher = (CashVoucher<ICashVoucherItem>) ResultCashVoucher.SourceCollection;

            var ticketsNeed = CurrentRollInfo.TicketsLeft - cashVoucher.Sum(item => item.Count);
            if (ticketsNeed < 0)
            {
                var informationViewModel = ObserveWrapperHelper.GetInstance().Resolve<InformationViewModel>();
                informationViewModel.Count = ticketsNeed;
                informationViewModel.CurrentTicketSeries = CurrentTicketSeries;
                informationViewModel.CurrentTicketNumber = CurrentTicketNumber;
                informationViewModel.CurrentTicketColor = CurrentRollInfo.Color;
                informationViewModel.Show();
                informationViewModel.Closed += delegate(object senderD, RollInfoEventArgs args)
                {
                    if (args != null) CurrentRollInfo = args.RollInfo ?? CurrentRollInfo;

                    ResolvePaymentViewModel(cashVoucher);
                };
            }
            else ResolvePaymentViewModel(cashVoucher);
        }

        private void ResolvePaymentViewModel(IEnumerable<ICashVoucherItem> cashVoucher)
        {
            IsShowErrorMessage = false;
            var paymentViewModel = ObserveWrapperHelper.GetInstance().Resolve<PaymentViewModel>();
            paymentViewModel.Total = Total;
            paymentViewModel.Show();
            paymentViewModel.PaymentReached += delegate(object sender, PaymentEventArgs args)
            {
                if (!args.PaymentType.HasValue) return;

                _cheque.MoneyType = (short)args.PaymentType.Value;

                if (!PrintTickets(cashVoucher))
                {
                    IsShowErrorMessage = true;
                    RightErrorMessage = "Печать завершилась неудачей!";
                    return;
                };
                
                BaseAPI.createCheque(_cheque);

                UpdateResultCashVoucher(new CashVoucher<ICashVoucherItem>());
                Total = 0;
                _cheque = null;
                _currentOrder = 0;
            };
        }
        

        #endregion

        #region LoadedCommand
        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                return _loadedCommand ?? (_loadedCommand = new RelayCommand(param => OnLoadedCommand(), null));
            }
        }

        private void OnLoadedCommand()
        {

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
                return _changeRollCommand ?? (_changeRollCommand = new RelayCommand(param => OnChangeRollCommand(), null));
            }
        }

        private void OnChangeRollCommand()
        {
            IsShowErrorMessage = false;
            //var rollInfoViewModelD = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            //rollInfoViewModel.Prepare("Укажите информацию о бабине", "Первый билет", "Сменить бабину", true);
            //rollInfoViewModelD.Mode = ChildWindowMode.ChangeRollDeactivate;
            //rollInfoViewModelD.Show();
            //rollInfoViewModelD.Closed += delegate(object senderD, RollInfoEventArgs argsD)
            //{
                //if (argsD == null) throw new NullReferenceException("Информация о смене и бабине не определена!");
            //};

            var rollInfoViewModelA = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModelA.Mode = ChildWindowMode.ChangeRoll;
            rollInfoViewModelA.CurrentTicketSeries = CurrentTicketSeries;
            rollInfoViewModelA.CurrentTicketNumber = CurrentTicketNumber;
            rollInfoViewModelA.CurrentTicketColor = CurrentRollInfo.Color;
            rollInfoViewModelA.Show();
            rollInfoViewModelA.Closed += delegate(object senderA, RollInfoEventArgs argsA)
            {
                if (argsA == null) throw new NullReferenceException("Информация о смене и бабине не определена!");
                CurrentRollInfo = argsA.RollInfo ?? CurrentRollInfo;
            };
        }
        #endregion

        #region CloseShiftCommand
        private ICommand _closeShiftCommand;
        public ICommand CloseShiftCommand
        {
            get
            {
                return _closeShiftCommand ?? (_closeShiftCommand = new RelayCommand(param => OnCloseShiftCommand(), null));
            }
        }

        private void OnCloseShiftCommand()
        {
            var rollInfoViewModel = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            //rollInfoViewModel.Prepare("Укажите информацию о бабине", "Текущий билет", "Закрыть смену", true);
            rollInfoViewModel.Mode = ChildWindowMode.CloseShift;
            rollInfoViewModel.Show();
            rollInfoViewModel.Closed += delegate(object sender, RollInfoEventArgs args)
            {
                if (args == null || args.RollInfo == null) return;

                var loginViewModel = ObserveWrapperHelper.GetInstance().Resolve<LoginViewModel>();
                loginViewModel.Show();
                Close();
                Dispose();
            };
        }
        #endregion

        #region SendZPLRollCommand
        private ICommand _sendZPLRollCommand;
        public ICommand SendZPLRollCommand
        {
            get
            {
                return _sendZPLRollCommand ?? (_sendZPLRollCommand = new RelayCommand(param => OnSendZPLRollCommand(), null));
            }
        }

        private void OnSendZPLRollCommand()
        {
            var printerName = SettingsStore.Load().PrinterName;
            var zplPath = SettingsStore.Load().PathToZpl;
            RawPrinterHelper.SendFileToPrinter(printerName, zplPath);
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
                TicketNumber = CurrentTicketNumber,
                Attraction = attraction,
                TicketRoll = CurrentRollInfo
            });
        }

        private bool PrintTickets(IEnumerable<ICashVoucherItem> cashVoucher)
        {
            _chequeRows = new List<ChequeRow>();
            var settings = SettingsStore.Load();

            foreach (var item in cashVoucher.Where(item => item.Count >= 1))
            {
                String barcode;
                if(item.Count == 1)
                {
                    barcode = PrepareBarcode(CurrentTicketNumber);
                    var printed = SendToPrint(settings.PrinterName, item, barcode);
                    CreateChequeRow(printed, DateTime.Now, barcode, item.AttractionInfo);
                    
                    
                    if (!printed) return false;
                    
                    continue;
                }


                var i = 0;
                do
                {
                    i++;
                    barcode = PrepareBarcode(CurrentTicketNumber);
                    var printed = SendToPrint(settings.PrinterName, item, barcode);
                    CreateChequeRow(printed, DateTime.Now, barcode, item.AttractionInfo);



                    if (!printed) return false;
                } while (item.Count > i);
            }

            _cheque.CloseDate = DateTime.Now;
            return true;
        }

        private static String PrepareBarcode(long num)
        {
            var curTicketNum = num.ToString(CultureInfo.InvariantCulture);
            var curTicketLength = curTicketNum.Length;
            return curTicketLength > 13
                               ? curTicketNum.Remove(13, curTicketLength)
                               : curTicketNum.Insert(curTicketLength - 1, "0000000000000".Remove(0, 13 - curTicketLength));
        }

        private bool SendToPrint(String printerName, ICashVoucherItem item, String barcode)
        {
            IsShowErrorMessage = false;
            var pathToTemplate = SettingsStore.Load().PathToTemplate;
            try
            {
                RawPrinterHelper.SendStringToPrinter(printerName, ZebraHelper.LoadAndFillTemplate(pathToTemplate, CurrentDateTime.Date.ToString("dd.MM.yyyy"), item.Price.ToString(CultureInfo.InvariantCulture), item.PrintTitle, "", barcode));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
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

            return TicketsLeft > 0 || NeedChangeRoll();
        }

        private bool NeedChangeRoll()
        {
            var asyncChangingRoll = new AsyncChangingRoll(BeginChangingRoll);
            var result = asyncChangingRoll.BeginInvoke(null, null);

            while (result.IsCompleted == false)
                Thread.Sleep(1000);

            return asyncChangingRoll.EndInvoke(result);
        }

        public delegate bool AsyncChangingRoll();

        bool _isChangingRollCompleted;
        public bool BeginChangingRoll()
        {
            _isChangingRollCompleted = false;
            var rollInfoViewModelA = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModelA.Mode = ChildWindowMode.NeedNewRoll;
            rollInfoViewModelA.CurrentTicketSeries = CurrentTicketSeries;
            rollInfoViewModelA.CurrentTicketNumber = CurrentTicketNumber;
            rollInfoViewModelA.CurrentTicketColor = CurrentRollInfo.Color;
            rollInfoViewModelA.Show();
            rollInfoViewModelA.Closed += delegate(object sender, RollInfoEventArgs args)
            {
                if (args == null || args.RollInfo.IsNull()) throw new NullReferenceException("Информация о смене и бабине не определена!");
                CurrentRollInfo = args.RollInfo;
                _isChangingRollCompleted = true;
            };

            while (_isChangingRollCompleted == false)
            {
                //
            }

            return true;
        }
        #endregion
    }
}
