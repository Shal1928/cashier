using System.Windows;
using System.Windows.Controls;

namespace ASofCP.Cashier.Views.Controls.SquareButtonParts
{
    public class SquareButton : Button
    {
        public SquareButton()
        {
            var partsDic = new SquareButtonDic();
            Style = (Style)partsDic["SquareButtonStyle"];
        }
    }
}
