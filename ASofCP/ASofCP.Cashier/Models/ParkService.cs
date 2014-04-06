using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models.Base;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;

namespace ASofCP.Cashier.Models
{
    public class ParkService : IGroupContentItem, IPrice
    {
        public ParkService()
        {
            //
        }


        public ParkService(string title, double price)
        {
            Title = title;
            Price = price;
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


        #region Implementation of IPrice

        public double Price
        {
            get; 
            set;
        }

        #endregion
    }


//    public class ParkService : ParkService<IGroupContentItem>
//    {
//        public ParkService()
//        {
//            //
//        }

//        public ParkService(string title, double price) : base(title, price)
//        {
//            //
//        }

//        public ParkService(string title, GroupContentList<IGroupContentItem> subItems)
//            : base(title, subItems)
//        {
//            //
//        }
//    }
}
