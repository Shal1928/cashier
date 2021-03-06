﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;

namespace ASofCP.Cashier.Views.Controls.GroupContentGridParts
{
    /// <summary>
    /// Interaction logic for GroupContentGrid.xaml
    /// </summary>
    public partial class GroupContentGrid : UserControl
    {
        //private static readonly ILog Log = LogManager.GetLogger(typeof(GroupContentGrid));

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
            //Log.Debug("OnSelectedItemPropertyChanged {0}; N: {1}; O: {2}", d.GetType(), e.NewValue, e.OldValue);

            //Показывать ошибку
            var entity = d as GroupContentGrid;
            if(entity == null) return;

            //Log.Debug("DependencyObject d as GroupContentGrid");

            var newContentItems = e.NewValue as GroupContentList;
            if(newContentItems.IsNullOrEmpty()) return;

            //Log.Debug("e.NewValue as GroupContentList");

            //Log.Debug("newContentItems.IsTop = ", newContentItems.IsTop);

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
            //Log.Debug("OnSelectedItemPropertyChanged {0}; N: {1}; O: {2}", d.GetType(), e.NewValue, e.OldValue);

            var entity = d as GroupContentGrid;
            if (entity == null) return;

            //Log.Debug("DependencyObject d as GroupContentGrid");

            var contentItem = e.NewValue as IGroupContentItem;
            if (contentItem == null) return;

            //Log.Debug("e.NewValue as IGroupContentItem");

            //Log.Debug("!contentItem.IsFinal = ", !contentItem.IsFinal);
            //Log.Debug("Equals(entity.ContentItems, contentItem.SubItemsCollection) = ", Equals(entity.ContentItems, contentItem.SubItemsCollection));

            //Не обрабатываем если уже является выбранным
            if (!contentItem.IsFinal && Equals(entity.ContentItems, contentItem.SubItemsCollection))
                return;

            //Log.Debug("contentItem.SubItemsCollection.IsNullOrEmpty() = ", contentItem.SubItemsCollection.IsNullOrEmpty());

            if (contentItem.SubItemsCollection.IsNullOrEmpty()) return;

            entity.PreviousContentItems = entity.ContentItems;
            entity.ContentItems = contentItem.SubItemsCollection;
            //if (!contentItem.SubItemsCollection.IsNullOrEmpty())
            //{
            //    entity.PreviousContentItems = entity.ContentItems;
            //    entity.ContentItems = contentItem.SubItemsCollection;
            //}

           
            _helper.TitleBlockUpdate(entity.IsSub, entity.IsSub ? contentItem.Title : entity.DefaultTitle);
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
            if (String.IsNullOrWhiteSpace(SearchBox.Text)) return;
            
            _helper.Search(SearchBox.Text);
        }

    }
}
