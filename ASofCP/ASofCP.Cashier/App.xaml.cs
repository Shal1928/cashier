using System;
using System.Collections.Generic;
using System.Windows;
using ASofCP.Cashier.ViewModels;
using ASofCP.Cashier.Views;
using UseAbilities.IoC.Core;
using UseAbilities.IoC.Helpers;
using UseAbilities.MVVM.Base;
using UseAbilities.MVVM.Managers;

namespace ASofCP.Cashier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            //Loader(StaticHelper.IoCcontainer);
            //var startupWindowSeed = (LoginViewModel)StaticHelper.IoCcontainer.Resolve(ObserveWrapper.Wrap(typeof(LoginViewModel)));
            var startupWindowSeed = (MainViewModel)StaticHelper.IoCcontainer.Resolve(ObserveWrapper.Wrap(typeof(MainViewModel)));

            var relationsViewToViewModel = new Dictionary<Type, Type>
                                         {
                                            {typeof(LoginViewModel), typeof (LoginView)},
                                            {startupWindowSeed.GetType(), typeof (MainView)}
                                         };

            ViewManager.RegisterViewViewModelRelations(relationsViewToViewModel);
            ViewModelManager.ActiveViewModels.CollectionChanged += ViewManager.OnViewModelsCoolectionChanged;

            startupWindowSeed.Show();
        }

        //private static void Loader(IoC ioc)
        //{
        //    ioc.RegisterSingleton<IXmlStore<FactimeSettings>, FactimeSettingsStore>();
        //    ioc.RegisterSingleton<IFileStore<List<CalendarDay>>, CalendarDayStore>();
        //}
    }
}
