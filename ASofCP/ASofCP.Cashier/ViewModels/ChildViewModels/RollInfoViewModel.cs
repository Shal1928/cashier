using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models.Contracts;
using ASofCP.Cashier.ViewModels.Base;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels.ChildViewModels
{
    public class RollInfoViewModel : ChildViewModelBase
    {
        public RollInfoViewModel()
        {
            TicketColorIndex = -1;

            Prepare("Укажите информацию о вашей смене",
                    "Первый билет",
                    "Открыть смену",
                    true);

            Colors = new ObservableCollection<string>
                {
                    "Yellow",
                    "White",
                    "Orange",
                    "Pink"
                };
        }

        public virtual ObservableCollection<string> Colors { get; set; }
        public virtual string MainTitle { get; set; }
        public virtual string TicketTitle { get; set; }
        public virtual string MainButtonTitle { get; set; }
        public virtual bool IsColorNeed { get; set; }

        public string FirstTicketSeries{get; set;}

        public long FirstTicketNumber{get; set;}
        

        public virtual int TicketColorIndex{get; set;}

        public string TicketColor
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
            _rollInfo = new RollInfo()
            {
                Series = FirstTicketSeries,
                NextTicket = FirstTicketNumber,
                Color = new RollColor
                {
                    Color = TicketColor
                }
            };
            Close();
            Dispose();
        }

        private bool ValidateMainCommand()
        {
            return FirstTicketNumber > 0
                && !FirstTicketSeries.IsNullOrEmptyOrSpaces()
                && (!TicketColor.IsNullOrEmptyOrSpaces() || !IsColorNeed);
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
            _rollInfo = null;
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
        public override void Close()
        {
            OnClose(new RollInfoEventArgs(_rollInfo));
            base.Close();
        }
    }

    public delegate void CloseEventHandler(Object sender, RollInfoEventArgs e);

    public class RollInfoEventArgs : EventArgs
    {
        public RollInfoEventArgs(RollInfo rollInfo)
        {
            RollInfo = rollInfo;
        }

        public RollInfo RollInfo { get; set; }
    }
}
