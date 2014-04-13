using System.ComponentModel;

namespace ASofCP.Cashier.Models
{
    public enum PaymentTypes
    {
        [Description("Наличный")]
        Cash = 0,
        [Description("Безналичный")]
        Cashless,
        [Description("Сертификат")]
        Certificate
    }
}
