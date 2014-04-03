using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UseAbilities.MVVM.Base;

namespace ASofCP.Cashier.ViewModels.Base
{
    public class ApplicationViewModel : ViewModelBase
    {
        public String Title
        {
            get { return "ASofCP модуль «Кассир» 1.0"; }
        }

    }
}
