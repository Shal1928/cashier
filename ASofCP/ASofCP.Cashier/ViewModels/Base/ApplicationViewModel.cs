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
            get { return String.Format("ASofCP модуль «Кассир» {0}", Assembly.GetExecutingAssembly().GetName().Version); }
        }

        public bool Topmost
        {
            get { return !DebugHelper.IsDebug; }
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
