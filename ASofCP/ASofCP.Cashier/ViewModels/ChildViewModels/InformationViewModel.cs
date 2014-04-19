using System;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.ViewModels.Base;
using it.q02.asocp.api.data;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels.ChildViewModels
{
    public class InformationViewModel : ChildViewModelBase
    {
        public virtual string Message
        {
            get
            {
                return String.Format("Для печати не хватит {0} билетов!", Count);
            }
        }

        public long Count { get; set; }
        public string CurrentTicketSeries { get; set; }
        public long CurrentTicketNumber { get; set; }
        public RollColor CurrentTicketColor { get; set; }

        #region ChangeRollCommand
        private ICommand _changeRollCommand;
        public ICommand ChangeRollCommand
        {
            get
            {
                return _changeRollCommand ?? (_changeRollCommand = new RelayCommand(param => OnChangeRollCommand(), null));
            }
        }

        private void OnChangeRollCommand()
        {
            var rollInfoViewModelA = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
            rollInfoViewModelA.Mode = ChildWindowMode.ChangeRoll;
            rollInfoViewModelA.CurrentTicketSeries = CurrentTicketSeries;
            rollInfoViewModelA.CurrentTicketNumber = CurrentTicketNumber;
            rollInfoViewModelA.CurrentTicketColor = CurrentTicketColor;
            rollInfoViewModelA.Show();
            rollInfoViewModelA.Closed += delegate(object senderA, RollInfoEventArgs argsA)
            {
                if (argsA == null) throw new NullReferenceException("Информация о смене и бабине не определена!");
                _rollInfoEventArgs = argsA;
                Close();
                Dispose();
            };
        }
        #endregion

        #region CancelCommand
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
            _rollInfoEventArgs = null;
            Close();
        }
        #endregion

        private RollInfoEventArgs _rollInfoEventArgs;

        public event CloseEventHandler Closed;
        protected virtual void OnClose(RollInfoEventArgs e)
        {
            if (Closed != null) Closed(this, e);
        }

        public override void Close()
        {
            OnClose(_rollInfoEventArgs);
            base.Close();
        }

        protected override void OnLoadedCommand()
        {
            //
        }
    }
}
