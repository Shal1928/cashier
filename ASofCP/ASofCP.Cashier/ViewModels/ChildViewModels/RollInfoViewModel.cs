using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.ViewModels.Base;
using it.q02.asocp.api.data;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels.ChildViewModels
{
    public class RollInfoViewModel : ChildViewModelBase
    {
        public RollInfoViewModel()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            TicketColorIndex = -1;
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        public virtual ObservableCollection<RollColor> Colors { get; set; }
        public virtual string MainTitle { get; set; }
        public virtual string TicketTitle { get; set; }
        public virtual string MainButtonTitle { get; set; }
        public virtual bool IsColorNeed { get; set; }

        private ChildWindowMode _mode;
        public ChildWindowMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                switch (_mode)
                {
                    case ChildWindowMode.OpenShift:
                        Prepare("Укажите информацию о вашей смене", "Первый билет", "Открыть смену", true);
                        break;
                    case ChildWindowMode.CloseShift:
                        Prepare("Укажите информацию о вашей смене", "Последний напечатанный билет", "Закрыть смену", true);
                        break;
                    case ChildWindowMode.ChangeRoll:
                        Prepare("Укажите информацию о новом рулоне билетов", "Первый билет", "Сменить рулон", true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public string FirstTicketSeries{get; set;}

        public long FirstTicketNumber{get; set;}
        

        public virtual int TicketColorIndex{get; set;}

        public RollColor TicketColor
        {
            get
            {
                return Colors.IsNullOrEmpty() ? null : Colors.ElementAtOrDefault(TicketColorIndex);
            }
        }


        public void Prepare(string mainTitle, string ticketTitle, string mainButtonTitle, bool isColorNeed)
        {
            MainTitle = mainTitle;
            TicketTitle = ticketTitle;
            MainButtonTitle = mainButtonTitle;
            IsColorNeed = isColorNeed;
        }

        #region MainCommand
        private ICommand _mainCommand;
        public ICommand MainCommand
        {
            get
            {
                return _mainCommand ?? (_mainCommand = new RelayCommand(param => OnMainCommand(), can => ValidateMainCommand()));
            }
        }

        private void OnMainCommand()
        {
            switch (Mode)
            {
                case ChildWindowMode.OpenShift:
                    _rollInfo = BaseAPI.activateTicketRoll(FirstTicketSeries, FirstTicketNumber, TicketColor);
                    _shift = BaseAPI.isShiftOpen() ? BaseAPI.getCurrentShift() : BaseAPI.openShift();
                    break;
                case ChildWindowMode.CloseShift:
                    _rollInfo = null;
                    _shift = null;
                    //TODO: Выбрасывать что нибудь, если не получилось
                    BaseAPI.deactivateTicketRoll(FirstTicketSeries, FirstTicketNumber, TicketColor);
                    if (BaseAPI.isShiftOpen()) BaseAPI.closeShift(BaseAPI.getCurrentShift());
                    break;
                case ChildWindowMode.ChangeRoll:
                    _rollInfo = BaseAPI.activateTicketRoll(FirstTicketSeries, FirstTicketNumber, TicketColor);
                    _shift = BaseAPI.getCurrentShift();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Close();
            Dispose();
        }

        private bool ValidateMainCommand()
        {
            return FirstTicketNumber > 0
                && !FirstTicketSeries.IsNullOrEmptyOrSpaces()
                && (!TicketColor.IsNull() || !IsColorNeed);
        }
        #endregion

        protected override void OnLoadedCommand()
        {
            var colors = BaseAPI.getColors();
            Colors = new ObservableCollection<RollColor>(colors);
        }

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
            _rollInfo = null;
            _shift = BaseAPI.getCurrentShift();
            Close();
            Dispose();
        }
        #endregion


        public event CloseEventHandler Closed;
        protected virtual void OnClose(RollInfoEventArgs e)
        {
            if (Closed != null) Closed(this, e);
        }

        private RollInfo _rollInfo;
        private Shift _shift;

        public override void Close()
        {
            OnClose(new RollInfoEventArgs(_rollInfo, _shift));
            base.Close();
        }
    }

    public delegate void CloseEventHandler(Object sender, RollInfoEventArgs e);

    public class RollInfoEventArgs : EventArgs
    {
        public RollInfoEventArgs(RollInfo rollInfo, Shift shift)
        {
            RollInfo = rollInfo;
            Shift = shift;
        }

        public RollInfo RollInfo { get; set; }
        public Shift Shift { get; set; }
    }
}
