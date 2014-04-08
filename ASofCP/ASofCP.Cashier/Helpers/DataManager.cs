using ASofCP.Cashier.Models;
using UseAbilities.IoC.Attributes;
using UseAbilities.IoC.Stores;

namespace ASofCP.Cashier.Helpers
{
    public class DataManager
    {
        [InjectedProperty]
        public IXmlStore<ModuleSettings> SettingsStore
        {
            get;
            set;
        }
    }
}
