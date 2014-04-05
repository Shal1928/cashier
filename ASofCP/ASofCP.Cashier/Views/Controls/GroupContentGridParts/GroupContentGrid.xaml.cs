using System;
using System.Windows;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;

namespace ASofCP.Cashier.Views.Controls.GroupContentGridParts
{
    /// <summary>
    /// Interaction logic for GroupContentGrid.xaml
    /// </summary>
    public partial class GroupContentGrid
    {
        internal GroupContentList TopContentItems;
        internal GroupContentList PreviousContentItems;
        internal bool IsSub
        {
            get
            {
                return PreviousContentItems.NotEmpty() && (ContentItems.IsNullOrEmpty() || !ContentItems.IsTop);
            }
        }

        internal bool IsInternalChange;

        private static GroupContentGridHelper _helper;

        public GroupContentGrid()
        {
            InitializeComponent();
            _helper = new GroupContentGridHelper(this);
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            ContentItems = PreviousContentItems;
            SelectedItem = null;
        }



        #region ContentItems

        public static readonly DependencyProperty ContentItemsProperty =
            DependencyProperty.Register("ContentItems",
                                        typeof(GroupContentList),
                                        typeof(GroupContentGrid),
                                        new UIPropertyMetadata(null, OnContentItemsPropertyChanged)
                                        );

        public GroupContentList ContentItems
        {
            get
            {
                return (GroupContentList)GetValue(ContentItemsProperty);
            }
            set
            {
                SetValue(ContentItemsProperty, value);
            }
        }

        private static void OnContentItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var entity = d as GroupContentGrid;
            if(entity == null) return;

            var newContentItems = e.NewValue as GroupContentList;
            if(newContentItems.IsNullOrEmpty()) return;

            // ReSharper disable PossibleNullReferenceException
            if (newContentItems.IsTop) entity.TopContentItems = newContentItems;
            // ReSharper restore PossibleNullReferenceException

            _helper.ClearItems();
            _helper.GenerateItems(newContentItems);
        }

        #endregion  

        

        #region SelectedItem

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem",
                                        typeof(IGroupContentItem),
                                        typeof(GroupContentGrid),
                                        new UIPropertyMetadata(null, OnSelectedItemPropertyChanged)
                                        );

        public IGroupContentItem SelectedItem
        {
            get
            {
                return (IGroupContentItem)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var entity = d as GroupContentGrid;
            if (entity == null) return;

            //Не обрабатываем, если изменение создано внтури контрола
            if(entity.IsInternalChange)
            {
                entity.IsInternalChange = false;
                return;
            }

            var contentItem = e.NewValue as IGroupContentItem;
            if (contentItem == null) return;

            //Не обрабатываем если уже является выбранным
            if(Equals(entity.ContentItems, contentItem.SubItemsCollection)) 
                return;

            _helper.SelectItem(contentItem);
        }

        #endregion 


        #region DefaultTitle

        public static readonly DependencyProperty DefaultTitleProperty =
            DependencyProperty.Register("DefaultTitle",
                                        typeof(String),
                                        typeof(GroupContentGrid),
                                        new UIPropertyMetadata(String.Empty, null)
                                        );

        public String DefaultTitle
        {
            get
            {
                return (String)GetValue(DefaultTitleProperty);
            }
            set
            {
                SetValue(DefaultTitleProperty, value);
            }
        }

        #endregion  

        private void SearchBoxEnter(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter || String.IsNullOrWhiteSpace(SearchBox.Text)) return;

            _helper.Search(SearchBox.Text); 
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            _helper.Search(SearchBox.Text);
        }

    }
}
