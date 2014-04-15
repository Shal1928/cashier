using System.ComponentModel;

namespace ASofCP.Cashier.Models
{
    public enum PaymentTypes : short
    {
        [Description("Наличный")]
        Cash = 0,
        [Description("Безналичный")]
        Cashless = 1,
        [Description("Сертификат")]
        Certificate = 2
    }
}
