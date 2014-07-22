using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier.Models
{
    public class CategoryService : IGroupContentItem
    {
        public CategoryService(AttractionGroupInfo group, IEnumerable<AttractionInfo> attractions)
        {
            if(group.Type == 0) return;
            Info = group;

            SubItemsCollection = new GroupContentList();
            SubItemsCollection.AddRange(attractions.OrderBy(i => i.Number).Select(attraction => new ParkService(attraction)));
        }

        public AttractionGroupInfo Info { get; set; }

        #region Implementation of IGroupContentItem

        public string Title { get { return Info.Title; } }
        public string Code { get { return Convert.ToString(Info.Number); } }
        public GroupContentList SubItemsCollection { get; set; }
        public bool IsFinal { get { return SubItemsCollection.IsNullOrEmpty(); } }
        public long Number { get { return Info.Number; } }
        public String Background { get { return Info.BackgroundColor; } }
        public String Foreground { get { return Info.ForegroundColor; } }


        #endregion
    }
}
