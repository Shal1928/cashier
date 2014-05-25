using System;
using System.Collections.Specialized;
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

        public static bool IsPrinterBusy(string printerName)
        {
            var printJobCollection = new StringCollection();
            const string searchQuery = "SELECT * FROM Win32_PrintJob";

            /*searchQuery can also be mentioned with where Attribute,
                but this is not working in Windows 2000 / ME / 98 machines 
                and throws Invalid query error*/
            var searchPrintJobs = new ManagementObjectSearcher(searchQuery);
            var prntJobCollection = searchPrintJobs.Get();
            foreach (var prntJob in prntJobCollection)
            {
                var jobName = prntJob.Properties["Name"].Value.ToString();

                //Job name would be of the format [Printer name], [Job ID]
                var splitArr = new char[1];
                splitArr[0] = Convert.ToChar(",");
                var prnterName = jobName.Split(splitArr)[0];
                var documentName = prntJob.Properties["Document"].Value.ToString();
                if (String.Compare(prnterName, printerName, StringComparison.OrdinalIgnoreCase) == 0)
                    printJobCollection.Add(documentName);
                
            }

            return !printJobCollection.IsNullOrEmpty();
        }

        public static bool IsPlug(string printerName)
        {
            return true;

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
