using System;
using ASofCP.Cashier.Models.Base;

namespace ASofCP.Cashier.Models
{
    public sealed class CashVoucherItem : CashVoucherItemBase
    {
        public static CashVoucherItem Empty = new CashVoucherItem("", 0, 0);

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

        public override object Clone()
        {
            //var item = (CashVoucherItem) MemberwiseClone();
            return MemberwiseClone();
        }
    }
}
