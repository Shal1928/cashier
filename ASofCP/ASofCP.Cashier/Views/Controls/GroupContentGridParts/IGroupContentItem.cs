using System;
using System.Collections.Generic;

namespace ASofCP.Cashier.Views.Controls.GroupContentGridParts
{
    public interface IGroupContentItem
    {
        String Title
        {
            get; 
            set;
        }

        IList<IGroupContentItem> SubItemsCollection
        {
            get; 
            set;
        }
    }
}
