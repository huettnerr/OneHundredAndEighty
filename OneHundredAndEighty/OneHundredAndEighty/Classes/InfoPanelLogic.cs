#region Usings

using OneHundredAndEighty.Controls;
using OneHundredAndEighty.Score;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

#endregion

namespace OneHundredAndEighty
{
    public class InfoPanelLogic //  Класс логики инфо-панели
    {
        //private readonly MainWindow mainWindow = (MainWindow)Application.Current.MainWindow; //  Ссылка на главное окно для доступа к элементам
        public InfoControl InfoControl { get => ((MainWindow)Application.Current.MainWindow).InfoControl; }


        private readonly TimeSpan panelFadeTime = TimeSpan.FromSeconds(0.5); //  Время анимации фейда панели

        public void PanelShow()
        {
            InfoControl.InfoPanel.Visibility = Visibility.Visible;
            var animation = new DoubleAnimation(0, 1, panelFadeTime);
            InfoControl.InfoPanel.BeginAnimation(UIElement.OpacityProperty, animation);
        } //  Спрятать инфо-панель

        public void PanelHide()
        {
            var animation = new DoubleAnimation(1, 0, panelFadeTime);
            InfoControl.InfoPanel.BeginAnimation(UIElement.OpacityProperty, animation);
            InfoControl.InfoPanel.Visibility = Visibility.Hidden;
        } //  Показать инфо-панель

        public void TextLogAdd(string s) //  Новая строка в текстовую панель
        {
            InfoControl.TextLog.Text += new StringBuilder().Append(s).Append("\n").ToString();
            InfoControl.TextLog.ScrollToEnd(); //  Прокручиваем вниз
        }

        public void TextLogUndo() // Удаление последный строки в текстовой панели
        {
            InfoControl.TextLog.Text = InfoControl.TextLog.Text.Remove(InfoControl.TextLog.Text.LastIndexOf("\n"));
            InfoControl.TextLog.Text = InfoControl.TextLog.Text.Remove(InfoControl.TextLog.Text.LastIndexOf("\n"));
            InfoControl.TextLog.AppendText("\n");
            InfoControl.TextLog.ScrollToEnd(); //  Прокручиваем вниз
        }

        public void TextLogClear() //  Очищаем текстовую панель текстовую панель
        {
            InfoControl.TextLog.Clear();
        }

        public void UndoThrowButtonOn() //  Разблокируем кнопку отмены броска
        {
            InfoControl.UndoThrow.IsEnabled = true;
        }

        public void UndoThrowButtonOff() //  Блокируем кнопку отмены броска
        {
            InfoControl.UndoThrow.IsEnabled = false;
        }
    }
}