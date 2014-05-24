using System;
using System.Text;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier.Helpers
{
    public static class BarcodeHelper
    {
        private static readonly Random _random = new Random();

        public static string GetBarcode(params object[] parameters)
        {
            var sb = new StringBuilder();
            foreach (var param in parameters)
                sb.Append(param);

            return sb.ToString();
        }

        public static string GetBarcode(long currentNum, RollInfo roll)
        {
            return GetBarcode(roll.Color.Id, roll.Series.ConvertToASCIICodes(), currentNum, _random.Next(0, 9));
            //[ID ЦВЕТА][СЕРИЯ-НОМЕР][СЛУЧАЙНОЕ ЧИСЛО]
        }
    }
}
