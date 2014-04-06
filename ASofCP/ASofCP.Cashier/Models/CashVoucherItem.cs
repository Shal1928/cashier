using System;
using ASofCP.Cashier.Models.Base;

namespace ASofCP.Cashier.Models
{
    public sealed class CashVoucherItem : CashVoucherItemBase
    {
        public CashVoucherItem(String title, double price)
        {
            Title = title;
            Price = price;
            Count = 1;
        }

        public CashVoucherItem(String title, double price, int count)
        {
            Title = title;
            Price = price;
            Count = count;
        }

        public CashVoucherItem(ParkService parkService)
        {
            if(parkService == null) return;

            Title = parkService.Title;
            Price = parkService.Price;
            Count = 1;
        }
    }
}
