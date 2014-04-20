using System;
using System.Globalization;
using UseAbilities.WPF.Converters.Base;

namespace ASofCP.Cashier.Converters
{
    public class BoolToOpacityConverter : ConverterBase<BoolToOpacityConverter>
    {
        public BoolToOpacityConverter()
        {
            //
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) throw new Exception("Value is not bool!");

            return (bool)value ? 1 : parameter;
        }
    }
}
