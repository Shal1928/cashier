using System;
using System.IO;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.ViewModels.Base;
using UseAbilities.IoC.Attributes;
using UseAbilities.IoC.Stores;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels.ChildViewModels
{
    public class SettingsViewModel : ChildViewModelBase
    {
        private bool _isLoadComplete;

        [InjectedProperty]
        public IStore<ModuleSettings> SettingsStore { get; set; }

        public ModuleSettings Settings { get; set; }

        public virtual String IP
        {
            get { return Settings.NotNull() ? Settings.IP : String.Empty; } 
            set { if (Settings.NotNull()) Settings.IP = value; }
        }

        public virtual String Port
        {
            get { return Settings.NotNull() ? Settings.Port : String.Empty; }
            set { if (Settings.NotNull()) Settings.Port = value; }
        }

        public virtual String Id
        {
            get { return Settings.NotNull() ? Settings.Id : String.Empty; }
            set { if (Settings.NotNull()) Settings.Id = value; }
        }

        public virtual String PrinterName
        {
            get { return Settings.NotNull() ? Settings.PrinterName : String.Empty; }
            set { if (Settings.NotNull()) Settings.PrinterName = value; }
        }

        public virtual String PathToTemplate
        {
            get { return Settings.NotNull() ? Settings.PathToTemplate : String.Empty; }
            set { if (Settings.NotNull()) Settings.PathToTemplate = value; }
        }

        public virtual String PathToZpl
        {
            get { return Settings.NotNull() ? Settings.PathToZpl : String.Empty; }
            set { if (Settings.NotNull()) Settings.PathToZpl = value; }
        }

        protected override void OnLoadedCommand()
        {
            Settings = SettingsStore.Load();
            OnPropertyChanged(() => IP);
            OnPropertyChanged(() => Port);
            OnPropertyChanged(() => Id);
            OnPropertyChanged(() => PrinterName);
            OnPropertyChanged(() => PathToTemplate);
            OnPropertyChanged(() => PathToZpl);
            _isLoadComplete = true;
        }


        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(param => OnSaveCommand(), can => ValidateSaveCommand()));
            }
        }

        private void OnSaveCommand()
        {
            if (SettingsStore.IsNull()) return;
            SettingsStore.Save(Settings);
            Close();
            Dispose();
        }

        private bool ValidateSaveCommand()
        {
            if (SettingsStore.IsNull()) return false;
            var originalSettings = SettingsStore.Load();
            return !Equals(Settings, originalSettings);
        }


        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(param => OnCancelCommand(), null));
            }
        }

        private void OnCancelCommand()
        {
            Close();
            Dispose();
        }


        #region SendZPLRollCommand
        private ICommand _sendZPLRollCommand;
        public ICommand SendZPLRollCommand
        {
            get
            {
                return _sendZPLRollCommand ?? (_sendZPLRollCommand = new RelayCommand(param => OnSendZPLRollCommand(), can => ValidateSendZPLCommand()));
            }
        }

        private void OnSendZPLRollCommand()
        {
            RawPrinterHelper.SendFileToPrinter(PrinterName, PathToZpl);
        }

        private bool ValidateSendZPLCommand()
        {
            IsShowErrorMessage = false; 
            if (!_isLoadComplete) return true;
            if (File.Exists(PathToZpl)) return true;

            ErrorMessage = String.Format("Файл шаблона zpl \"{0}\" не найден!", PathToZpl);
            IsShowErrorMessage = true;
            return false;
        }
        #endregion

        //#region ValidatePrintTemplatePathCommand
        //private ICommand _validatePrintTemplatePathCommand;
        //public ICommand ValidatePrintTemplatePathCommand
        //{
        //    get
        //    {
        //        return _validatePrintTemplatePathCommand ?? (_validatePrintTemplatePathCommand = new RelayCommand(param => OnValidatePrintTemplatePathCommand(), can => ValidatePathToTemplate()));
        //    }
        //}

        //private void OnValidatePrintTemplatePathCommand()
        //{
        //    //
        //}

        //private bool ValidatePathToTemplate()
        //{
        //    IsShowErrorMessage = false; 
        //    if (!_isLoadComplete) return true;
        //    if (File.Exists(PathToTemplate)) return true;

        //    ErrorMessage = String.Format("Файл шаблона zpl \"{0}\" не найден!", PathToTemplate);
        //    IsShowErrorMessage = true;
        //    return false;
        //}
        //#endregion
    }
}
