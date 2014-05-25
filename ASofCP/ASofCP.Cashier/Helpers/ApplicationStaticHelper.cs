using System;

namespace ASofCP.Cashier.Helpers
{
    public static class ApplicationStaticHelper
    {
        public static bool IsValidExit { get; set; }

        public static bool IsCurrentRollDeactivated { get; set; }

        public static DateTime ShiftCloseDate { get; set; }
    }
}
