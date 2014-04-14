using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Stores.API;
using ASofCP.Cashier.Stores.Base;
using ASofCP.Cashier.ViewModels.Base;
using UseAbilities.IoC.Attributes;
using UseAbilities.IoC.Helpers;
using UseAbilities.IoC.Stores;
using UseAbilities.MVVM.Base;
using UseAbilities.MVVM.Command;
using it.q02.asocp.api.data;
using System.Linq;

namespace ASofCP.Cashier.ViewModels
{
    public class LoginViewModel : ApplicationViewModel
    {
        public LoginViewModel()
        {
            //Users = new ObservableCollection<UserCS>
            //    {
            //        "Константин Константинович Константинопольский", 
            //        "Анна Юрьевна Агейман"
            //    };
        }

        [InjectedProperty]
        public IReadStore<POSInfo> POSInfoStore
        {
            get;
            set;
        }

        [InjectedProperty]
        public ISecureReadStore<BaseAPI> BaseAPIStore
        {
            get;
            set;
        }

        public virtual ObservableCollection<UserCS> Users
        {
            get;
            set;
        }

        public virtual String Password
        {
            get; 
            set;
        }

        public virtual bool IsAuthority
        {
            get; 
            
            set;
        }

        public virtual UserCS User
        {
            get; 
            set;
        }

        

        private ICommand _enterCommand;
        public ICommand EnterCommand
        {
            get
            {
                return _enterCommand ?? (_enterCommand = new RelayCommand(param => OnEnterCommand(), null));
            }
        }

        private void OnEnterCommand()
        {
            BaseAPIStore.Logon(User.Login, Password);
            bool a = true;
            //var mainViewModel = ObserveWrapperHelper.GetInstance().Resolve<MainViewModel>();
            //mainViewModel.OpenSession();
            //Close();
            //Dispose();
        }

        

        private ICommand _loadedCommand;
        public ICommand LoadedCommand
        {
            get
            {
                return _loadedCommand ?? (_loadedCommand = new RelayCommand(param => OnLoadedCommand(), null));
            }
        }

        private void OnLoadedCommand()
        {
            var posInfo = POSInfoStore.Load();
            var ucs = POSInfoStore.Load().AvailableUsers;
            Users = new ObservableCollection<UserCS>(ucs);
        }

        private ICommand _logonOffCommand;
        public ICommand LogonOffCommand
        {
            get
            {
                return _logonOffCommand ?? (_logonOffCommand = new RelayCommand(param => OnLogonOffCommand(), null));
            }
        }

        private void OnLogonOffCommand()
        {
            IsAuthority = false;
            User = null;
            Password = null;
        }

        
    }
}
