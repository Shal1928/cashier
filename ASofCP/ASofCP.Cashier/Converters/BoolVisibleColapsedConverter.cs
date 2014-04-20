using System;
using System.Globalization;
using System.Windows;
using UseAbilities.WPF.Converters.Base;

namespace ASofCP.Cashier.Converters
{
    public class BoolVisibleColapsedConverter : ConverterBase<BoolVisibleColapsedConverter>
    {
        public BoolVisibleColapsedConverter()
        {
            //
        }

        #region Overrides of ConverterBase<AuthorityToVisibilityConverter>

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) throw new Exception("Value is not bool!");

            return (bool) value ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility)) throw new Exception("Value is not Visibility!");

            return (Visibility)value == Visibility.Visible;
        }

        #endregion
    }
}
