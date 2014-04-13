using System;
using System.Windows.Input;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.ViewModels.Base;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels.ChildViewModels
{
    public class PaymentViewModel : ChildViewModelBase
    {
        public PaymentTypes? PaymentType { get; set; }
        public double Total { get; set; }
        public virtual double Change { get; set; }

        private double _cash;
        public virtual double Cash
        {
            get
            {
                return _cash;
            }
            set
            {
                _cash = value;
                Change = _cash > Total ? Total - _cash : 0;
            }
        }

        #region PayCommand
        private ICommand _payCommand;
        public ICommand PayCommand
        {
            get
            {
                return _payCommand ?? (_payCommand = new RelayCommand<PaymentTypes?>(OnPayCommand, null));
            }
        }

        private ICommand _cashPayCommand;

        public ICommand CashPayCommand
        {
            get
            {
                return _cashPayCommand ?? (_cashPayCommand = new RelayCommand<PaymentTypes?>(OnPayCommand, can => Cash >= Total));
            }
        }

        private void OnPayCommand(PaymentTypes? paymentType)
        {
            PaymentType = paymentType;
            Close();
        }
        #endregion

        #region ChangelessCommand
        private ICommand _changelessCommand;
        public ICommand ChangelessCommand
        {
            get
            {
                return _changelessCommand ?? (_changelessCommand = new RelayCommand(param => OnChangelessCommand(), null));
            }
        }

        private void OnChangelessCommand()
        {
            Cash = Total;
        }
        #endregion



        public event PaymentEventHandler PaymentReached;
        protected virtual void OnPaymentReached(PaymentEventArgs e)
        {
            if (PaymentReached != null) PaymentReached(this, e);
        }

        public override void Close()
        {
            OnPaymentReached(new PaymentEventArgs(PaymentType));
            base.Close();
        }
    }

    public delegate void PaymentEventHandler(Object sender, PaymentEventArgs e);

    public class PaymentEventArgs : EventArgs
    {
        public PaymentEventArgs(PaymentTypes? paymentType)
        {
            PaymentType = paymentType;
        }

        public PaymentTypes? PaymentType { get; set; }
    }
}
