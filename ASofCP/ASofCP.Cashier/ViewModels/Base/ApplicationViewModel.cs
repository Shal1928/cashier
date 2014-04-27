using System;
using System.Reflection;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Stores.API;
using ASofCP.Cashier.Stores.Base;
using UseAbilities.IoC.Attributes;
using UseAbilities.MVVM.Base;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels.Base
{
    public abstract class ApplicationViewModel : ViewModelBase
    {
        public String Title
        {
            get
            {
                #if LOGIN_DEBUG && PRINT_DEBUG
                const string mode = " [Режим полной отладки]";
                #elif LOGIN_DEBUG
                const string mode = " [Режим отладки авторизации]";
                #elif PRINT_DEBUG
                const string mode = " [Режим отладки печати]";
                #elif DEBUG
                const string mode = " [Режим отладки]";
                #else
                const string mode = "";
                #endif
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return String.Format("ASofCP модуль «Кассир» {0}{1}", version, mode);
            }
        }

        public bool Topmost
        {
            get
            {
                #if DEBUG
                return false;
                #else
                return true;
                #endif
            }
        }

        [InjectedProperty]
        public ISecureReadStore<BaseAPI> BaseAPIStore
        {
            get;
            set;
        }

        public BaseAPI BaseAPI { get { return BaseAPIStore.Load(); } }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                return _loadedCommand ?? (_loadedCommand = new RelayCommand(param => OnLoadedCommand(), null));
            }
        }

        protected abstract void OnLoadedCommand();
    }
}
