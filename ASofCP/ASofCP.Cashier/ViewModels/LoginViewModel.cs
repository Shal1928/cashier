using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.ViewModels.Base;
using ASofCP.Cashier.ViewModels.ChildViewModels;
using ASofCP.Cashier.ViewModels._00_MainViewModel;
using hessiancsharp.io;
using log4net;
using UseAbilities.IoC.Attributes;
using UseAbilities.IoC.Stores;
using UseAbilities.MVVM.Command;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier.ViewModels
{
    public class LoginViewModel : UtilViewModel
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoginViewModel));

        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        public LoginViewModel()
        {
            IsShowAll = true;
        }
        // ReSharper restore DoNotCallOverridableMethodsInConstructor

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
        public virtual string ErrorMessage { get; set; }
        public virtual bool IsShowAll { get; set; }

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
                mainViewModel.User = User.UserDisplayName;
                mainViewModel.OpenSession();
                Close();
                Dispose();
            }
            catch (Exception e)
            {
                Log.Fatal(e);
                ErrorMessage = "Не верный логин или пароль!";
                IsShowErrorMessage = true;
            }
        }

        private bool ValidateEnterCommand()
        {
            return User.NotNull() && !Password.IsNullOrEmpty();
        }


        protected override void OnLoadedCommand()
        {
            IsShowAll = true;
            IsShowErrorMessage = false;
            POSInfo posInfo;
            try
            {
                posInfo = POSInfoStore.Load();
            }
            catch (Exception e)
            {
                Log.Fatal(e);
                IsShowAll = false;
                ErrorMessage = e.Message;
                IsShowErrorMessage = true;
                //throw e;
                return;
            }
            
            PosTitle = posInfo.DisplayName;
            Users = new ObservableCollection<UserCS>(POSInfoStore.Load().AvailableUsers.OrderBy(u => u.DisplayName));
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

        protected override void OnSettingsCommand()
        {
            IsEnabled = false;
            var settingsVM = ObserveWrapperHelper.GetInstance().Resolve<SettingsViewModel>();
            settingsVM.Show();
            settingsVM.CloseEventHandler += delegate(object sender, ResultEventArgs args)
            {
                IsEnabled = true;
                if(args==null) return;

                if (args.Result == Result.Yes) OnLoadedCommand();
                
            };
        }
    }
}
