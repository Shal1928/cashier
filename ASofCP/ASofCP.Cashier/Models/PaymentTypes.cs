using System.ComponentModel;

namespace ASofCP.Cashier.Models
{
    public enum PaymentTypes : short
    {
        [Description("наличными")]
        Cash = 0,
        [Description("безналом")]
        Cashless = 1,
        [Description("сертификатом")]
        Certificate = 2,
        [Description("списание")]
        WriteOff = 66
    }
}
