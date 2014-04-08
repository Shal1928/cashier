using ASofCP.Cashier.Models;
using ASofCP.Cashier.Stores.API;
using ASofCP.Cashier.Stores.Base;
using UseAbilities.IoC.Attributes;
using UseAbilities.IoC.Stores;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier.Stores
{
    public class POSInfoStore : CHessianStore<UserAPI>, IReadStore<POSInfo>
    {
        public POSInfoStore()
        {
            //
        }

        [InjectedProperty]
        public IXmlStore<ModuleSettings> SettingsStore
        {
            get;
            set;
        }

        public new POSInfo Load()
        {
            GetURL();
            return base.Load().getPOSInfo();
        }

        public new POSInfo Load(int key)
        {
            throw new System.NotImplementedException();
        }

        private void GetURL()
        {
            var settings = SettingsStore.Load();
            URL = string.Format("http://{0}:{1}/api/user?st={2}", settings.IP, settings.Port, settings.Id);
        }
    }
}
