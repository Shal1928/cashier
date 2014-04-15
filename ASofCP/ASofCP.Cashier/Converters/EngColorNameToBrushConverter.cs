using System;
using System.Globalization;
using System.Windows.Media;
using ASofCP.Cashier.Helpers;
using it.q02.asocp.api.data;
using UseAbilities.WPF.Converters.Base;

namespace ASofCP.Cashier.Converters
{
    public class EngColorNameToBrushConverter : ConverterBase<EngColorNameToBrushConverter> 
    {
        public EngColorNameToBrushConverter()
        {
            //
        }

        #region Overrides of ConverterBase<EngColorNameToBrushConverter>

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //if (!(value is string)) throw new Exception("Value is not string!");

            var colorName = value as RollColor;
            return colorName == null ? null : new SolidColorBrush {Color = colorName.Color.ToMediaColor()};
        }

        #endregion
    }
}
