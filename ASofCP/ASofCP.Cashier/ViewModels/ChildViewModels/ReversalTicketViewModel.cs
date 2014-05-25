using System;
using System.ComponentModel;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.ViewModels.Base;
using it.q02.asocp.api.data;
using log4net;
using UseAbilities.MVVM.Command;

namespace ASofCP.Cashier.ViewModels.ChildViewModels
{
    public class ReversalTicketViewModel : ChildViewModelBase
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ReversalTicketViewModel));

        public virtual string TicketBarcode { get; set; }
        public virtual string ReversalReason { get; set; }
        public virtual TicketInfo TicketInfo { get; set; }
        public virtual bool IsDisplayTicketInfo {get; set; }

        public string Name { get { return TicketInfo.IsNull() ? "Неизвестно" : TicketInfo.Name; } }
        public double Price { get { return TicketInfo.IsNull() ? 0 : TicketInfo.Price/100; } }
        public string PayType {
            get
            {
                if (TicketInfo.IsNull()) return "Неизвестно";

                switch (TicketInfo.PayType)
                {
                    case 0: return "наличные";
                    case 1: return "безнал";
                    case 2: return "сертификат";
                    case 66: return "списание";
                    default: throw new InvalidEnumArgumentException("Неизвестный тип платежа {0}!".F(TicketInfo.PayType));
                }
            } }
        public string CloseDate { get { return TicketInfo.IsNull() ? "Неизвестно" : TicketInfo.CloseDate.ToString("dd.MM.yy HH:mm:ss"); } }
        public string ReservalStatus {get { return TicketInfo.IsNull() ? "Неизвестно" : TicketInfo.IsReversed ? "Сторнирован" : string.Empty;}}
        public string ReservalCashier { get { return TicketInfo.IsNull() ? "(Неизвестно)" : TicketInfo.ReversedCashier.IsNullOrEmpty() ? string.Empty : " ({0})".F(TicketInfo.ReversedCashier); } }
        public string POSName { get { return TicketInfo.IsNull() ? "Неизвестно" : TicketInfo.PosName; } }
        public string Cashier { get { return TicketInfo.IsNull() ? "Неизвестно" : TicketInfo.Cashier; } }

        private void UpdateTicketInfoView()
        {
            OnPropertyChanged(() => Name);
            OnPropertyChanged(() => Price);
            OnPropertyChanged(() => PayType);
            OnPropertyChanged(() => CloseDate);
            OnPropertyChanged(() => ReservalStatus);
            OnPropertyChanged(() => ReservalCashier);
            OnPropertyChanged(() => POSName);
            OnPropertyChanged(() => Cashier);
        }

        protected override void OnLoadedCommand()
        {
            //
        }

        #region ReversalCommand
        private ICommand _reversalCommand;
        public ICommand ReversalCommand
        {
            get
            {
                return _reversalCommand ?? (_reversalCommand = new RelayCommand(param => OnReversalCommand(), can => ValidateReversalCommand()));
            }
        }

        private void OnReversalCommand()
        {
            IsShowErrorMessage = false;
            try
            {
                TicketInfo = BaseAPI.reverseTicket(TicketInfo, ReversalReason.IsNull() ? string.Empty : ReversalReason);
                UpdateTicketInfoView();
                IsDisplayTicketInfo = true;
            }
            catch (Exception e)
            {
                Log.Fatal("В процессе сторнирования билета {0}, произошло исключение:\n{1}",TicketBarcode, e);

                ErrorMessage = "Произошло исключение: {0}".F(e.Message);
                IsShowErrorMessage = true;
                return;
            }

            if (TicketInfo.RequestError)
                Log.Error("В процессе сторнирования билета {0}, произошла ошибка: {1}", TicketBarcode,
                    TicketInfo.ErrorMessage);
            else
            {
                ErrorMessage = "Успешно!";
                IsShowErrorMessage = true;
                Log.Debug("Билет {0} успешно сторнирован!", TicketBarcode);
            }
        }

        private bool ValidateReversalCommand()
        {
            return !TicketBarcode.IsNullOrEmptyOrSpaces() && 
                   !TicketInfo.IsNull() && 
                   !TicketInfo.IsReversed &&
                   TicketInfo.CloseDate.Date == DateTime.Now.Date;
        }
        #endregion

        #region FindCommand
        private ICommand _findCommand;
        public ICommand FindCommand
        {
            get
            {
                return _findCommand ?? (_findCommand = new RelayCommand(param => OnFindCommand(), can => !TicketBarcode.IsNullOrEmptyOrSpaces()));
            }
        }

        private void OnFindCommand()
        {
            IsDisplayTicketInfo = false;
            IsShowErrorMessage = false;

            try
            {
                TicketInfo = BaseAPI.findTicketByBarcode(TicketBarcode);               
            }
            catch (Exception e)
            {
                IsDisplayTicketInfo = false;
                Log.Fatal("В процессе поиска билета по шк {0}, произошло исключение:\n{1}", TicketBarcode, e);

                ErrorMessage = "Произошло исключение: {0}".F(e.Message);
                IsShowErrorMessage = true;
                return;
            }

            if (TicketInfo != null && !TicketInfo.RequestError)
            {
                UpdateTicketInfoView();
                IsDisplayTicketInfo = true;
            }
            else
            {
                IsDisplayTicketInfo = false;
                ErrorMessage = "{0} {1}".F(TicketBarcode, TicketInfo.IsNull() ? "Информация о билете не определена!" : TicketInfo.ErrorMessage);
                IsShowErrorMessage = true;
            }
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
            Close();
            Dispose();
        }
        #endregion

        public event CloseEventHandler Closed;
        protected virtual void OnClose(RollInfoEventArgs e)
        {
            if (Closed != null) Closed(this, e);
        }

        public override void Close()
        {
            OnClose(new RollInfoEventArgs());
            base.Close();
        }
    }
}
