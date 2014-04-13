using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UseAbilities.WPF.Converters.Base;

namespace ASofCP.Cashier.Converters
{
    public class UniversalDecimalDelimetrConverter : ConverterBase<UniversalDecimalDelimetrConverter>
    {
        #region Overrides of ConverterBase<UniversalDecimalDelimetrConverter>

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is double)) throw new Exception("Value is not double!");
            var price = (double)value;

            return price.ToString(CultureInfo.InvariantCulture).Replace('.',',');
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value as string;
            if (strValue == null) return 0;

            return strValue.Replace(',', '.');
        }
        #endregion
    }
}
