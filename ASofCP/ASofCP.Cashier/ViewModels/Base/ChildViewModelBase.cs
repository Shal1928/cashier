namespace ASofCP.Cashier.ViewModels.Base
{
    public class ChildViewModelBase : ApplicationViewModel
    {
        public virtual string ErrorMessage { get; set; }
        public virtual bool IsShowErrorMessage { get; set; }
    }
}
