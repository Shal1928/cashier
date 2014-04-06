using System;

namespace ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models
{
    public interface IGroupContentItem
    {
        String Title
        {
            get; 
            set;
        }

        GroupContentList SubItemsCollection
        {
            get; 
            set;
        }

        bool IsFinal
        {
            get; 
        }
    }

    //public interface IGroupContentItem : IGroupContentItem<IGroupContentItem>
    //{
    //    //
    //}
}
