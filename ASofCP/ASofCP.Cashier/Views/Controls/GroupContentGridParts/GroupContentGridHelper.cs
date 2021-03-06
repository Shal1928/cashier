﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ASofCP.Cashier.Views.Controls.GroupContentGridParts.Models;
using ASofCP.Cashier.Helpers;
using ASofCP.Cashier.Views.Controls.SquareButtonParts;
using log4net;

namespace ASofCP.Cashier.Views.Controls.GroupContentGridParts
{
    public class GroupContentGridHelper
    {
        //private static readonly ILog Log = LogManager.GetLogger(typeof(GroupContentGridHelper));

        private readonly GroupContentGrid _entity;
        // ReSharper disable StaticFieldInGenericType
        private static readonly DispatcherTimer DispatcherTimer = new DispatcherTimer();
        // ReSharper restore StaticFieldInGenericType

        public GroupContentGridHelper(GroupContentGrid entity)
        {
            _entity = entity;
        }

        public Button CreateItemControl(IGroupContentItem item, int column, int row)
        {
            var itemControl = new SquareButton(item.Background)
            {
                Content = item.Title,
                Height = 42,
                MinWidth = 170,
                FontSize = 14,
                Margin = new Thickness(2),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Foreground = new SolidColorBrush { Color = item.Foreground.ToMediaColor() }
            };

            itemControl.Click += delegate
            {
                if (_entity.IsSub && !item.IsFinal) return;

                _entity.SelectedItem = item;

                var binding = _entity.GetBindingExpression(GroupContentGrid.SelectedItemProperty);
                if (binding != null) binding.UpdateSource();
            };

            Grid.SetColumn(itemControl, column);
            Grid.SetRow(itemControl, row);

            return itemControl;
        }

        public void AddRow()
        {
            var height = new GridLength(0, GridUnitType.Auto);
            _entity.RootWidget.RowDefinitions.Add(new RowDefinition { Height = height });
        }

        public void AddItem(Control itemControl)
        {
            _entity.RootWidget.Children.Add(itemControl);
        }

        public void GenerateItems(IList<IGroupContentItem> itemsCollection)
        {
            //Log.Debug("GenerateItems count: {0}", itemsCollection.Count);

            var curCol = 0;
            var curRow = 0;
            foreach (var contentItem in itemsCollection)
            {
                //Log.Debug("curCol > 1 < {0}", curCol);

                if (curCol > 1)
                {
                    curCol = 0;
                    curRow++;
                }

                AddRow();
                Control itemControl = CreateItemControl(contentItem, curCol, curRow);
                AddItem(itemControl);
                curCol++;
            }
        }

        public void ClearItems()
        {
            _entity.RootWidget.Children.Clear();
            TitleBlockUpdate(false, _entity.DefaultTitle);
        }

        internal void TitleBlockUpdate(bool isSub, string title)
        {
            _entity.TitleBlock.Text = title;
            _entity.TitleBlock.TextDecorations = isSub ? TextDecorations.Underline : null;
            _entity.TitleBlock.Cursor = isSub ? Cursors.Hand : Cursors.Arrow;
        }

        internal void Search(String query)
        {
            var result = Search(query, _entity.TopContentItems ?? _entity.ContentItems);

            String searchResult;
            Brush resultBrush;
            if (result != null)
            {
                _entity.SelectedItem = result.Result;
                _entity.SearchBox.Text = String.Empty;

                if (_entity.SelectedItem.SubItemsCollection.NotEmpty()) return;

                searchResult = String.Format("Элемент \"{0}\" найден и добавлен в чек!", query);
                resultBrush = new SolidColorBrush(Color.FromRgb(100, 169, 107));
            }
            else
            {
                searchResult = String.Format("По запросу \"{0}\" ничего не найдено!", query);
                resultBrush = new SolidColorBrush(Color.FromRgb(220, 20, 60));
            }

            _entity.SearchResult.Foreground = resultBrush;
            _entity.SearchResult.Text = searchResult;
            _entity.SearchResult.Visibility = Visibility.Visible;

            DispatcherTimer.Tick += delegate
            {
                _entity.SearchResult.Visibility = Visibility.Hidden;
                DispatcherTimer.Stop();
            };
            DispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 7000);
            DispatcherTimer.Start();
            _entity.SearchBox.Text = String.Empty;
        }

        internal static GroupSearchResult Search(String query, GroupContentList searchScope)
        {
            foreach (var item in searchScope)
            {
                if (CheckGroupContentListEq(query, item)) return new GroupSearchResult(item, searchScope);
                if (item.SubItemsCollection.IsNullOrEmpty()) continue;

                var result = Search(query, item.SubItemsCollection);
                if (result != null) return result;
            }

            return null;
        }

        private static bool CheckGroupContentListEq(string query, IGroupContentItem item)
        {
            return String.Equals(query, item.Title, StringComparison.OrdinalIgnoreCase) ||
                   String.Equals(query, item.Code, StringComparison.OrdinalIgnoreCase);
        }
    }
}
