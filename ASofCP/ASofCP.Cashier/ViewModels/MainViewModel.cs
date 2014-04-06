using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Helpers.Test;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Models.Base;
using ASofCP.Cashier.ViewModels.Base;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;

namespace ASofCP.Cashier.ViewModels
{
    public class MainViewModel : ApplicationViewModel
    {
        public MainViewModel()
        {
            CollectionServices = TestDataHelper.GetParkServices();
            ResultCashVoucherItem = new CashVoucher<ICashVoucherItem>();

        }

        public virtual GroupContentList CollectionServices
        {
            get; 
            set;
        }

        public virtual double Total { get; set; }

        public virtual CashVoucher<ICashVoucherItem> ResultCashVoucherItem
        {
            get; 
            set;
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
                ResultCashVoucherItem.Add(item);
                SelectedVoucherItem = ResultCashVoucherItem.Get(item);
                Total = ResultCashVoucherItem.GetTotal();
                OnPropertyChanged(()=>ResultCashVoucherItem);
            }
        }
    }
}
