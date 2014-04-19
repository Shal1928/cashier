using System;

namespace ASofCP.Cashier.ViewModels.Base
{
    public abstract class ResultViewModel : ApplicationViewModel
    {
        public virtual Result Result { get; set; }

        public event CloseEventHandler CloseEventHandler;
        protected virtual void OnClose(ResultEventArgs e)
        {
            if (CloseEventHandler != null) CloseEventHandler(this, e);
        }

        public override void Close()
        {
            OnClose(new ResultEventArgs(Result));
            base.Close();
        }
    }



    public class ResultEventArgs : EventArgs
    {
        public ResultEventArgs(Result result)
        {
            Result = result;
        }

        public Result Result { get; set; }
    }

    public delegate void CloseEventHandler(Object sender, ResultEventArgs e);

    public enum Result
    {
        Cancel = 0,
        Yes = 1,
        No = 2
    }
}
