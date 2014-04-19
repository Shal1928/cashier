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
        public IStore<ModuleSettings> SettingsStore
        {
            get;
            set;
        }

        public override string URL
        {
            get
            {
                var settings = SettingsStore.Load();
                return string.Format("http://{0}:{1}/api/user?st={2}", settings.IP, settings.Port, settings.Id);
            }
        }

        public new POSInfo Load()
        {
            return base.Load().getPOSInfo();
        }

        public new POSInfo Load(int key)
        {
            throw new System.NotImplementedException();
        }
    }
}
