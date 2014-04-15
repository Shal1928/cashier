using System;
using System.Globalization;
using UseAbilities.WPF.Converters.Base;

namespace ASofCP.Cashier.Helpers.Test
{
    public class TestConverter : ConverterBase<TestConverter>
    {
        #region Ovveride of ConverterBase

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Console.WriteLine(value.ToString());
            return value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
