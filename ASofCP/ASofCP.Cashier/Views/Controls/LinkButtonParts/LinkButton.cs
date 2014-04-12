using System.Windows;
using System.Windows.Controls;

namespace ASofCP.Cashier.Views.Controls.LinkButtonParts
{
    public class LinkButton : Button
    {
        public LinkButton()
        {
            var linkButtonDic = new LinkButtonDic();
            Style = (Style)linkButtonDic["LinkButtonStyle"];
        }

        //#region ContentItems

        //public static readonly DependencyProperty ContentItemsProperty =
        //    DependencyProperty.Register("ContentItems",
        //                                typeof(GroupContentList),
        //                                typeof(GroupContentGrid),
        //                                new UIPropertyMetadata(null, OnContentItemsPropertyChanged)
        //                                );

        //public GroupContentList ContentItems
        //{
        //    get
        //    {
        //        return (GroupContentList)GetValue(ContentItemsProperty);
        //    }
        //    set
        //    {
        //        SetValue(ContentItemsProperty, value);
        //    }
        //}

        //private static void OnContentItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var entity = d as GroupContentGrid;
        //    if (entity == null) return;

        //    var newContentItems = e.NewValue as GroupContentList;
        //    if (newContentItems.IsNullOrEmpty()) return;

        //    // ReSharper disable PossibleNullReferenceException
        //    if (newContentItems.IsTop) entity.TopContentItems = newContentItems;
        //    // ReSharper restore PossibleNullReferenceException

        //    _helper.ClearItems();
        //    _helper.GenerateItems(newContentItems);
        //}

        //#endregion  
    }
}
