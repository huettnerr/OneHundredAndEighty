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
        public ScoreControl ScoreControl { get => ((MainWindow)Application.Current.MainWindow).InfoControl.ScoreControl; }
        public ScoreViewModel ScoreVM { get => ((App)Application.Current).Game.scoreVM; }

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

        public void PanelNewGame(int points, string legs, string sets, Player p1, Player p2, Player first)
        {
            ScoreControl.MainBoxSummary.Content = new StringBuilder().Append("First to ").Append(sets).Append(" sets in ").Append(legs).Append(" legs").ToString();
            ScoreVM.IsSetMode.Val = true;
            ScoreVM.P1Name.Val = p1.Name;
            ScoreVM.P2Name.Val = p2.Name;

            ScoreVM.P1Sets.Val = 0;
            ScoreVM.P1Legs.Val = 0;
            ScoreVM.P2Sets.Val = 0;
            ScoreVM.P2Legs.Val = 0;

            ScoreControl.HelpHide(p1);
            ScoreControl.HelpHide(p2);
            ScoreVM.ClearScores(points);
            DotSet(first);
            WhoThrowSliderSet(first);
        } //  Установка в 0 в начале игры

        public void DotSet(Player p)
        {
            ScoreControl.DotSet(p);

            ScoreVM.BeginningPlayer.Val = p.Tag;
        }

        public void WhoThrowSliderSet(Player p)
        {
            ScoreControl.WhoThrowSliderSet(p);

            ScoreVM.ActivePlayer.Val = p.Tag;
        }

        public void HelpCheck(Player p)
        {
            if(ScoreVM.UpdateFinishHelp(p, out string txt)) ScoreControl.HelpShow(p, txt);
            else ScoreControl.HelpHide(p);
        }

        //public void PointsSet(Player p)
        //{
        //    ScoreVM.AddScore(p);
        //}

        //public void PointsClear(int p)
        //{
        //    ScoreVM.ClearScores(p);
        //}

        public void LegsClear()
        {
            ScoreVM.P1Legs.Val = 0;
            ScoreVM.P2Legs.Val = 0;
        }

        public void LegsSet(Player p)
        {
            if (p.Tag.Equals("Player1")) ScoreVM.P1Legs.Val = p.legsWon;
            else if (p.Tag.Equals("Player2")) ScoreVM.P2Legs.Val = p.legsWon;
        }

        public void SetsSet(Player p)
        {
            if (p.Tag.Equals("Player1")) ScoreVM.P1Sets.Val = p.setsWon;
            else if (p.Tag.Equals("Player2")) ScoreVM.P2Sets.Val = p.setsWon;
        }

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

            ScoreVM.UndoScore();
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