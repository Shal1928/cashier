using System;
using System.Management;
using log4net;

namespace ASofCP.Cashier.Helpers
{
    public static class PrinterDeviceHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PrinterDeviceHelper));

        public static string[] ExtendedPrinterStatus = { 
            "","Other", "Unknown", "Idle", "Printing", "Warming Up",
            "Stopped Printing", "Offline", "Paused", "Error", "Busy",
            "Not Available", "Waiting", "Processing", "Initialization",
            "Power Save", "Pending Deletion", "I/O Active", "Manual Feed"
        };

        public static string[] ErrorState = {
            "Unknown", "Other", "No Error", "Low Paper", "No Paper", "Low Toner",
            "No Toner", "Door Open", "Jammed", "Offline", "Service Requested",
            "Output Bin Full"
        };

        //public static bool IsPlug(string printerName)
        //{
        //    if (printerName.IsNullOrEmptyOrSpaces()) return false;

        //    var scope = new ManagementScope(@"\root\cimv2");
        //    scope.Connect();

        //    var allPrinters = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
        //    return (from ManagementBaseObject printer in allPrinters.Get() 
        //            let name = printer.GetPropertyValue("Name").ToString() 
        //            where String.Equals(printerName, name, StringComparison.InvariantCultureIgnoreCase) 
        //            select (bool) printer.GetPropertyValue("WorkOffline")).FirstOrDefault();
        //}

        public static bool IsPlug(string printerName)
        {
            if (printerName.IsNullOrEmptyOrSpaces()) return false;

            var scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();

            var query = new ObjectQuery("SELECT ExtendedPrinterStatus FROM Win32_Printer WHERE Name =\"{0}\"".F(printerName));
            var searcher = new ManagementObjectSearcher(scope, query);
            var collection = searcher.Get();

            foreach (var obj in collection)
            {
                Log.Debug("Статус принтера (не обработанный) {0}", obj["ExtendedPrinterStatus"].ToString());
                var status = Int32.Parse(obj["ExtendedPrinterStatus"].ToString());
                Log.Debug("Статус принтера {0}", ExtendedPrinterStatus[status]);
                if (status == 7 || status == 9 || status == 11) return false;
            }

            return true;
        }
    }
}
