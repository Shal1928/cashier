namespace ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models
{
    public class GroupSearchResult

{
    public GroupSearchResult(IGroupContentItem result, GroupContentList group)
    {
        Result = result;
        Group = group;
    }

    public IGroupContentItem Result { get; private set; }

    public GroupContentList Group { get; private set; }
}
}
