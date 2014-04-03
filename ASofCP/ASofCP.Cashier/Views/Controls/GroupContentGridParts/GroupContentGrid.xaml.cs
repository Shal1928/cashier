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

            //Если IsSub неопределено используем новые значения или true, если IsSub false используем старые значения
            var newContentItems = groupContentGrid.IsSub.HasValue ?
                groupContentGrid.IsSub.Value ? e.NewValue as IList<IGroupContentItem> : e.OldValue as IList<IGroupContentItem> 
                : e.NewValue as IList<IGroupContentItem>;
            
            if(newContentItems == null) return;

            groupContentGrid.RootWidget = new Grid();

            var colDef = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };

            groupContentGrid.RootWidget.ColumnDefinitions.Add(colDef);
            groupContentGrid.RootWidget.ColumnDefinitions.Add(colDef);

            var curCol = 0;
            var curRow = 0;
            foreach (var contentItem in newContentItems)
            {
                curCol = curCol > 1 ? 0 : curCol;
                var rowDef = new RowDefinition
                    {
                        Height = new GridLength(0, GridUnitType.Auto)
                    };
                groupContentGrid.RootWidget.RowDefinitions.Add(rowDef);

                var button = new Button
                    {
                        Content = contentItem.Title
                    };
                
                Grid.SetColumn(button, curCol);
                Grid.SetRow(button, curRow);

                curCol++;
                curRow++;
            }
        }

        #endregion 
    }
}
