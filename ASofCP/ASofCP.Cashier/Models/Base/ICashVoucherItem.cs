using System;

namespace ASofCP.Cashier.Models.Base
{
    public interface ICashVoucherItem
    {
        String Title
        {
            get;
            set;
        }

        double Price
        {
            get;
            set;
        }

        int Count
        {
            get;
            set;
        }

        double Total
        {
            get;
        }
    }
}
