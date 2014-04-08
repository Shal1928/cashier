using System;
using System.Collections.Generic;
using System.Windows;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Stores;
using ASofCP.Cashier.ViewModels;
using ASofCP.Cashier.Views;
using UseAbilities.IoC.Core;
using UseAbilities.IoC.Helpers;
using UseAbilities.IoC.Stores;
using UseAbilities.MVVM.Base;
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
            var startupWindowSeed = (LoginViewModel)StaticHelper.IoCcontainer.Resolve(ObserveWrapper.Wrap(typeof(LoginViewModel)));
            //var startupWindowSeed = (MainViewModel)StaticHelper.IoCcontainer.Resolve(ObserveWrapper.Wrap(typeof(MainViewModel)));

            var relationsViewToViewModel = new Dictionary<Type, Type>
                                         {
                                            {startupWindowSeed.GetType(), typeof (LoginView)},
                                            {typeof(MainViewModel), typeof (MainView)}
                                            //{typeof(LoginViewModel), typeof (LoginView)},
                                            //{startupWindowSeed.GetType(), typeof (MainView)}
                                         };

            ViewManager.RegisterViewViewModelRelations(relationsViewToViewModel);
            ViewModelManager.ActiveViewModels.CollectionChanged += ViewManager.OnViewModelsCoolectionChanged;

            startupWindowSeed.Show();
        }

        private static void Loader(IoC ioc)
        {
            ioc.RegisterSingleton<IXmlStore<ModuleSettings>, SettingsStore>();
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
