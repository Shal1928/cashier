using System.ComponentModel;
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

        private void UpdateResultCashVoucher(CashVoucher<ICashVoucherItem> cashVoucher)
        {
            var view = CollectionViewSource.GetDefaultView(cashVoucher);
            if (view == null) return;
            view.Filter = null;

            ResultCashVoucher = view;
        }

        public virtual ICashVoucherItem SelectedVoucherItem
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
        
    }
}
