using System.Text;

namespace ASofCP.Cashier.Helpers
{
    public static class StringExt
    {
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
    }
}
