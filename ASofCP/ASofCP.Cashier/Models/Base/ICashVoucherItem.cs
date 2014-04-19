using System;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier.Models.Base
{
    public interface ICashVoucherItem : ICloneable
    {
        String Title
        {
            get;
            set;
        }

        String PrintTitle
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

        bool IsPrinted { get; set; }

        int Order { get; set; }

        AttractionInfo AttractionInfo { get; }
    }
}
