using System;
using System.Collections.Generic;
using System.Security;
using System.Windows.Input;
using ASofCP.Cashier.ViewModels.Base;
using UseAbilities.MVVM.Base;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels
{
    public class LoginViewModel : ApplicationViewModel
    {
        public LoginViewModel()
        {
            Users = new List<String>
                {
                    "Константин Константинович Константинопольский", 
                    "Анна Юрьевна Агейман"
                };
        }
        

        public virtual IList<String> Users
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
    }
}
