using System.Collections.Generic;

namespace ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models
{
    public class GroupContentList : List<IGroupContentItem>
    {
        public bool IsTop { get; set; }
    }
}
