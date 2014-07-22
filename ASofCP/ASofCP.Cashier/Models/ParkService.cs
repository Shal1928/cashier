using System;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Models.Base;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;
using it.q02.asocp.api.data;

namespace ASofCP.Cashier.Models
{
    public class ParkService : IGroupContentItem, IPrice
    {
        public static ParkService Empty = new ParkService(new AttractionInfo{Id = 0});

        public ParkService()
        {
            //
        }

        public ParkService(AttractionInfo attractionInfo)
        {
            AttractionInfo = attractionInfo;
            SubItemsCollection = null;
        }

        //public ParkService(string title, double price)
        //{
        //    Title = title;
        //    Price = price;
        //    SubItemsCollection = null;
        //}

        //public ParkService(string title, GroupContentList subItems)
        //{
        //    Title = title;
        //    SubItemsCollection = subItems;
        //}


        #region Implementation of IGroupContentItem

        public string Title
        {
            get { return AttractionInfo.DisplayName; }
        }

        public string PrintTitle
        {
            get { return AttractionInfo.PrintName; }
        }

        public string Code
        {
            get { return AttractionInfo.Code; }
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

        public long Number
        {
            get { return AttractionInfo.Number; }
        }

        public String Background {get { return AttractionInfo.BackgroundColor; }}
        public String Foreground { get { return AttractionInfo.ForegroundColor; } }

        #endregion


        public AttractionInfo AttractionInfo { get; set; }


        #region Implementation of IPrice

        public double Price
        {
            get { return AttractionInfo.Price/100; }
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
