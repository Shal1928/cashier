using System;
using System.Globalization;
using UseAbilities.WPF.Converters.Base;

namespace ASofCP.Cashier.Views.Controls.GroupContentGridParts.Converters
{
    //Converter={Converters:EmptyToBoolConverter}

    public class EmptyToBoolConverter : ConverterBase<EmptyToBoolConverter>
    {
        public EmptyToBoolConverter()
        {
            //
        }

        #region Overrides of ConverterBase<EmptyToBoolConverter>

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is String)) throw new Exception("Value is not String!");
            var str = value as String;

            return !String.IsNullOrWhiteSpace(str);
        }

        #endregion
    }
}
