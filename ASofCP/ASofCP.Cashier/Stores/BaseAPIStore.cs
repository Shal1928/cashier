using ASofCP.Cashier.Models;
using ASofCP.Cashier.Stores.API;
using ASofCP.Cashier.Stores.Base;
using UseAbilities.IoC.Attributes;
using UseAbilities.IoC.Stores;

namespace ASofCP.Cashier.Stores
{
    public class BaseAPIStore : CHessianStore<BaseAPI>
    {
        public BaseAPIStore()
        {
            //
        }

        [InjectedProperty]
        public IReadStore<ModuleSettings> SettingsStore
        {
            get;
            set;
        }

        #region Implementation of IReadStore<POSInfo>

        public override string URL
        {
            get
            {
                var settings = SettingsStore.Load();
                return string.Format("http://{0}:{1}/api/user?st={2}", settings.IP, settings.Port, settings.Id);
            }
        }

        #endregion
    }
}
