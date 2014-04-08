using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.ViewModels.Base;
using UseAbilities.IoC.Attributes;
using UseAbilities.IoC.Stores;
using UseAbilities.MVVM.Command;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier.ViewModels
{
    public class LoginViewModel : ApplicationViewModel
    {
        public LoginViewModel()
        {
            //Users = new List<String>
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

        public virtual ObservableCollection<String> Users
        {
            get;
            set;
        }

        public virtual String Password
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
            var mainViewModel = new MainViewModel();
            mainViewModel.Show();
            Close();
            Dispose();
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
            var ucs = POSInfoStore.Load().AvailableUsers;
            Users = new ObservableCollection<string>();
            foreach (var u in ucs)
                Users.Add(u.UserDisplayName.ToUTF32());
        }
    }
}
