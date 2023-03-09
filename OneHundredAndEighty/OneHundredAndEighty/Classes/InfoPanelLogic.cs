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

        #region Finishes

        private readonly SortedList<int, string> checkoutTableThreeThrows = new SortedList<int, string>()
                                                                   {
                                                                       [2] = "D1",
                                                                       [4] = "D2",
                                                                       [6] = "D3",
                                                                       [8] = "D4",
                                                                       [10] = "D5",
                                                                       [12] = "D6",
                                                                       [14] = "D7",
                                                                       [16] = "D8",
                                                                       [18] = "D9",
                                                                       [20] = "D10",
                                                                       [22] = "D11",
                                                                       [24] = "D12",
                                                                       [26] = "D13",
                                                                       [28] = "D14",
                                                                       [30] = "D15",
                                                                       [32] = "D16",
                                                                       [34] = "D17",
                                                                       [36] = "D18",
                                                                       [38] = "D19",
                                                                       [40] = "D20",
                                                                       [3] = "1 D1",
                                                                       [5] = "1 D2",
                                                                       [7] = "3 D2",
                                                                       [9] = "1 D4",
                                                                       [11] = "3 D4",
                                                                       [13] = "5 D4",
                                                                       [15] = "7 D4",
                                                                       [17] = "9 D4",
                                                                       [19] = "3 D8",
                                                                       [21] = "5 D8",
                                                                       [23] = "7 D8",
                                                                       [25] = "1 D12",
                                                                       [27] = "3 D12",
                                                                       [29] = "5 D12",
                                                                       [31] = "7 D12",
                                                                       [33] = "1 D16",
                                                                       [35] = "3 D16",
                                                                       [37] = "5 D16",
                                                                       [39] = "7 D6",
                                                                       [41] = "9 D16",
                                                                       [42] = "10 D16",
                                                                       [43] = "3 D20",
                                                                       [44] = "4 D20",
                                                                       [45] = "13 D16",
                                                                       [46] = "6 D20",
                                                                       [47] = "7 D20",
                                                                       [48] = "16 D16",
                                                                       [49] = "17 D16",
                                                                       [50] = "18 D16",
                                                                       [51] = "19 D16",
                                                                       [52] = "20 D16",
                                                                       [53] = "13 D20",
                                                                       [54] = "14 D20",
                                                                       [55] = "15 D20",
                                                                       [56] = "16 D20",
                                                                       [57] = "17 D20",
                                                                       [58] = "18 D20",
                                                                       [59] = "19 D20",
                                                                       [60] = "20 D20",
                                                                       [61] = "T15 D8",
                                                                       [62] = "T10 D16",
                                                                       [63] = "T13 D12",
                                                                       [64] = "T16 D8",
                                                                       [65] = "T19 D4",
                                                                       [66] = "T14 D12",
                                                                       [67] = "T17 D8",
                                                                       [68] = "T20 D4",
                                                                       [69] = "T19 D6",
                                                                       [70] = "T18 D8",
                                                                       [71] = "T13 16",
                                                                       [72] = "T16 D12",
                                                                       [73] = "T19 D8",
                                                                       [74] = "T14 D16",
                                                                       [75] = "T17 D12",
                                                                       [76] = "T20 D8",
                                                                       [77] = "T19 D10",
                                                                       [78] = "T18 D12",
                                                                       [79] = "T19 D11",
                                                                       [80] = "T20 D10",
                                                                       [81] = "T19 D12",
                                                                       [82] = "Bull D16",
                                                                       [83] = "T17 D16",
                                                                       [84] = "T20 D12",
                                                                       [85] = "T15 D20",
                                                                       [86] = "T18 D18",
                                                                       [87] = "T17 D18",
                                                                       [88] = "T20 D14",
                                                                       [89] = "T19 D16",
                                                                       [90] = "T20 D15",
                                                                       [91] = "T17 D20",
                                                                       [92] = "T20 D16",
                                                                       [93] = "T19 D18",
                                                                       [94] = "T18 D20",
                                                                       [95] = "T19 D19",
                                                                       [96] = "T20 D18",
                                                                       [97] = "T19 D20",
                                                                       [98] = "T20 D19",
                                                                       [100] = "T20 D20",
                                                                       [99] = "T19 10 D16",
                                                                       [101] = "T20 9 D16",
                                                                       [102] = "T16 14 D20",
                                                                       [103] = "T19 6 D20",
                                                                       [104] = "T16 16 D20",
                                                                       [105] = "T20 13 D16",
                                                                       [106] = "T20 6 D20",
                                                                       [107] = "T19 10 D20",
                                                                       [108] = "T20 16 D16",
                                                                       [109] = "T20 17 D16",
                                                                       [110] = "T20 10 D20",
                                                                       [111] = "T19 14 D20",
                                                                       [112] = "T20 20 D16",
                                                                       [113] = "T19 16 D20",
                                                                       [114] = "T20 14 D20",
                                                                       [115] = "T20 15 D20",
                                                                       [116] = "T20 16 D20",
                                                                       [117] = "T20 17 D20",
                                                                       [118] = "T20 18 D20",
                                                                       [119] = "T19 12 Bull",
                                                                       [120] = "T20 20 D20",
                                                                       [121] = "T20 11 Bull",
                                                                       [122] = "T18 18 Bull",
                                                                       [123] = "T19 16 Bull",
                                                                       [124] = "T20 14 Bull",
                                                                       [125] = "25 T20 D20",
                                                                       [126] = "T19 19 Bull",
                                                                       [127] = "T20 17 Bull",
                                                                       [128] = "18 T20 Bull",
                                                                       [129] = "19 T20 Bull",
                                                                       [130] = "T20 20 Bull",
                                                                       [131] = "T20 T13 D16",
                                                                       [132] = "25 T19 Bull",
                                                                       [133] = "T20 T19 D8",
                                                                       [134] = "T20 T14 D16",
                                                                       [135] = "25 T20 Bull",
                                                                       [136] = "T20 T20 D8",
                                                                       [137] = "T20 T19 D10",
                                                                       [138] = "T20 T18 D12",
                                                                       [139] = "T19 T14 D20",
                                                                       [140] = "T20 T20 D10",
                                                                       [141] = "T20 T19 D12",
                                                                       [142] = "T20 T14 D20",
                                                                       [143] = "T20 T17 D16",
                                                                       [144] = "T20 T20 D12",
                                                                       [145] = "T20 T15 D20",
                                                                       [146] = "T20 T18 D16",
                                                                       [147] = "T20 T17 D18",
                                                                       [148] = "T20 T20 D14",
                                                                       [149] = "T20 T19 D16",
                                                                       [150] = "T20 T18 D18",
                                                                       [151] = "T20 T17 D20",
                                                                       [152] = "T20 T20 D16",
                                                                       [153] = "T20 T19 D18",
                                                                       [154] = "T20 T18 D20",
                                                                       [155] = "T20 T19 D19",
                                                                       [156] = "T20 T20 D18",
                                                                       [157] = "T20 T19 D20",
                                                                       [158] = "T20 T20 D19",
                                                                       [160] = "T20 T20 D20",
                                                                       [161] = "T20 T17 Bull",
                                                                       [164] = "T20 T18 Bull",
                                                                       [167] = "T20 T19 Bull",
                                                                       [170] = "T20 T20 Bull",
                                                                   }; //  Коллекция закрытия сета на один бросок

        private readonly SortedList<int, string> checkoutTableTwoThrows = new SortedList<int, string>()
                                                                 {
                                                                     [2] = "D1",
                                                                     [4] = "D2",
                                                                     [6] = "D3",
                                                                     [8] = "D4",
                                                                     [10] = "D5",
                                                                     [12] = "D6",
                                                                     [14] = "D7",
                                                                     [16] = "D8",
                                                                     [18] = "D9",
                                                                     [20] = "D10",
                                                                     [22] = "D11",
                                                                     [24] = "D12",
                                                                     [26] = "D13",
                                                                     [28] = "D14",
                                                                     [30] = "D15",
                                                                     [32] = "D16",
                                                                     [34] = "D17",
                                                                     [36] = "D18",
                                                                     [38] = "D19",
                                                                     [40] = "D20",
                                                                     [3] = "1 D1",
                                                                     [5] = "1 D2",
                                                                     [7] = "3 D2",
                                                                     [9] = "1 D4",
                                                                     [11] = "3 D4",
                                                                     [13] = "5 D4",
                                                                     [15] = "7 D4",
                                                                     [17] = "9 D4",
                                                                     [19] = "3 D8",
                                                                     [21] = "5 D8",
                                                                     [23] = "7 D8",
                                                                     [25] = "1 D12",
                                                                     [27] = "3 D12",
                                                                     [29] = "5 D12",
                                                                     [31] = "7 D12",
                                                                     [33] = "1 D16",
                                                                     [35] = "3 D16",
                                                                     [37] = "5 D16",
                                                                     [39] = "7 D6",
                                                                     [41] = "9 D16",
                                                                     [42] = "10 D16",
                                                                     [43] = "3 D20",
                                                                     [44] = "4 D20",
                                                                     [45] = "13 D16",
                                                                     [46] = "6 D20",
                                                                     [47] = "7 D20",
                                                                     [48] = "16 D16",
                                                                     [49] = "17 D16",
                                                                     [50] = "18 D16",
                                                                     [51] = "19 D16",
                                                                     [52] = "20 D16",
                                                                     [53] = "13 D20",
                                                                     [54] = "14 D20",
                                                                     [55] = "15 D20",
                                                                     [56] = "16 D20",
                                                                     [57] = "17 D20",
                                                                     [58] = "18 D20",
                                                                     [59] = "19 D20",
                                                                     [60] = "20 D20",
                                                                     [61] = "T15 D8",
                                                                     [62] = "T10 D16",
                                                                     [63] = "T13 D12",
                                                                     [64] = "T16 D8",
                                                                     [65] = "T19 D4",
                                                                     [66] = "T14 D12",
                                                                     [67] = "T17 D8",
                                                                     [68] = "T20 D4",
                                                                     [69] = "T19 D6",
                                                                     [70] = "T18 D8",
                                                                     [71] = "T13 16",
                                                                     [72] = "T16 D12",
                                                                     [73] = "T19 D8",
                                                                     [74] = "T14 D16",
                                                                     [75] = "T17 D12",
                                                                     [76] = "T20 D8",
                                                                     [77] = "T19 D10",
                                                                     [78] = "T18 D12",
                                                                     [79] = "T19 D11",
                                                                     [80] = "T20 D10",
                                                                     [81] = "T19 D12",
                                                                     [82] = "Bull D16",
                                                                     [83] = "T17 D16",
                                                                     [84] = "T20 D12",
                                                                     [85] = "T15 D20",
                                                                     [86] = "T18 D18",
                                                                     [87] = "T17 D18",
                                                                     [88] = "T20 D14",
                                                                     [89] = "T19 D16",
                                                                     [90] = "T20 D15",
                                                                     [91] = "T17 D20",
                                                                     [92] = "T20 D16",
                                                                     [93] = "T19 D18",
                                                                     [94] = "T18 D20",
                                                                     [95] = "T19 D19",
                                                                     [96] = "T20 D18",
                                                                     [97] = "T19 D20",
                                                                     [98] = "T20 D19",
                                                                     [100] = "T20 D20",
                                                                     [101] = "T17 Bull",
                                                                     [104] = "T18 Bull",
                                                                     [107] = "T19 Bull",
                                                                     [110] = "T20 Bull"
                                                                 }; //  Коллекция закрытия сета на два броска

        private readonly SortedList<int, string> checkoutTableOneThrow = new SortedList<int, string>()
                                                                {
                                                                    [2] = "D1",
                                                                    [4] = "D2",
                                                                    [6] = "D3",
                                                                    [8] = "D4",
                                                                    [10] = "D5",
                                                                    [12] = "D6",
                                                                    [14] = "D7",
                                                                    [16] = "D8",
                                                                    [18] = "D9",
                                                                    [20] = "D10",
                                                                    [22] = "D11",
                                                                    [24] = "D12",
                                                                    [26] = "D13",
                                                                    [28] = "D14",
                                                                    [30] = "D15",
                                                                    [32] = "D16",
                                                                    [34] = "D17",
                                                                    [36] = "D18",
                                                                    [38] = "D19",
                                                                    [40] = "D20",
                                                                    [50] = "Bull"
                                                                }; //  Коллекция закрытия сета на три броска

        #endregion

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
            PointsClear(points);
            DotSet(first);
            WhoThrowSliderSet(first);
        } //  Установка в 0 в начале игры

        public void DotSet(Player p)
        {
            ScoreControl.DotSet(p);

            ScoreVM.BeginningPlayer.Val = p.Tag.Equals("Player1") ? Players.Player1 : Players.Player2;
        }

        public void WhoThrowSliderSet(Player p)
        {
            ScoreControl.WhoThrowSliderSet(p);

            ScoreVM.ActivePlayer.Val = p.Tag.Equals("Player1") ? Players.Player1 : Players.Player2;
        }

        public void HelpCheck(Player p)
        {
            var table = checkoutTableThreeThrows;
            if (p.throw1 == null)
            {
                table = checkoutTableThreeThrows;
            }
            else if (p.throw2 == null && p.throw1 != null)
            {
                table = checkoutTableTwoThrows;
            }
            else if (p.throw3 == null && p.throw2 != null)
            {
                table = checkoutTableOneThrow;
            }

            if (table.ContainsKey(p.pointsToOut))
            {
                if(p.Tag.Equals("Player1")) ScoreVM.P1Help.Val = table[p.pointsToOut];
                else if (p.Tag.Equals("Player2")) ScoreVM.P2Help.Val = table[p.pointsToOut];
               
                ScoreControl.HelpShow(p, table[p.pointsToOut]);
            }
            else
            {
                if (p.Tag.Equals("Player1")) ScoreVM.P1Help.Val = "";
                else if (p.Tag.Equals("Player2")) ScoreVM.P2Help.Val = "";

                ScoreControl.HelpHide(p);
            }
        }

        public void PointsSet(Player p)
        {
            if (p.Tag.Equals("Player1")) ScoreVM.P1Points.Val = p.pointsToOut;
            else if (p.Tag.Equals("Player2")) ScoreVM.P2Points.Val = p.pointsToOut;
        }

        public void PointsClear(int p)
        {
            ScoreVM.P1Points.Val = p;
            ScoreVM.P2Points.Val = p;
        }

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