using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ASofCP.Cashier.Views.Controls.SquareButtonParts.Colors;
using MColor = System.Windows.Media.Color;

namespace ASofCP.Cashier.Views.Controls.SquareButtonParts
{
    public class SquareButton : Button
    {
        private readonly SquareButtonDic _partDic = new SquareButtonDic();

        private readonly Dictionary<String, ResourceDictionary> _resourceDictionary = new Dictionary<String, ResourceDictionary> 
        {
            {"WHITE", new WhiteDic()}, 
            {"BLACK", new BlackDic()},
            {"GRAY", new GrayDic()},
            {"RED", new RedDic()},
            {"ORANGE", new OrangeDic()},
            {"YELLOW", new YellowDic()},
            {"GREEN", new GreenDic()},
            {"BLUE", new BlueDic()},
        } ;

        public SquareButton()
        {
            Initialize("GRAY");
        }

        public SquareButton(String color)
        {
            Initialize(color);
        }

        private void Initialize(String color)
        {
            _partDic.MergedDictionaries.RemoveAt(0);
            var rd = _resourceDictionary.ContainsKey(color) ? _resourceDictionary[color] : _resourceDictionary["GRAY"];
            _partDic.MergedDictionaries.Add(rd);
            Style = (Style)_partDic["SquareButtonStyle"];
        }
    }
}
