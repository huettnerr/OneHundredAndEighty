#region Usings

using OneHundredAndEighty.Controls;
using System;
using System.Windows;
using System.Windows.Media.Animation;

#endregion

namespace OneHundredAndEighty
{
    public class SettingsPanelLogic //  Класс логики панели настроек
    {
        private readonly MainWindow mainWindow = (MainWindow) Application.Current.MainWindow; //  Ссылка на главное окно для доступа к элементам
        private readonly TimeSpan panelFadeTime = TimeSpan.FromSeconds(0.5); //  Время анимации фейда панели

        public void PanelShow()
        {
            mainWindow.SettingsControl.SettingsPanel.Visibility = Visibility.Visible;
            var animation = new DoubleAnimation(0, 1, panelFadeTime);
            mainWindow.SettingsControl.SettingsPanel.BeginAnimation(UIElement.OpacityProperty, animation);
        } //  Показать панель настроек

        public void PanelHide()
        {
            var animation = new DoubleAnimation(1, 0, panelFadeTime);
            mainWindow.InfoControl.InfoPanel.BeginAnimation(UIElement.OpacityProperty, animation);
            mainWindow.SettingsControl.SettingsPanel.Visibility = Visibility.Hidden;
        } //  Спрятать панель настроек

        public Player WhoThrowFirst(Player p1, Player p2)
        {
            if (mainWindow.SettingsControl.Player1Radiobutton.IsChecked == true)
            {
                return p1;
            }
            else
            {
                return p2;
            }
        } //  Кто бросает первым

        public int PointsToGo()
        {
            return int.Parse(mainWindow.SettingsControl.PointsBox.Text);
        } //  Сколько очков в леге

        public int LegsToGo()
        {
            int bestOf = int.Parse(mainWindow.SettingsControl.LegBox.Text);
            return (int)Math.Ceiling((double)bestOf / 2);
        } //  Сколько играем легов в сете

        public int SetsToGo()
        {
            int bestOf = int.Parse(mainWindow.SettingsControl.SetBox.Text);
            return (int)Math.Ceiling((double)bestOf / 2);
        } //  Сколько легов в метче

        public string Player1Name()
        {
            return mainWindow.SettingsControl.Player1NameCombobox.Text;
        } //  Имя 1 игрока

        public string Player2Name()
        {
            return mainWindow.SettingsControl.Player2NameCombobox.Text;
        } //  Имя 2 игрока
    }
}