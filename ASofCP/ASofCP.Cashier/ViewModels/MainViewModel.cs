using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Helpers.Test;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Models.Base;
using ASofCP.Cashier.ViewModels.Base;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels
{
    public class MainViewModel : ApplicationViewModel
    {
        public MainViewModel()
        {
            CollectionServices = TestDataHelper.GetParkServices();
            var resultCashVoucher = new CashVoucher<ICashVoucherItem>();
            UpdateResultCashVoucher(resultCashVoucher);
        }

        public virtual GroupContentList CollectionServices
        {
            get; 
            set;
        }

        public virtual double Total { get; set; }

        //CashVoucher<ICashVoucherItem> 
        public virtual ICollectionView ResultCashVoucher
        {
            get; 
            set;
        }

        //public virtual ICashVoucherItem SelectedCashVoucher
        //{
        //    get; 
        //    set;
        //}

        private int _backupIndex;
        private Dictionary<int, CashVoucher<ICashVoucherItem>> _backup = new Dictionary<int, CashVoucher<ICashVoucherItem>>();
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


        private void UpdateResultCashVoucher(CashVoucher<ICashVoucherItem> cashVoucher)
        {
            var view = CollectionViewSource.GetDefaultView(cashVoucher);
            if (view == null) return;
            view.Filter = null;

            ResultCashVoucher = view;
            ResultCashVoucher.Refresh();
        }

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

        public virtual int? NewCount
        {
            get; 
            set;
        }

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
                cashVoucherItem.Add(item);
                SelectedVoucherItem = cashVoucherItem.Get(item);
                Total = cashVoucherItem.GetTotal();
                ResultCashVoucher.Refresh();
            }
        }

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
            //
        }

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

        private static int CalculateBackupIndex(int targetBackupIndex)
        {
            if (targetBackupIndex > 0 && targetBackupIndex < 11) return targetBackupIndex;
            return targetBackupIndex <= 0 ? 10 : 1;
        }
    }
}
