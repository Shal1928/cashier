using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;

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

        public ParkService(string title, GroupContentList subItems)
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

        public GroupContentList SubItemsCollection
        {
            get; 
            set;
        }

        public bool IsFinal
        {
            get { return SubItemsCollection.IsNullOrEmpty(); }
        }

        #endregion
    }
}
