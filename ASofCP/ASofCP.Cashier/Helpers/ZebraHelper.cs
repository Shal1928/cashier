using System;

namespace ASofCP.Cashier.Helpers
{
    public static class ZebraHelper
    {
        public static string FillTemplate(String date, String price, String title, String description, String barcode)
        {
            return
                String.Format("<?xml version=\"1.0\" standalone=\"no\"?><!DOCTYPE labels SYSTEM \"label.dtd\"><labels _FORMAT=\"E:ZEBRARU.ZPL\" _QUANTITY=\"1\" _PRINTERNAME=\"Printer 1\" _JOBNAME=\"LBL101\"><label><variable name=\"Price\">{1}</variable><variable name=\"Date\">{0}</variable><variable name=\"Title\">{2}</variable><variable name=\"Barcode\">{4}</variable></label></labels>", date, price, title, description, barcode);
        }
    }
}
