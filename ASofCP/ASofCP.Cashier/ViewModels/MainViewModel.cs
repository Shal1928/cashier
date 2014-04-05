using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.ViewModels.Base;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;
using UseAbilities.MVVM.Base;

namespace ASofCP.Cashier.ViewModels
{
    public class MainViewModel : ApplicationViewModel
    {
        public MainViewModel()
        {
            var attractionsForOldMan = new GroupContentList
                {
                    new ParkService("Кресло качалка"), new ParkService("Скамейка сплетен") , new ParkService("Домино")
                };
            var attractionsForNewMan = new GroupContentList
                {
                   new ParkService("Хали-гали"), new ParkService("Паратрупер") , new ParkService("Супер 8") 
                };
            var gameMachines = new GroupContentList
                {
                    new ParkService("Городки"), new ParkService("Морской бой") , new ParkService("Авторалли-М"), new ParkService("Зимняя охота"), new ParkService("Репка")
                };

            CollectionServices = new GroupContentList
                {

                    new ParkService("Аттракционы для стариков", attractionsForOldMan), 
                    new ParkService("Аттракционы для молодых", attractionsForNewMan), 
                    new ParkService("Игровые автоматы", gameMachines)
                };

            CollectionServices.IsTop = true;
        }

        public virtual GroupContentList CollectionServices
        {
            get; 
            set;
        }



    }
}
