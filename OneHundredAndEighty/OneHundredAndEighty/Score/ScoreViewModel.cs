using MyToolkit.Mvvm;
using OneHundredAndEighty.Classes;
using OneHundredAndEighty.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using OneHundredAndEighty.OBS;
using System.Windows.Media;
using OneHundredAndEighty.Statistics;
using System.Windows.Documents;

namespace OneHundredAndEighty.Score
{        
    public class ScoreViewModel : ViewModelBase
    {
        public ScoreControl ScoreControl { get; private set; }
        private ScoreWindow scoreWindow;

        public EventHandler<WhiteboardScore?> ScoresChanged;

        public ViewProperty<bool> IsSetMode { get; set; }
        public ViewProperty<string> BeginningPlayer { get; set; }
        public ViewProperty<string> ActivePlayer { get; set; }

        #region individual scores

        public ScoreStack _p1Throws;
        public ScoreStack P1Throws
        {
            get => _p1Throws;
            set => Set(ref _p1Throws, value);
        }

        public ScoreStack _p2Throws;
        public ScoreStack P2Throws
        {
            get => _p2Throws;
            set => Set(ref _p2Throws, value);
        }

        #endregion

        public ViewProperty<string> Summary { get; set; }

        public ViewProperty<string> P1Name { get; set; }
        public ViewProperty<int> P1Sets { get; set; }
        public ViewProperty<int> P1Legs { get; set; }
        public ViewProperty<int> P1Points { get; set; }
        public ViewProperty<string> P1Help { get; set; }         
        public ViewProperty<bool> P1HelpActive { get; set; }         
        
        
        public ViewProperty<string> P2Name { get; set; }
        public ViewProperty<int> P2Sets { get; set; }
        public ViewProperty<int> P2Legs { get; set; }
        public ViewProperty<int> P2Points { get; set; }
        public ViewProperty<string> P2Help { get; set; }
        public ViewProperty<bool> P2HelpActive { get; set; }

        public ScoreViewModel()
        {
            IsSetMode = new ViewProperty<bool>();
            BeginningPlayer = new ViewProperty<string>();
            ActivePlayer = new ViewProperty<string>();

            Summary = new ViewProperty<string>();

            P1Name = new ViewProperty<string>();
            P1Sets = new ViewProperty<int>();
            P1Legs = new ViewProperty<int>();
            P1Points = new ViewProperty<int>();
            P1Help = new ViewProperty<string>();
            P1HelpActive = new ViewProperty<bool>();

            P2Name = new ViewProperty<string>();
            P2Sets = new ViewProperty<int>();
            P2Legs = new ViewProperty<int>();
            P2Points = new ViewProperty<int>();
            P2Help = new ViewProperty<string>();
            P2HelpActive = new ViewProperty<bool>();

            P1Throws = new ScoreStack();
            P2Throws = new ScoreStack();
            ScoreStack.ThrowEvents += StatisticsOverlayManager.NewThrowEventHandler;

            ScoresChanged += updateScores;
        }

        public void NewGame(int points, int legs, int sets, Player p1, Player p2, Player first)
        {
            CreateScoreControl();

            Summary.Val = $"First to {sets} sets in {legs} legs";
            if(sets > 1)
            {
                IsSetMode.Val = true;
                Summary.Val = $"First to {sets} sets in {legs} legs";
            }
            else
            {
                IsSetMode.Val = false;
                Summary.Val = $"First to {legs} legs";
            }

            P1Throws.Init(p1);
            P2Throws.Init(p2);

            P1Name.Val = p1.Name;
            P2Name.Val = p2.Name;

            P1Sets.Val = 0;
            P1Legs.Val = 0;
            P2Sets.Val = 0;
            P2Legs.Val = 0;

            ClearScores(p1, points);
            ClearScores(p2, points);
            DotSet(first);
            WhoThrowSliderSet(first);
        } //  Установка в 0 в начале игры

        public void DotSet(Player p)
        {
            ScoreControl?.DotSet(p);

            BeginningPlayer.Val = p.Tag;
        }

        public void WhoThrowSliderSet(Player p)
        {
            ScoreControl?.WhoThrowSliderSet(p);

            ActivePlayer.Val = p.Tag;
        }

        public void HelpCheck(Player p)
        {
            if (updateFinishHelp(p, out string txt))
            {
                if (p.Tag.Equals("Player1")) P1HelpActive.Val = true;
                else if (p.Tag.Equals("Player2")) P2HelpActive.Val = true;
            }
            else
            {
                if (p.Tag.Equals("Player1")) P1HelpActive.Val = false;
                else if (p.Tag.Equals("Player2")) P2HelpActive.Val = false;
            }
        }

        private bool updateFinishHelp(Player p, out string helpText)
        {
            Finish f = FinishHelper.GetBestFinish(p.pointsToOut, p.ThrowsLeft);
            if (!f?.ObsScene.Equals(String.Empty) ?? false && p.ThrowsLeft > 0)
            {
                ObsManager.ChangeBoardView(f.ObsScene);
            }
            else
            {
                ObsManager.NormalBoardView();
            }

            helpText = (f is object) ? f.HelpText : "";
            if (p.Tag.Equals("Player1")) P1Help.Val = helpText;
            else if (p.Tag.Equals("Player2")) P2Help.Val = helpText;

            return (f is object);
        }

        public void CountThrow(ref Player p, ref Throw t)
        {
            //Add Throw
            if (p.Tag.Equals("Player1"))
            {
                P1Throws?.AddThrow(t, p, ScoresChanged);
            }
            else if (p.Tag.Equals("Player2"))
            {
                P2Throws?.AddThrow(t, p, ScoresChanged);
            }
        }

        public void UndoThrow(Player p)
        {
            if (p.Tag.Equals("Player1"))
            {
                if(P1Throws?.UndoThrow(ScoresChanged) ?? false)
                {
                    P2Throws?.RestoreWhiteboard(ScoresChanged);
                    P2Points.Val = P2Throws?.WhiteboardScores.Last().PointsToGo ?? 0;
                }
            }
            else if (p.Tag.Equals("Player2"))
            {
                if (P2Throws?.UndoThrow(ScoresChanged) ?? false)
                {
                    P1Throws?.RestoreWhiteboard(ScoresChanged);
                    P1Points.Val = P1Throws?.WhiteboardScores.Last().PointsToGo ?? 0;
                }
            }
        }

        private void updateScores(object s, WhiteboardScore? wbs)
        {
            if(wbs.HasValue)
            {
                if (wbs.Value.Player.Tag.Equals("Player1"))
                {
                    P1Points.Val = wbs.Value.PointsToGo;
                }
                else if (wbs.Value.Player.Tag.Equals("Player2"))
                {
                    P2Points.Val = wbs.Value.PointsToGo;
                }
            }
        }

        public void ClearScores(Player p, int pointsToGo)
        {
            if (p.Tag.Equals("Player1"))
            {
                P1Throws.ClearWhiteboard(p, pointsToGo);
                P1Points.Val = pointsToGo;
            }
            else if (p.Tag.Equals("Player2"))
            {
                P2Throws.ClearWhiteboard(p, pointsToGo);
                P2Points.Val = pointsToGo;
            }
        }


        public void LegsClear()
        {
            P1Legs.Val = 0;
            P2Legs.Val = 0;
        }

        public void LegsSet(Player p)
        {
            if (p.Tag.Equals("Player1")) P1Legs.Val = p.legsWon;
            else if (p.Tag.Equals("Player2")) P2Legs.Val = p.legsWon;
        }

        public void SetsSet(Player p)
        {
            if (p.Tag.Equals("Player1")) P1Sets.Val = p.setsWon;
            else if (p.Tag.Equals("Player2")) P2Sets.Val = p.setsWon;
        }

        public void CloseScore()
        {
            scoreWindow?.Close();
        }

        public void CreateScoreControl()
        {
            if(scoreWindow is object)
            {
                scoreWindow.Close();
            }

            scoreWindow = new ScoreWindow();
            StatisticsOverlayManager.scoreWindow = scoreWindow;
            ScoreControl = scoreWindow.Score as ScoreControl;
            scoreWindow.Score.DataContext = this;

            //scoreWindow.Content = ScoreControl;
            //scoreWindow.Title = "OneEightyScore";
            //scoreWindow.AllowsTransparency = true;
            //scoreWindow.Background = Brushes.Transparent;
            //scoreWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            //scoreWindow.ResizeMode = ResizeMode.NoResize;
            //scoreWindow.ShowInTaskbar = false;
            //scoreWindow.WindowStyle = WindowStyle.None;
            //scoreWindow.Width = 600;
            //scoreWindow.Height = 120;
            scoreWindow.Show();

            Task.Delay(200).ContinueWith(t =>
            {
                ((App)Application.Current).Dispatcher.Invoke(() => { ((App)Application.Current).MainWindow.Focus(); ((App)Application.Current).MainWindow.Activate(); ((App)Application.Current).MainWindow.Show(); });
            });
        }
    }
}
