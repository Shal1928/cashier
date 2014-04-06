using System.Windows;

namespace ASofCP.Cashier.Helpers
{
    public static class DependencyMechHelper
    {
        public static DependencyPropertyChangedEventArgs GetEventArgs(DependencyProperty dProperty, object oV, object nV)
        {
            return new DependencyPropertyChangedEventArgs(dProperty, oV, nV);
        }
    }
}
