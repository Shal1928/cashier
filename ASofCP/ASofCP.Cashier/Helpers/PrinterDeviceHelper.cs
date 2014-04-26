using System;
using System.Linq;
using System.Management;

namespace ASofCP.Cashier.Helpers
{
    public static class PrinterDeviceHelper
    {
        public static bool IsPlug(string printerName)
        {
            if (printerName.IsNullOrEmptyOrSpaces()) return false;

            var scope = new ManagementScope(@"\root\cimv2");
            scope.Connect();

            var allPrinters = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
            return (from ManagementBaseObject printer in allPrinters.Get() 
                    let name = printer.GetPropertyValue("Name").ToString() 
                    where String.Equals(printerName, name, StringComparison.InvariantCultureIgnoreCase) 
                    select (bool) printer.GetPropertyValue("WorkOffline")).FirstOrDefault();
        }
    }
}
