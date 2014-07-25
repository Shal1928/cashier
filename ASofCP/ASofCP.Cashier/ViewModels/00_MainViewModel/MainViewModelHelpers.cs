using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier.ViewModels._00_MainViewModel
{
    public partial class MainViewModel
    {
        private void FillCollectionServices()
        {
            var collectionServices = new GroupContentList { IsTop = true };

            var categories = BaseAPI.getGroups();
            AttractionInfo[] attractions;
            if (categories.IsNullOrEmpty())
            {
                Log.Debug("Категории не найдены! Все аттракционы будут загружены в корневую группу.");
                attractions = BaseAPI.getAttractionsFromGroup(new AttractionGroupInfo());
                if (attractions.IsNullOrEmpty()) return;
                collectionServices.AddRange(attractions.OrderBy(i => i.Number).Select(attraction => new ParkService(attraction)));
            }
            else
            {
                var list = new List<IGroupContentItem>();
                var sb = new StringBuilder();
                foreach (var category in categories)
                {
                    attractions = BaseAPI.getAttractionsFromGroup(category);
                    if (category.Type == 0) list.AddRange(attractions.OrderBy(i => i.Number).Select(attraction => new ParkService(attraction)));
                    else list.Add(new CategoryService(category, attractions));

                    foreach (var a in attractions)
                        sb.AppendLine("Аттракцион {0} добавлен в группу {1}".F(a.DisplayName, category.Title));
                }
                Log.Debug(sb);

                collectionServices.AddRange(list.OrderBy(i => i.Number));
            }

            CollectionServices = collectionServices;
            OnPropertyChanged(() => CollectionServices);
        }
    }
}
