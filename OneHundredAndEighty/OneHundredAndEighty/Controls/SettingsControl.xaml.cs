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
    /// Interaktionslogik für SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        private void NewPlayer_Click(object sender, RoutedEventArgs e) //  Кнопка нового игрока
        {
            ((MainWindow)Application.Current.MainWindow).FadeIn();
            NewPlayer.ShowNewPlayerRegisterWindow();
            ((MainWindow)Application.Current.MainWindow).FadeOut();
        }

        private void GameOn_Click(object sender, RoutedEventArgs e) //  Кнопка GAMEON
        {
            ((App)Application.Current).Game.StartGame((int)Player1NameCombobox.SelectedValue, (int)Player2NameCombobox.SelectedValue);
        }
    }
}
