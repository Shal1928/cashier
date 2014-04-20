using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Stores;
using ASofCP.Cashier.Stores.API;
using ASofCP.Cashier.Stores.Base;
using ASofCP.Cashier.ViewModels;
using ASofCP.Cashier.ViewModels.ChildViewModels;
using ASofCP.Cashier.Views;
using ASofCP.Cashier.Views.ChildViews;
using log4net;
using UseAbilities.IoC.Core;
using UseAbilities.IoC.Helpers;
using UseAbilities.IoC.Stores;
using UseAbilities.MVVM.Managers;
using it.q02.asocp.api.data;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace ASofCP.Cashier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private void OnStartup(object sender, StartupEventArgs e)
        {
            Log.Info("Касса запущена");
            Loader(StaticHelper.IoCcontainer);
            var o = ObserveWrapperHelper.GetInstance();
            var relationsViewToViewModel = new Dictionary<Type, Type>
                                         {
                                            {o.GetWrappedType<LoginViewModel>(true), typeof (LoginView)},
                                            {o.GetWrappedType<MainViewModel>(true), typeof (MainView)},
                                            {o.GetWrappedType<PaymentViewModel>(true), typeof(PaymentView)},
                                            {o.GetWrappedType<RollInfoViewModel>(true), typeof(RollInfoView)},
                                            {o.GetWrappedType<InformationViewModel>(true), typeof(InformationView)},
                                            {o.GetWrappedType<SettingsViewModel>(true), typeof(SettingsView)}
                                         };
            
            ViewManager.RegisterViewViewModelRelations(relationsViewToViewModel);
            ViewModelManager.ActiveViewModels.CollectionChanged += ViewManager.OnViewModelsCoolectionChanged;

            var debugValue = !true;
            DebugHelper.IsDebug = debugValue;
            DebugHelper.IsPrintEnabled = !debugValue;
            if (DebugHelper.IsDebug) Log.Info("Режим отладки активирован");
            if (!DebugHelper.IsPrintEnabled) Log.Info("Вывод на принтер отключен");

            if (DebugHelper.IsDebug)
            {
                var startupWindowSeed = o.Resolve<MainViewModel>();
                startupWindowSeed.OpenSession();
            }
            else
            {
                var startupWindowSeed = o.Resolve<LoginViewModel>();
                startupWindowSeed.Show();
            }
        }

        private static void Loader(IoC ioc)
        {
            ioc.RegisterSingleton<IStore<ModuleSettings>, SettingsStore>();
            ioc.RegisterSingleton<ISecureReadStore<BaseAPI>, BaseAPIStore>();
            ioc.RegisterSingleton<IReadStore<POSInfo>, POSInfoStore>();
        }
    }


    //1 Авторизация, 
    //Открыть смену - серия номер первого билета, цвет на сервер
    //Получаем инфу о бабине
    // 1 Услуга 1 билет
    //Знак сколько осталось
    //Оплата налом, без нал, сертификат - передаем инфу каким образом оплачено
    //Билеты печатаем по очереди
    //Если зажевало? Сообщаем, 
    //ШК билета: номер серия
    //Чек открывается: дата открытия/дата закрытия, форма оплаты, Коллекция билетов (серия номер, дата печати) 
    //Если кончилась бабина показываем открытие новой бабины
    //2 
    //Активация бабины
    //Закрыть смену
    //Сторнирование по билетам
}
