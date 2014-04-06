using System;
using System.Globalization;
using UseAbilities.WPF.Converters.Base;

namespace ASofCP.Cashier.Converters
{
    //Converter={Converters:PriceToTextConverter}

    public class PriceToTextConverter : ConverterBase<PriceToTextConverter>
    {
        public PriceToTextConverter()
        {
            //
        }

        #region Overrides of ConverterBase<PriceToTextConverter>

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double)) throw new Exception("Value is not double!");
            var price = (double)value;

            return price.ToString("# ##,0.00");
        }

        #endregion
    }
}
