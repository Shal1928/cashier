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

            //FirstTicketSeries = "КС";
            //FirstTicketNumber = 303854;
            //TicketColorIndex = 0;
            IsShowAll = true;
        }

        public virtual ObservableCollection<RollColor> Colors { get; set; }
        public virtual string MainTitle { get; set; }
        public virtual string TicketTitle { get; set; }
        public virtual string MainButtonTitle { get; set; }
        public virtual bool IsColorNeed { get; set; }
        public virtual bool IsCanCanceld { get; set; }
        public virtual bool IsShowAll { get; set; }

        public string CurrentTicketSeries { get; set; }
        public long CurrentTicketNumber { get; set; }
        public RollColor CurrentTicketColor { get; set; }

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
                        IsCanCanceld = true;
                        break;
                    case ChildWindowMode.CloseShift:
                        Prepare("Укажите информацию о вашей смене", "Последний напечатанный билет", "Закрыть смену", true);
                        IsCanCanceld = true;
                        break;
                    case ChildWindowMode.NeedNewRoll:
                        Prepare("Билеты в рулоне закончились, укажите информацию о новом рулоне билетов", "Первый билет", "Активировать ленту", true);
                        IsCanCanceld = false;
                        break;
                    //case ChildWindowMode.ChangeRollDeactivate:
                    //    Prepare("Укажите информацию о текущем рулоне билетов", "Последний напечатанный билет", "Деактивировать ленту", true);
                    //    break;
                    case ChildWindowMode.ChangeRoll:
                        Prepare("Укажите информацию о новом рулоне билетов", "Первый билет", "Активировать ленту", true);
                        IsCanCanceld = true;
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
            IsShowErrorMessage = false;
            switch (Mode)
            {
                case ChildWindowMode.OpenShift:
                    _rollInfo = BaseAPI.activateTicketRoll(FirstTicketSeries, FirstTicketNumber, TicketColor);
                    _shift = BaseAPI.isShiftOpen() ? BaseAPI.getCurrentShift() : BaseAPI.openShift();
                    break;
                case ChildWindowMode.CloseShift:
                    _rollInfo = null;
                    _shift = null;
                    if (!DeactivateRoll(FirstTicketSeries, FirstTicketNumber, TicketColor)) return;
                    if (BaseAPI.isShiftOpen()) BaseAPI.closeShift(BaseAPI.getCurrentShift());
                    break;
                case ChildWindowMode.NeedNewRoll:
                    if (!DeactivateRoll(CurrentTicketSeries, CurrentTicketNumber, CurrentTicketColor)) return;
                    _rollInfo = BaseAPI.activateTicketRoll(FirstTicketSeries, FirstTicketNumber, TicketColor);
                    _shift = BaseAPI.getCurrentShift();
                    break;
                //case ChildWindowMode.ChangeRollDeactivate:
                //    isDeactivateSucces = BaseAPI.deactivateTicketRoll(FirstTicketSeries, FirstTicketNumber, TicketColor);
                //    _rollInfo = null;
                //    _shift = BaseAPI.getCurrentShift();
                //    break;
                case ChildWindowMode.ChangeRoll:
                    if (!DeactivateRoll(CurrentTicketSeries, CurrentTicketNumber, CurrentTicketColor)) return;
                    _rollInfo = BaseAPI.activateTicketRoll(FirstTicketSeries, FirstTicketNumber, TicketColor);
                    _shift = BaseAPI.getCurrentShift();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_rollInfo.IsNull() && Mode != ChildWindowMode.CloseShift /*&& Mode != ChildWindowMode.ChangeRollDeactivate*/)
            {
                ErrorMessage = String.Format("Бабина с параметрами {0} {1} {2} не существует!", FirstTicketSeries, FirstTicketNumber, TicketColor.Color);
                IsShowErrorMessage = true;
                return;
            }

            if (_shift.IsNull() && Mode != ChildWindowMode.CloseShift)
            {
                ErrorMessage = String.Format("Смена не определена!");
                IsShowErrorMessage = true;
                return;
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

        private bool DeactivateRoll(String series, long number, RollColor color)
        {
            CurrentTicketSeries = series;
            CurrentTicketNumber = number;
            CurrentTicketColor = color;

            IsShowErrorMessage = false;
            if (BaseAPI.deactivateTicketRoll(series, number, color))
            {
                IsShowAll = true;
                return true;
            }

            IsShowAll = false;
            ErrorMessage = String.Format("Деактивировать ленту билетов {0} {1} {2} не получилось!", series, number, color.Color);
            IsShowErrorMessage = true;
            return false;
        }

        #region DeactivateCommand
        private ICommand _deactivateCommand;
        public ICommand DeactivateCommand
        {
            get
            {
                return _deactivateCommand ?? (_deactivateCommand = new RelayCommand(param => OnDeactivateCommand(), null));
            }
        }

        private void OnDeactivateCommand()
        {
            DeactivateRoll(CurrentTicketSeries, CurrentTicketNumber, CurrentTicketColor);
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
                return _cancelCommand ?? (_cancelCommand = new RelayCommand(param => OnCancelCommand(), can => IsCanCanceld));
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
