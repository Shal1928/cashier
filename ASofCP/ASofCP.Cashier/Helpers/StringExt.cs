using System;
using System.Drawing;
using System.Text;
using DColor = System.Drawing.Color;
using MColor = System.Windows.Media.Color;

namespace ASofCP.Cashier.Helpers
{
    public static class StringExt
    {
        public static DColor ToDrawingColor(this string colorName)
        {
            if (String.IsNullOrWhiteSpace(colorName)) throw new ArgumentException("colorName");
            return ColorTranslator.FromHtml(colorName);
        }

        public static MColor ToMediaColor(this string colorName)
        {
            if (String.IsNullOrWhiteSpace(colorName)) throw new ArgumentException("colorName");
            var dColor = ColorTranslator.FromHtml(colorName);
            return MColor.FromArgb(dColor.A, dColor.R, dColor.G, dColor.B);
        }

        public static string ToUTF8(this string str)
        {
            
            var encoder = Encoding.UTF8;
            var utfBytes = encoder.GetBytes(str);
            return encoder.GetString(utfBytes, 0, utfBytes.Length);     
        }

        public static string ToUTF32(this string str)
        {
            var encoder = Encoding.UTF32;
            var utfBytes = encoder.GetBytes(str);
            return encoder.GetString(utfBytes, 0, utfBytes.Length);
        }

        public static bool IsNullOrEmptyOrSpaces(this string value)
        {
            return String.IsNullOrWhiteSpace(value) || value.IsEmpty();
        }

        public static string F(this string value, params object[] parameters)
        {
            return String.Format(value, parameters);
        }
    }
}
