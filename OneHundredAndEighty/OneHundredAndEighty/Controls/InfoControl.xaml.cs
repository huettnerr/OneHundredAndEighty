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
            ((App)Application.Current).Game.AbortGameRequest();
        }

        private void UndoThrow_Click(object sender, RoutedEventArgs e) //  Кнопка отмены броска
        {
            ((App)Application.Current).Game.UndoThrow();
        }
    }
}
