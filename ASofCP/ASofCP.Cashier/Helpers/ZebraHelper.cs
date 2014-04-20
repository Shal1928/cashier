using System;
using System.IO;
using System.Text;

namespace ASofCP.Cashier.Helpers
{
    public static class ZebraHelper
    {
        public static string FillTemplate(String date, String price, String title, String description, String barcode)
        {
            return
                String.Format("<?xml version=\"1.0\" standalone=\"no\"?><!DOCTYPE labels SYSTEM \"label.dtd\"><labels _FORMAT=\"E:ZEBRARU.ZPL\" _QUANTITY=\"1\" _PRINTERNAME=\"Printer 1\" _JOBNAME=\"LBL101\"><label><variable name=\"Price\">{1}</variable><variable name=\"Date\">{0}</variable><variable name=\"Title\">{2}</variable><variable name=\"Barcode\">{3}</variable></label></labels>", date, price, title, barcode, description);
        }

        public static string LoadAndFillTemplate(String templatePath, String date, String price, String title, String description, String barcode)
        {
            String template;
            using (var fsSource = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                // Read the source file into a byte array. 
                var bytes = new byte[fsSource.Length];
                var numBytesToRead = (int) fsSource.Length;
                var numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead. 
                    var n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached. 
                    if (n == 0) break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }
                var uniEncoding = Encoding.UTF8;
                template = uniEncoding.GetString(bytes);
            }

            template = template.Replace("{0}", date);
            template = template.Replace("{1}", price);
            template = template.Replace("{2}", title);
            template = template.Replace("{3}", barcode);
            if(template.Contains("{4}")) template = template.Replace("{4}", description);

            return template;
        }
    }
}
