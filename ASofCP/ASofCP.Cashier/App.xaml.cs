﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Stores;
using ASofCP.Cashier.Stores.API;
using ASofCP.Cashier.Stores.Base;
using ASofCP.Cashier.ViewModels;
using ASofCP.Cashier.ViewModels.ChildViewModels;
using ASofCP.Cashier.ViewModels._00_MainViewModel;
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
    public partial class App
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));

        public static int Tier { get { return RenderCapability.Tier >> 16; } }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            Console.Out.WriteLine("Render Tier: {0}", Tier);

            SingleInstanceHelper.Make();

            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendFormat("Запущена касса версии {0}", Assembly.GetExecutingAssembly().GetName().Version);
            Log.Info(sb);

            Loader(StaticHelper.IoCcontainer);
            var o = ObserveWrapperHelper.GetInstance();
            var relationsViewToViewModel = new Dictionary<Type, Type>
                                         {
                                            {o.GetWrappedType<LoginViewModel>(true), typeof (LoginView)},
                                            {o.GetWrappedType<MainViewModel>(true), typeof (MainView)},
                                            {o.GetWrappedType<PaymentViewModel>(true), typeof(PaymentView)},
                                            {o.GetWrappedType<RollInfoViewModel>(true), typeof(RollInfoView)},
                                            {o.GetWrappedType<InformationViewModel>(true), typeof(InformationView)},
                                            {o.GetWrappedType<SettingsViewModel>(true), typeof(SettingsView)},
                                            {o.GetWrappedType<TicketWriteOffViewModel>(true), typeof(TicketWriteOffView)},
                                            {o.GetWrappedType<ReversalTicketViewModel>(true), typeof(ReversalTicketView)}
                                         };
            
            ViewManager.RegisterViewViewModelRelations(relationsViewToViewModel);
            ViewModelManager.ActiveViewModels.CollectionChanged += ViewManager.OnViewModelsCoolectionChanged;

            #if LOGIN_DEBUG && PRINT_DEBUG
            Log.Warn("Запущено в режиме полной отладки!");
            #elif LOGIN_DEBUG
            Log.Warn("Запущено в режиме отладки авторизации!");
            #elif PRINT_DEBUG
            Log.Warn("Запущено в режиме отладки печати!");
            #elif DEBUG
            Log.Warn("Запущено в режиме отладки!");
            #else
            Log.Info("Запущено в продуктивном режиме.");
            #endif

            #if DEBUG && !LOGIN_DEBUG
            var startupWindowSeed = o.Resolve<MainViewModel>();
            startupWindowSeed.OpenSession();
            #else
            var startupWindowSeed = o.Resolve<LoginViewModel>();
            startupWindowSeed.Show();
            #endif
        }

        private static void Loader(IoC ioc)
        {
            ioc.RegisterSingleton<IStore<ModuleSettings>, SettingsStore>();
            ioc.RegisterSingleton<ISecureReadStore<BaseAPI>, BaseAPIStore>();
            ioc.RegisterSingleton<IReadStore<POSInfo>, POSInfoStore>();
            ioc.RegisterSingleton<IQueueStore<ChequeQueue>, ChequeQueueStore>();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {   
            Log.Debug("Произошло необработанное исключение!");
            Log.Fatal(e.Exception);
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
