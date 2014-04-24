﻿using System;
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
        // ReSharper disable DoNotCallOverridableMethodsInConstructor
        public RollInfoViewModel()
        {
            TicketColorIndex = -1;
            IsShowAll = true;

            #if DEBUG
            FirstTicketSeries = "RC";
            FirstTicketNumber = 16;
            TicketColorIndex = 0;
            #endif
        }
        // ReSharper restore DoNotCallOverridableMethodsInConstructor

        public virtual ObservableCollection<RollColor> Colors { get; set; }
        public virtual string MainTitle { get; set; }
        public virtual string TicketTitle { get; set; }
        public virtual string MainButtonTitle { get; set; }
        public virtual bool IsColorNeed { get; set; }
        public virtual bool IsCanCanceld { get; set; }
        public virtual bool IsShowAll { get; set; }

        //public string CurrentTicketSeries { get; set; }
        //public long CurrentTicketNumber { get; set; }
        //public RollColor CurrentTicketColor { get; set; }
        public RollInfo CurrentRollInfo { get; set; }

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
                    if (!DeactivateRoll()) return;
                    if (BaseAPI.isShiftOpen()) BaseAPI.closeShift(BaseAPI.getCurrentShift());
                    _rollInfo = null;
                    _shift = null;
                    break;
                case ChildWindowMode.NeedNewRoll:
                    if (!CloseRoll()) return;
                    _rollInfo = BaseAPI.activateTicketRoll(FirstTicketSeries, FirstTicketNumber, TicketColor);
                    _shift = BaseAPI.getCurrentShift();
                    break;
                //case ChildWindowMode.ChangeRollDeactivate:
                //    isDeactivateSucces = BaseAPI.deactivateTicketRoll(FirstTicketSeries, FirstTicketNumber, TicketColor);
                //    _rollInfo = null;
                //    _shift = BaseAPI.getCurrentShift();
                //    break;
                case ChildWindowMode.ChangeRoll:
                    if (!DeactivateRoll()) return;
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

        private bool DeactivateRoll()
        {
            IsShowErrorMessage = false;
            if (!CurrentRollInfo.IsActiveOnStation || BaseAPI.deactivateTicketRoll(CurrentRollInfo.Series, CurrentRollInfo.NextTicket, CurrentRollInfo.Color))
            {
                IsShowAll = true;
                return true;
            }

            IsShowAll = false;
            ErrorMessage = String.Format("Деактивировать ленту билетов {0} {1} {2} не получилось!", CurrentRollInfo.Series, CurrentRollInfo.NextTicket, CurrentRollInfo.Color.Color);
            IsShowErrorMessage = true;
            return false;
        }

        private bool CloseRoll()
        {
            IsShowErrorMessage = false;
            if (BaseAPI.closeTicketRoll(CurrentRollInfo))
            {
                IsShowAll = true;
                return true;
            }

            IsShowAll = false;
            ErrorMessage = String.Format("Закрыть ленту билетов {0} {1} {2} не получилось!", CurrentRollInfo.Series, CurrentRollInfo.NextTicket, CurrentRollInfo.Color.Color);
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
            if (Mode == ChildWindowMode.NeedNewRoll) CloseRoll();
            else DeactivateRoll();
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
            _rollInfo = CurrentRollInfo;
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
