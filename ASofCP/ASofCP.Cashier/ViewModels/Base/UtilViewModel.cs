using System.Windows.Input;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels.Base
{
    public abstract class UtilViewModel : ApplicationViewModel
    {
        private ICommand _settingsCommand;
        public ICommand SettingsCommand
        {
            get
            {
                return _settingsCommand ?? (_settingsCommand = new RelayCommand(param => OnSettingsCommand(), null));
            }
        }

        protected abstract void OnSettingsCommand();
    }
}
