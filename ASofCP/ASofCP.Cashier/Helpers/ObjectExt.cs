using System;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace ASofCP.Cashier.Helpers
{
    public static class ObjectExt
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        public static bool NotNull(this object obj)
        {
            return obj != null;
        }

        public static T GetValue<T>(this object obj, string propertyName)
        {
            var info = obj.GetType().GetProperty(propertyName);
            return (T)info.GetValue(obj, null);
        }

        //TODO: Make with Expression safe call Props or Meths with null check
        //public static bool SafeCheck(this object obj, )
    }
}
