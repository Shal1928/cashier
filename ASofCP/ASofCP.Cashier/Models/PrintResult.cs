namespace ASofCP.Cashier.Models
{
    public class PrintResult
    {
        public static PrintResult Success = new PrintResult(false, false);
        public static PrintResult SuccessAndNeedNewTicketRoll = new PrintResult(false, true);
        public static PrintResult NeedTickets = new PrintResult(false, true);
        public static PrintResult Failure = new PrintResult(true, false);

        public PrintResult(bool hasError, bool isNeedNewTicketRoll)
        {
            HasError = hasError;
            IsNeedNewTicketRoll = isNeedNewTicketRoll;
        }

        public bool HasError { get; private set; }
        public bool IsNeedNewTicketRoll { get; private set; }
        public bool IsSuccess{ get { return !HasError; }}
    }
}
