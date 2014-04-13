﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Helpers.Test;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Models.Base;
using ASofCP.Cashier.Models.Contracts;
using ASofCP.Cashier.ViewModels.Base;
using ASofCP.Cashier.ViewModels.ChildViewModels;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;
using UseAbilities.IoC.Attributes;
using UseAbilities.IoC.Stores;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels
{
    public class MainViewModel : ApplicationViewModel
    {
        private int _backupIndex;
        private Dictionary<int, CashVoucher<ICashVoucherItem>> _backup = new Dictionary<int, CashVoucher<ICashVoucherItem>>();


        public MainViewModel()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            CollectionServices = TestDataHelper.GetParkServices();
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
        public IReadStore<ModuleSettings> SettingsStore
        {
            get;
            set;
        }

        public virtual GroupContentList CollectionServices { get; set; }
        public virtual double Total { get; set; }
        public virtual ICollectionView ResultCashVoucher { get; set; }
        public virtual int? NewCount { get; set; }
        public virtual String CurrentTicketSeries { get; set; }
        public virtual long CurrentTicketNumber { get; set; }
        public virtual DateTime CurrentDateTime { get; set; }

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

                ICashVoucherItem item = new CashVoucherItem(_selectedParkService);
                var cashVoucherItem = (CashVoucher<ICashVoucherItem>) ResultCashVoucher.SourceCollection;
                // ReSharper disable PossibleMultipleEnumeration
                cashVoucherItem.Add(item);
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
            var paymentViewModel = ObserveWrapperHelper.GetInstance().Resolve<PaymentViewModel>();
            paymentViewModel.Total = Total;
            paymentViewModel.Show();
            paymentViewModel.PaymentReached += delegate(object sender, PaymentEventArgs args)
                {
                    if (!args.PaymentType.HasValue) return;

                    if (!PrintTickets((CashVoucher<ICashVoucherItem>) ResultCashVoucher.SourceCollection)) return;

                    UpdateResultCashVoucher(new CashVoucher<ICashVoucherItem>());
                    Total = 0;
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
            var rollInfoViewModel = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModel.Prepare("Укажите информацию о бабине", "Первый билет", "Сменить бабину", true);
            rollInfoViewModel.Show();
            rollInfoViewModel.Closed += delegate(object sender, RollInfoEventArgs args)
            {
                if (args == null || args.RollInfo == null) return;
                CurrentRollInfo = args.RollInfo;
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
            rollInfoViewModel.Prepare("Укажите информацию о бабине", "Текущий билет", "Закрыть смену", true);
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

        public void OpenSession()
        {
            Show();
            var rollInfoViewModel = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
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

        private void UpdateResultCashVoucher(CashVoucher<ICashVoucherItem> cashVoucher)
        {
            var view = CollectionViewSource.GetDefaultView(cashVoucher);
            if (view == null) return;
            view.Filter = null;

            ResultCashVoucher = view;
            ResultCashVoucher.Refresh();
        }

        private bool PrintTickets(IEnumerable<ICashVoucherItem> cashVoucher)
        {
            var settings = SettingsStore.Load();

            foreach (var item in cashVoucher.Where(item => item.Count >= 1))
            {
                String barcode;
                if(item.Count == 1)
                {
                    barcode = PrepareBarcode(CurrentTicketNumber);
                    if (!SendToPrint(settings.PrinterName, item, barcode)) return false;
                    continue;
                }


                var i = 0;
                do
                {
                    i++;
                    barcode = PrepareBarcode(CurrentTicketNumber);
                    if (!SendToPrint(settings.PrinterName, item, barcode)) return false;
                } while (item.Count > i);
            }

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
            try
            {
                RawPrinterHelper.SendStringToPrinter(printerName, ZebraHelper.FillTemplate(CurrentDateTime.Date.ToString("dd.MM.yyyy"), item.Price.ToString(CultureInfo.InvariantCulture), item.Title, "", barcode));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            finally
            {
                CurrentTicketNumber++;
            }

            return true;
        }
        #endregion
    }
}
