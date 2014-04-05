using System.Collections.Generic;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts;

namespace ASofCP.Cashier.Models
{
    public class ParkService : IGroupContentItem
    {
        public ParkService()
        {
            //
        }

        public ParkService(string title)
        {
            Title = title;
            SubItemsCollection = null;
        }

        public ParkService(string title, IList<IGroupContentItem> subItems)
        {
            Title = title;
            SubItemsCollection = subItems;
        }


        #region Implementation of IGroupContentItem

        public string Title
        {
            get; 
            set;
        }

        public IList<IGroupContentItem> SubItemsCollection
        {
            get; 
            set;
        }

        #endregion
    }
}
