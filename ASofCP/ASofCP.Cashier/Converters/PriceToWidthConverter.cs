using System;
using System.Globalization;
using UseAbilities.WPF.Converters.Base;

namespace ASofCP.Cashier.Converters
{
    public class PriceToWidthConverter : ConverterBase<PriceToWidthConverter>
    {
        #region Overrides of ConverterBase<PriceToWidthConverter>

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double)) throw new Exception("Value is not double!");

            return (double)value + 110;
        }

        #endregion
    }
}
