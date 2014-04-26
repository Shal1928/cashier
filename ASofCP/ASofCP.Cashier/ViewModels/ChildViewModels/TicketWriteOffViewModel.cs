using System;
using System.Collections.Generic;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.ViewModels.Base;
using it.q02.asocp.api.data;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels.ChildViewModels
{
    public class TicketWriteOffViewModel : ChildViewModelBase
    {
        private List<ChequeRow> _chequeRows; 

        public RollInfo CurrentRollInfo { get; set; }
        public String RollColor { get { return CurrentRollInfo.NotNull() && CurrentRollInfo.Color.NotNull() ? CurrentRollInfo.Color.Color : null; } }
        public String RollSeries { get { return CurrentRollInfo.NotNull() && !CurrentRollInfo.Series.IsNullOrEmptyOrSpaces() ? CurrentRollInfo.Series : "неопределена"; } }

        public virtual long FirstTicketNumber { get; set; }
        public virtual long LastTicketNumber { get; set; }

        protected override void OnLoadedCommand()
        {
            //
        }

        public void Show(RollInfo currentRollInfo)
        {
            CurrentRollInfo = currentRollInfo;
            Show();
        }
        
         #region WriteOffTicketsCommand
        private ICommand _writeOffTicketsCommand;
        public ICommand WriteOffTicketsCommand
        {
            get
            {
                return _writeOffTicketsCommand ?? (_writeOffTicketsCommand = new RelayCommand(param => OnWriteOffTicketsCommand(), can => ValidateWriteOffTicketsCommand()));
            }
        }

        private void OnWriteOffTicketsCommand()
        {
            _chequeRows = new List<ChequeRow>();
            while (CurrentRollInfo.NextTicket <= LastTicketNumber)
            {
                _chequeRows.Add(new ChequeRow
                {
                    Attraction = new AttractionInfo { Id = 0 },
                    PrintDate = DateTime.Now,
                    Printed = false,
                    TicketNumber = CurrentRollInfo.NextTicket,
                    TicketRoll = CurrentRollInfo
                });
                CurrentRollInfo.NextTicket++;
            } 

            Close();
            Dispose();
        }

        private bool ValidateWriteOffTicketsCommand()
        {
            return CurrentRollInfo.NotNull() &&
                   FirstTicketNumber == CurrentRollInfo.NextTicket &&
                   LastTicketNumber >= FirstTicketNumber &&
                   CurrentRollInfo.TicketsLeft >= (LastTicketNumber - CurrentRollInfo.NextTicket);
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
            _chequeRows = null;
            Close();
            Dispose();
        }
        #endregion


        public event WriteOffEventHandler Closed;
        protected virtual void OnClose(WriteOffEventArgs e)
        {
            if (Closed != null) Closed(this, e);
        }

        

        public override void Close()
        {
            OnClose(new WriteOffEventArgs(_chequeRows));
            base.Close();
        }
    }

    public delegate void WriteOffEventHandler(Object sender, WriteOffEventArgs e);

    public class WriteOffEventArgs : EventArgs
    {
        public WriteOffEventArgs(List<ChequeRow> chequeRows)
        {
            ChequeRows = chequeRows;
        }

        public List<ChequeRow> ChequeRows { get; set; }
    }
}
