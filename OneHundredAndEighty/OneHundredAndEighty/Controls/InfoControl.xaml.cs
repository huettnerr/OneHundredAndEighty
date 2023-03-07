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
    /// Interaktionslogik für InfoControl.xaml
    /// </summary>
    public partial class InfoControl : UserControl
    {
        public InfoControl()
        {
            InitializeComponent();
        }

        private void EndMatchButton_Click(object sender, RoutedEventArgs e) //  Кнопка отмены матча
        {
            ((MainWindow)Application.Current.MainWindow).FadeIn();
            var window = new Windows.AbortWindowConfirm { Owner = (MainWindow)Application.Current.MainWindow };
            window.ShowDialog();
            if (window.Result)
            {
                ((App)Application.Current).Game.AbortGame();
            }

            ((MainWindow)Application.Current.MainWindow).FadeOut();
        }

        private void UndoThrow_Click(object sender, RoutedEventArgs e) //  Кнопка отмены броска
        {
            ((App)Application.Current).Game.UndoThrow();
        }
    }
}
