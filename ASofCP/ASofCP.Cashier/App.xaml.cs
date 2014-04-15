using System;
using System.Collections.Generic;
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
using UseAbilities.IoC.Core;
using UseAbilities.IoC.Helpers;
using UseAbilities.IoC.Stores;
using UseAbilities.MVVM.Managers;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            Loader(StaticHelper.IoCcontainer);
            var o = ObserveWrapperHelper.GetInstance();
            var relationsViewToViewModel = new Dictionary<Type, Type>
                                         {
                                            {o.GetWrappedType<LoginViewModel>(true), typeof (LoginView)},
                                            {o.GetWrappedType<MainViewModel>(true), typeof (MainView)},
                                            {o.GetWrappedType<PaymentViewModel>(true), typeof(PaymentView)},
                                            {o.GetWrappedType<RollInfoViewModel>(true), typeof(RollInfoView)}
                                         };
            
            ViewManager.RegisterViewViewModelRelations(relationsViewToViewModel);
            ViewModelManager.ActiveViewModels.CollectionChanged += ViewManager.OnViewModelsCoolectionChanged;

            var startupWindowSeed = o.Resolve<LoginViewModel>();
            startupWindowSeed.Show();
            //var startupWindowSeed = o.Resolve<MainViewModel>();
            //startupWindowSeed.OpenSession();
        }

        private static void Loader(IoC ioc)
        {
            ioc.RegisterSingleton<IReadStore<ModuleSettings>, SettingsStore>();
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
