using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Models.Base;
using ASofCP.Cashier.Stores;
using ASofCP.Cashier.ViewModels.Base;
using ASofCP.Cashier.ViewModels.ChildViewModels;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;
using it.q02.asocp.api.data;
using log4net;
using UseAbilities.Extensions.EnumExt;
using UseAbilities.IoC.Attributes;
using UseAbilities.IoC.Stores;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels
{
    public class MainViewModel : UtilViewModel
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainViewModel));

        private static IEnumerable<ICashVoucherItem> _cashVoucherToPrint;
        private int _backupIndex;
        private Dictionary<int, CashVoucher<ICashVoucherItem>> _backup = new Dictionary<int, CashVoucher<ICashVoucherItem>>();
        private Cheque _cheque;
        private int _currentOrder;

        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        public MainViewModel()
        {
            AppDomain.CurrentDomain.ProcessExit += delegate
            {
                Log.Debug("Деактивация ленты билетов и закрытие смены вызвано завершением работы процесса приложения!");
                ExtremeCloseShift();
            };

            var resultCashVoucher = new CashVoucher<ICashVoucherItem>();
            UpdateResultCashVoucher(resultCashVoucher);
            
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += delegate
                {
                    OnPropertyChanged(() => CurrentDateTime);
                };

            dispatcherTimer.Interval = new TimeSpan(0, 0, 30);
            dispatcherTimer.Start();
        }
        // ReSharper restore DoNotCallOverridableMethodsInConstructor

        [InjectedProperty]
        public IStore<ModuleSettings> SettingsStore { get; set; }
        [InjectedProperty]
        public IQueueStore<ChequeQueue> ChequeQueueStore { get; set; }
        private ChequeQueue _queue;

        private ModuleSettings Settings { get { return SettingsStore != null ? SettingsStore.Load() : null; } }
        private string PrinterName {get { return Settings != null ? Settings.PrinterName : string.Empty; }}

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
        public virtual DateTime CurrentDateTime { get { return DateTime.Now; } }
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
            _cashVoucherToPrint = (CashVoucher<ICashVoucherItem>)ResultCashVoucher.SourceCollection;

            var ticketsNeed = Math.Abs(TicketsLeft - _cashVoucherToPrint.Sum(item => item.Count));
            if (ticketsNeed < 0)
            {
                IsEnabled = false;
                var informationViewModel = ObserveWrapperHelper.GetInstance().Resolve<InformationViewModel>();
                informationViewModel.Count = ticketsNeed;
                informationViewModel.CurrentRollInfo = CurrentRollInfo;
                informationViewModel.Show();
                informationViewModel.Closed += delegate(object senderD, RollInfoEventArgs args)
                {
                    IsEnabled = true;
                    if (args.IsNull())
                    {
                        Log.Fatal("Информация о смене и бабине не определена! (Автоматический запрос на смену)");
                        throw new NullReferenceException("Информация о смене и бабине не определена!");
                    }

                    if (!args.IsCancel)
                    {
                        ChangeRollInfoToLog(args.RollInfo, true);
                        CurrentRollInfo = args.RollInfo;
                    }
                    else Log.Debug("Отмена автоматического запроса на смену билетов");

                    ResolvePaymentViewModel();
                };
            }
            else ResolvePaymentViewModel();
        }

        private static void PrintCashVoucherToLog(IEnumerable<ICashVoucherItem> cashVoucher)
        {
            var sb = new StringBuilder();
            sb.AppendLine("На печать отправлен чек");
            foreach (var item in cashVoucher)
                sb.AppendLine("\"{0}\" X {1} = {2}".F(item.Title, item.Count, item.Price));
            
            Log.Debug(sb);
        }

        private void ResolvePaymentViewModel()
        {
            IsShowErrorMessage = false;

            #if !DEBUG || PRINT_DEBUG
            if (!PrinterDeviceHelper.IsPlug(PrinterName))
            {
                var message = "Принтер {0} не подключен!".F(PrinterName);
                Log.Debug(message);
                RightErrorMessage = message;
                IsShowErrorMessage = true;
                return;
            }
            #endif

            IsEnabled = false;
            var paymentViewModel = ObserveWrapperHelper.GetInstance().Resolve<PaymentViewModel>();
            paymentViewModel.Total = Total;
            paymentViewModel.Show();
            paymentViewModel.PaymentReached += delegate(object sender, PaymentEventArgs args)
            {
                IsEnabled = true;
                if (!args.PaymentType.HasValue) return;

                _cheque.MoneyType = (short)args.PaymentType.Value;
                Log.Debug("Чек оплачен {0}.".F(args.PaymentType.Value.DescriptionOf()));
                _chequeRows = new List<ChequeRow>();

                PrintCashVoucherToLog(_cashVoucherToPrint);
                PrintTickets(PrinterName);
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
            IsEnabled = false;
            IsShowErrorMessage = false;
            var rollInfoViewModelA = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModelA.Mode = RollInfoViewModelMode.ChangeRoll;
            rollInfoViewModelA.CurrentRollInfo = CurrentRollInfo;
            rollInfoViewModelA.Show();
            rollInfoViewModelA.Closed += delegate(object senderA, RollInfoEventArgs argsA)
            {
                IsEnabled = true;

                if (argsA == null)
                {
                    Log.Fatal("Информация о смене и бабине не определена! (Смена ленты билетов)");
                    throw new NullReferenceException("Информация о смене и бабине не определена!");
                }

                if (argsA.IsCancel)
                {
                    Log.Debug("Отмена смены ленты билетов");
                    return;
                }

                var isChange = !Equals(CurrentRollInfo, argsA.RollInfo);
                CurrentRollInfo = argsA.RollInfo ?? CurrentRollInfo;
                ChangeRollInfoToLog(CurrentRollInfo, isChange);

                OnPropertyChanged(() => CurrentTicketNumber);
                OnPropertyChanged(() => CurrentTicketSeries);
                OnPropertyChanged(() => TicketsLeft);
            };
        }

        private static void ChangeRollInfoToLog(RollInfo r, bool isChange)
        {
            if (r.IsNull())
            {
                Log.Warn("Лента билетов не определена!");
                return;    
            }

            var sb = new StringBuilder();
            var firstPart = isChange ? "Произошла смена ленты билетов" : "Текущая лента билетов";
            sb.AppendLine(firstPart);
            sb.AppendLine("{0} {1} {2}".F(r.Series, r.NextTicket, r.Color.Color));
            sb.AppendLine("Состояние: {0}; Осталось билетов: {1};".F(r.IsActiveOnStation ? "Активирована" : "Деактивирована", r.TicketsLeft));
            Log.Debug(sb);
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
            IsEnabled = false;
            var rollInfoViewModel = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModel.Mode = RollInfoViewModelMode.CloseShift;
            rollInfoViewModel.CurrentRollInfo = CurrentRollInfo;
            rollInfoViewModel.Show();
            rollInfoViewModel.Closed += delegate(object sender, RollInfoEventArgs args)
            {
                IsEnabled = true;

                if (args == null) return;

                if (args.IsCancel)
                {
                    Log.Debug("Отмена закрытия смены");
                    return;
                }

                ChangeRollInfoToLog(CurrentRollInfo, false);
                Log.Debug("Смена закрыта {0} – {1} {2}".F(CurrentShift.OpenDate, CurrentShift.CloseDate, CurrentShift.CashierName));

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
            //_isTerminate = false;
            IsEnabled = false;
            //TODO: Переопределить Show
            Show();
            var rollInfoViewModel = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModel.Mode = RollInfoViewModelMode.OpenShift;
            rollInfoViewModel.Show();
            rollInfoViewModel.Closed += delegate(object sender, RollInfoEventArgs args)
            {
                IsEnabled = true;

                if (args.IsNull() || args.IsCancel || args.RollInfo.IsNull())
                {
                    Log.Debug("Отмена открытия сессии");
                    var loginViewModel = ObserveWrapperHelper.GetInstance().Resolve<LoginViewModel>();
                    loginViewModel.Show();
                    Close();
                    Dispose();
                    return;
                }

                CurrentRollInfo = args.RollInfo;
                CurrentShift = args.Shift;
                 
                OnPropertyChanged(() => CurrentTicketNumber);
                OnPropertyChanged(() => CurrentTicketSeries);
                OnPropertyChanged(() => TicketsLeft);
                ChangeRollInfoToLog(CurrentRollInfo, false);

                var collectionServices = new GroupContentList();
                var attractions = BaseAPI.getAttractionsFromGroup(new AttractionGroupInfo());
                if (attractions.IsNullOrEmpty()) return;
                collectionServices.AddRange(attractions.OrderBy(i => i.DisplayName).Select(attraction => new ParkService(attraction)));

                CollectionServices = collectionServices;

                Log.Debug("Смена открыта {0} {1}", CurrentShift.OpenDate, CurrentShift.CashierName);
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
            Log.Debug("Напечатана позиция чека {0} {1} {2} ".F(attraction.DisplayName, CurrentTicketNumber - 1, CurrentRollInfo.NextTicket));
        }

        private void CloseCheque()
        {
            _cheque.CloseDate = DateTime.Now;
            _cheque.Rows = _chequeRows.ToArray();

            SendChequeToServer(_cheque);

            UpdateResultCashVoucher(new CashVoucher<ICashVoucherItem>());
            Total = 0;
            _cheque = null;
            _currentOrder = 0;
            _cashVoucherToPrint = null;
        }

        private void SendChequeToServer(Cheque cheque)
        {
            _queue = ChequeQueueStore.LoadAll();
            _queue.Add(cheque);
            try
            {
                foreach (var element in _queue.Elements)
                {
                    var currentCheque = element.Cheques.First();
                    BaseAPI.createCheque(currentCheque);
                    element.IsDelete = true;
                    ChequeToLog(currentCheque);
                }
            }
            catch (Exception e)
            {
                Log.Fatal(e);

                ChequeQueueStore.Save(_queue);
                ChequeToLog(cheque, true);
                //throw;
            }

            ChequeQueueStore.Delete(_queue);
        }

        private static void ChequeToLog(Cheque c, bool isInQueue = false)
        {
            if (c.IsNull())
            {
                Log.Warn("Чек не определен!");
                return;
            }
            
            var sb = new StringBuilder();
            var chequeStatus = isInQueue ? "в очереди" : "закрыт";
            sb.AppendLine("Чек {0}".F(chequeStatus));
            string paymentType;
            switch (c.MoneyType)
            {
                case 0: paymentType = "Наличные"; break;
                case 1: paymentType = "Безналичный расчет"; break;
                case 2: paymentType = "Сертификат"; break;
                case 66: paymentType = "Списание"; break;
                default: paymentType = "Неопознаный тип оплаты"; break;
            }
            sb.AppendLine("{0} – {1}; {2}".F(c.OpenDate, c.CloseDate, paymentType));
            sb.AppendLine("Смена открыта {0} {1}".F(c.Shift.OpenDate, c.Shift.CashierName));
            if ((short)PaymentTypes.WriteOff == c.MoneyType)
            {
                sb.AppendLine("Списанные билеты:");
                foreach (var row in c.Rows)
                    sb.AppendLine("{0} {1} {2}".F(row.TicketRoll.Series, row.TicketNumber, row.TicketRoll.Color.Color));
            }
            else
            {
                sb.AppendLine("Позиции чека:");
                foreach (var row in c.Rows)
                {
                    sb.AppendLine("Напечатано {0}; {1} {2} {3}".F(row.PrintDate, row.TicketNumber, row.TicketBarCode, row.TicketRoll.Color.Color));
                    sb.AppendLine("\"{0}\" {1} = {2}".F(row.Attraction.DisplayName, row.Attraction.Code, row.Attraction.Price));
                    sb.AppendLine("---");
                }
            }

            Log.Debug(sb);
        }

        private void PrintTickets(string printerName)
        {
            foreach (var item in _cashVoucherToPrint.Where(item => item.Count >= 1 && !item.IsPrinted))
            {
                if(item.Count == 1)
                {
                    if (!ProcessingPrint(printerName, item)) return;
                    continue;
                }

                do
                {
                    if (!ProcessingPrint(printerName, item)) return;
                } while (!item.IsPrinted);
            }

            if(_cashVoucherToPrint.All(item=> item.IsPrinted)) CloseCheque();
        }

        private bool ProcessingPrint(string printerName, ICashVoucherItem item)
        {
            var barcode = PrepareBarcode(CurrentTicketNumber);
            var printed = SendToPrint(printerName, item, barcode);
            CreateChequeRow(printed.IsSuccess, DateTime.Now, barcode, item.AttractionInfo);
            if (item.Count > 1) item.Count--;
            else item.IsPrinted = printed.IsSuccess;

            Log.Debug("Результат печати Успешно: {0} Ошибка: {1}  Требовалась смена: {2}".F(printed.IsSuccess, printed.HasError, printed.IsNeedNewTicketRoll));

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
            try
            {
                #if !DEBUG || PRINT_DEBUG
                if(!PrinterDeviceHelper.IsPlug(printerName)) throw new Exception("Принтер {0} не подключен!".F(printerName));
                var pathToTemplate = SettingsStore.Load().PathToTemplate;
                RawPrinterHelper.SendStringToPrinter(printerName, ZebraHelper.LoadAndFillTemplate(pathToTemplate, CurrentDateTime.ToString("dd.MM.yyyy HH:mm:ss"), item.Price.ToString(CultureInfo.InvariantCulture), item.PrintTitle, "", barcode));
                #endif
            }
            catch (Exception e)
            {
                PrintTroubleToLog(e);
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
                RightErrorMessage = "Заканчиваются билеты! Осталось: {0}".F(TicketsLeft);
                IsShowErrorMessage = true;
            }

            return TicketsLeft > 0 ? PrintResult.Success : PrintResult.SuccessAndNeedNewTicketRoll;
        }

        private void PrintTroubleToLog(Exception e)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Во время печати возникли проблемы!");
            sb.AppendLine(e.Message);
            sb.AppendLine(e.StackTrace);

            if (_cashVoucherToPrint.IsNullOrEmpty())
            {
                Log.Fatal(sb);
                return;
            }

            sb.AppendLine("Не напечатанными остались следующие позиции:");
            foreach (var item in _cashVoucherToPrint.Where(item => item.Count >= 1 && !item.IsPrinted))
                sb.AppendLine("\"{0}\" X {1} = {2}".F(item.Title, item.Count, item.Price));

            Log.Fatal(sb);
        }

        private void NeedChangeRoll()
        {
            IsEnabled = false;
            var rollInfoViewModelA = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModelA.Mode = RollInfoViewModelMode.NeedNewRoll;
            rollInfoViewModelA.CurrentRollInfo = CurrentRollInfo;
            rollInfoViewModelA.Show();
            rollInfoViewModelA.Closed += delegate(object sender, RollInfoEventArgs args)
            {
                IsEnabled = true;
                if (args.IsNull() || args.RollInfo.IsNull())
                {
                    Log.Fatal("Информация о смене и бабине не определена! (Необходимая смена ленты билетов)");
                    throw new NullReferenceException("Информация о смене и бабине не определена!");
                }

                CurrentRollInfo = args.RollInfo;
                ChangeRollInfoToLog(CurrentRollInfo, true);
                OnPropertyChanged(() => CurrentTicketNumber);
                OnPropertyChanged(() => CurrentTicketSeries);
                OnPropertyChanged(() => TicketsLeft);
                PrintTickets(PrinterName); 
            };
        }
        #endregion

        protected override void OnSettingsCommand()
        {
            IsEnabled = false;
            var settingsVM = ObserveWrapperHelper.GetInstance().Resolve<SettingsViewModel>();
            settingsVM.Show();
            settingsVM.CloseEventHandler += delegate
            {
                IsEnabled = true;
            };
        }

        #region WriteOffTicketsCommand
        private ICommand _writeOffTicketsCommand;
        public ICommand WriteOffTicketsCommand
        {
            get
            {
                return _writeOffTicketsCommand ?? (_writeOffTicketsCommand = new RelayCommand(param => OnWriteOffTicketsCommand(), can => ValidateWriteOffTicketsCommand()));
            }
        }

        private void OnWriteOffTicketsCommand()
        {
            IsEnabled = false;
            var ticketWriteOffViewModel = ObserveWrapperHelper.GetInstance().Resolve<TicketWriteOffViewModel>();
            ticketWriteOffViewModel.Show(CurrentRollInfo);
            ticketWriteOffViewModel.Closed += delegate(object sender, WriteOffEventArgs args)
            {
                IsEnabled = true;

                if (args == null || args.ChequeRows.IsNullOrEmpty())
                {
                    Log.Debug("Отмена списания порченых билетов.");
                    return;
                }

                var writeOffCheque = new Cheque
                {
                    MoneyType = (short)PaymentTypes.WriteOff,
                    OpenDate = DateTime.Now,
                    CloseDate = DateTime.Now,
                    Rows = args.ChequeRows.ToArray(),
                    Shift = CurrentShift
                };

                SendChequeToServer(writeOffCheque);
                
                OnPropertyChanged(() => CurrentTicketNumber);
                OnPropertyChanged(() => TicketsLeft);
            };
        }

        private bool ValidateWriteOffTicketsCommand()
        {
            return CurrentRollInfo.NotNull() && CurrentShift.NotNull() && CurrentShift.Active;
        }
        #endregion

        #region ClosedCommand
        private ICommand _closedCommand;
        public ICommand ClosedCommand
        {
            get
            {
                return _closedCommand ?? (_closedCommand = new RelayCommand(param => OnClosedCommand(), null));
            }
        }

        private static void OnClosedCommand()
        {
            Application.Current.Shutdown();
        }
        #endregion

        private void ExtremeCloseShift()
        {
            IsShowErrorMessage = false;

            if (BaseAPI == null)
            {
                Log.Debug("BaseAPI не определен. Возможно смена не будет закрыта, а лента билетов деактивирована!");
                return;
            }

            try
            {
                if (CurrentRollInfo != null && CurrentRollInfo.IsActiveOnStation)
                    if (BaseAPI.deactivateTicketRoll(CurrentRollInfo.Series, CurrentRollInfo.NextTicket, CurrentRollInfo.Color))
                        Log.Debug("Лента билетов {0} {1} {2} деактивирована.", CurrentRollInfo.Series, CurrentRollInfo.NextTicket, CurrentRollInfo.Color.Color);
                    else
                    {
                        Log.Fatal("Деактивировать ленту билетов {0} {1} {2} не получилось!", CurrentRollInfo.Series, CurrentRollInfo.NextTicket, CurrentRollInfo.Color.Color);
                        
                        return;
                    }
                
                if (BaseAPI.isShiftOpen()) BaseAPI.closeShift(BaseAPI.getCurrentShift());
            }
            catch (Exception e)
            {
                RightErrorMessage = "При закрытии смены, возникло исключение. Попробуйте снова!";
                IsShowErrorMessage = true;
                Log.Fatal(e);
                //throw;
            }
        }
    }
}
