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
            //
        }

        [InjectedProperty]
        public IReadStore<POSInfo> POSInfoStore
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

        public virtual UserCS User
        {
            get; 
            set;
        }

        public virtual string PosTitle { get; set; }
        public virtual bool IsShowErrorMessage { get; set; }

        private ICommand _enterCommand;
        public ICommand EnterCommand
        {
            get
            {
                return _enterCommand ?? (_enterCommand = new RelayCommand(param => OnEnterCommand(), can => ValidateEnterCommand()));
            }
        }

        private void OnEnterCommand()
        {
            IsShowErrorMessage = false;
            BaseAPIStore.Logon(User.Login, Password);
            try
            {
                BaseAPI.isShiftOpen();
                var mainViewModel = ObserveWrapperHelper.GetInstance().Resolve<MainViewModel>();
                mainViewModel.PosTitle = PosTitle;
                mainViewModel.OpenSession();
                Close();
                Dispose();
            }
            catch (Exception)
            {
                IsShowErrorMessage = true;
            }
        }

        private bool ValidateEnterCommand()
        {
            return User.NotNull() && !Password.IsNullOrEmpty();
        }

        

        //private ICommand _loadedCommand;
        //public ICommand LoadedCommand
        //{
        //    get
        //    {
        //        return _loadedCommand ?? (_loadedCommand = new RelayCommand(param => OnLoadedCommand(), null));
        //    }
        //}

        protected override void OnLoadedCommand()
        {
            var posInfo = POSInfoStore.Load();
            PosTitle = posInfo.DisplayName;
            Users = new ObservableCollection<UserCS>(POSInfoStore.Load().AvailableUsers);
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
            User = null;
            Password = null;
        }

        
    }
}
