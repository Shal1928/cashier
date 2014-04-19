using ASofCP.Cashier.Helpers;
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
            if (!DebugHelper.IsDebug) return;
            Logon("administrator", "1");
        }

        [InjectedProperty]
        public IStore<ModuleSettings> SettingsStore
        {
            get;
            set;
        }

        #region Implementation of IReadStore<BaseAPI>

        public override string URL
        {
            get
            {
                var settings = SettingsStore.Load();
                return string.Format("http://{0}:{1}/api/security?st={2}", settings.IP, settings.Port, settings.Id);
            }
        }

        #endregion
    }
}
