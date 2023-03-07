using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneHundredAndEighty.Controls
{
    /// <summary>
    /// Interaktionslogik für BoardControl.xaml
    /// </summary>
    public partial class BoardControl : UserControl
    {
        public BoardControl()
        {
            InitializeComponent();
        }

        private void PointsShow(object sender, MouseEventArgs e) //  Показ очков сектора
        {
            var shape = sender as Shape;
            HowManyPoints.Content = shape.Tag.ToString();
        }

        private void Throw(object sender, RoutedEventArgs e) //  Бросок
        {
            BoardPanelLogic.PanelHide(); //  Скрываем панель секторов
            ((App)Application.Current).Game.NextThrow(new Throw(sender));
            if (((App)Application.Current).Game.IsOn) //  Если игра продолжается
            {
                BoardPanelLogic.PanelShow(); //  Показываем панель секторов и бросаем дальше
            }
        }

        public void PanelShow()
        {
            BoardPanel.Visibility = System.Windows.Visibility.Visible;
        }

        public void PanelHide()
        {
            BoardPanel.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
