using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ASofCP.Cashier.Views.Controls.GroupContentGridParts
{
    /// <summary>
    /// Interaction logic for GroupContentGrid.xaml
    /// </summary>
    public partial class GroupContentGrid : UserControl
    {
        protected IList<IGroupContentItem> TopContentItems;

        protected bool? IsSub
        {
            get; 
            set;
        }

        public GroupContentGrid()
        {
            InitializeComponent();
        }

        #region ContentItems

        public static readonly DependencyProperty ContentItemsProperty =
            DependencyProperty.Register("ContentItems",
                                        typeof(IList<IGroupContentItem>),
                                        typeof(GroupContentGrid),
                                        new UIPropertyMetadata(null, OnContentItemsPropertyChanged)
                                        );

        public IList<IGroupContentItem> ContentItems
        {
            get
            {
                return (IList<IGroupContentItem>)GetValue(ContentItemsProperty);
            }
            set
            {
                SetValue(ContentItemsProperty, value);
            }
        }

        private static void OnContentItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var groupContentGrid = d as GroupContentGrid;
            if(groupContentGrid == null) return;

            var newContentItems = e.NewValue as IList<IGroupContentItem>;
            if(newContentItems == null) return;

            groupContentGrid    .RootWidget.Children.Clear();
            var colW = new GridLength(1, GridUnitType.Star);
            groupContentGrid.RootWidget.ColumnDefinitions.Add(new ColumnDefinition{Width = colW});
            groupContentGrid.RootWidget.ColumnDefinitions.Add(new ColumnDefinition { Width = colW });

            var curCol = 0;
            var curRow = 0;
            foreach (var contentItem in newContentItems)
            {
                if(curCol > 1)
                {
                    curCol = 0;
                    curRow++;
                }

                var rowDef = new RowDefinition
                    {
                        Height = new GridLength(0, GridUnitType.Auto)
                    };
                groupContentGrid.RootWidget.RowDefinitions.Add(rowDef);

                var button = new Button
                    {
                        Content = contentItem.Title,
                        Height = 42,
                        MinWidth = 170,
                        Margin = new Thickness(2),
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };

                var item = contentItem;
                button.Click += (sender, bE) =>
                    {
                        if(groupContentGrid.IsSub.HasValue && groupContentGrid.IsSub.Value) 
                            return;

                        groupContentGrid.TopContentItems = groupContentGrid.ContentItems;
                        groupContentGrid.ContentItems = item.SubItemsCollection;
                        groupContentGrid.IsSub = true;
                        groupContentGrid.BackButton.IsEnabled = true;
                    };

                Grid.SetColumn(button, curCol);
                Grid.SetRow(button, curRow);

                groupContentGrid.RootWidget.Children.Add(button);
                curCol++;
            }
        }

        #endregion 

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            IsSub = false;
            BackButton.IsEnabled = false;
            ContentItems = TopContentItems;
        }
    }
}
