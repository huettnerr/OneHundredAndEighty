#region Usings

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

#endregion

namespace OneHundredAndEighty
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            ((App)Application.Current).Game = new Game();
            InitializeComponent();
            DBwork.LoadPlayers();
            DBwork.LoadSettings();
        }

        public void FadeIn() //  Затухание главного окна
        {
            Opacity = 0.4;
        }

        public void FadeOut() //  Появление главного окна
        {
            Opacity = 1;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e) //  Выход из приложения
        {
            FadeIn();
            var window = new Windows.ExitWindow {Owner = this};
            window.ShowDialog();
            if (window.result) //  Если нажата отмена выхода
            {
                e.Cancel = true; //  Остаемся
            }
            else // Если выходим
            {
                DBwork.SaveSettings(); //  Сохраняем настройки
            }

            FadeOut();
        }

        private void PlayerTabNameCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e) //  Изменение выделенного игрока в вкладке данных игроков
        {
            var comboBox = sender as ComboBox;
            if (comboBox.SelectedIndex == -1)
            {
                if (comboBox.Name == "Player1TabNameCombobox")
                {
                    PlayerOverview.ClearPlayer1();
                }
                else
                {
                    PlayerOverview.ClearPlayer2();
                }
            }
            else
            {
                if (comboBox.Name == "Player1TabNameCombobox")
                {
                    PlayerOverview.ClearPlayer1();
                    PlayerOverview.RefreshPlayer1((int) comboBox.SelectedValue);
                }
                else
                {
                    PlayerOverview.ClearPlayer2();
                    PlayerOverview.RefreshPlayer2((int) comboBox.SelectedValue);
                }
            }

            if (Player1TabNameCombobox.SelectedIndex != -1 && Player2TabNameCombobox.SelectedIndex != -1) //  Если выбраны два игрока обновляем PvP данные
            {
                PlayerOverview.RefreshPvP((int) Player1TabNameCombobox.SelectedValue, (int) Player2TabNameCombobox.SelectedValue);
            }
        }
    }
}