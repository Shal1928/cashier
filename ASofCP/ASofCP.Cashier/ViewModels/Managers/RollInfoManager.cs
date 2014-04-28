using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.ViewModels.ChildViewModels;
using it.q02.asocp.api.data;
using log4net;

namespace ASofCP.Cashier.ViewModels.Managers
{
    public static class RollInfoManager
    {
        //private static readonly ILog Log = LogManager.GetLogger(typeof(RollInfoManager));
        //private static RollInfoViewModelMode _mode;

        //public static RollInfo RollInfo { get; set; }

        //public static void Resolve(RollInfoViewModelMode mode, RollInfo rollInfo)
        //{
        //    _mode = mode;
        //    RollInfo = rollInfo;

        //    var viewModel = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
        //    viewModel.Mode = mode;
        //    viewModel.CurrentRollInfo = rollInfo;
        //}

        //private void OnChangeRollCommand()
        //{
        //    IsEnabled = false;
        //    IsShowErrorMessage = false;
        //    var rollInfoViewModelA = ObserveWrapperHelper.GetInstance().Resolve<RollInfoViewModel>();
        //    rollInfoViewModelA.Mode = RollInfoViewModelMode.ChangeRoll;
        //    rollInfoViewModelA.CurrentRollInfo = CurrentRollInfo;
        //    rollInfoViewModelA.Show();
        //    rollInfoViewModelA.Closed += delegate(object senderA, RollInfoEventArgs argsA)
        //    {
        //        IsEnabled = true;
        //        if (argsA == null) throw new NullReferenceException("Информация о смене и бабине не определена!");
        //        var isChange = !Equals(CurrentRollInfo, argsA.RollInfo);
        //        CurrentRollInfo = argsA.RollInfo ?? CurrentRollInfo;
        //        ChangeRollInfoToLog(CurrentRollInfo, isChange);
        //        OnPropertyChanged(() => CurrentTicketNumber);
        //        OnPropertyChanged(() => CurrentTicketSeries);
        //        OnPropertyChanged(() => TicketsLeft);
        //    };
        //}
    }
}
