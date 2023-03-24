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

namespace OneHundredAndEighty.Score
{        
    public class ScoreViewModel : ViewModelBase
    {
        public ScoreControl ScoreControl { get => ((MainWindow)Application.Current.MainWindow).InfoControl.ScoreControl; }

        public EventHandler<WhiteboardScore> ScoresChanged;

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

        public ViewProperty<string> P1Name { get; set; }
        public ViewProperty<int> P1Sets { get; set; }
        public ViewProperty<int> P1Legs { get; set; }
        public ViewProperty<int> P1Points { get; set; }
        public ViewProperty<string> P1Help { get; set; }         
        
        
        public ViewProperty<string> P2Name { get; set; }
        public ViewProperty<int> P2Sets { get; set; }
        public ViewProperty<int> P2Legs { get; set; }
        public ViewProperty<int> P2Points { get; set; }
        public ViewProperty<string> P2Help { get; set; }

        public ScoreViewModel()
        {
            IsSetMode = new ViewProperty<bool>();
            BeginningPlayer = new ViewProperty<string>();
            ActivePlayer = new ViewProperty<string>();

            P1Name = new ViewProperty<string>();
            P1Sets = new ViewProperty<int>();
            P1Legs = new ViewProperty<int>();
            P1Points = new ViewProperty<int>();
            P1Help = new ViewProperty<string>();

            P2Name = new ViewProperty<string>();
            P2Sets = new ViewProperty<int>();
            P2Legs = new ViewProperty<int>();
            P2Points = new ViewProperty<int>();
            P2Help = new ViewProperty<string>();
        }

        public void NewGame(int points, int legs, int sets, Player p1, Player p2, Player first)
        {
            ScoreControl.MainBoxSummary.Content = $"First to {sets} sets in {legs} legs";
            IsSetMode.Val = sets > 1;

            P1Throws = new ScoreStack(p1);
            P2Throws = new ScoreStack(p2);

            P1Name.Val = p1.Name;
            P2Name.Val = p2.Name;

            P1Sets.Val = 0;
            P1Legs.Val = 0;
            P2Sets.Val = 0;
            P2Legs.Val = 0;

            ScoreControl.HelpHide(p1);
            ScoreControl.HelpHide(p2);
            ClearScores(points);
            DotSet(first);
            WhoThrowSliderSet(first);
        } //  Установка в 0 в начале игры

        public void DotSet(Player p)
        {
            ScoreControl.DotSet(p);

            BeginningPlayer.Val = p.Tag;
        }

        public void WhoThrowSliderSet(Player p)
        {
            ScoreControl.WhoThrowSliderSet(p);

            ActivePlayer.Val = p.Tag;
        }

        public void HelpCheck(Player p)
        {
            if (updateFinishHelp(p, out string txt)) ScoreControl.HelpShow(p, txt);
            else ScoreControl.HelpHide(p);
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
            //Update Live-Scores
            PointsSet(p);

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

        public void PointsSet(Player p)
        {
            if (p.Tag.Equals("Player1"))
            {
                P1Points.Val = p.pointsToOut;
            }
            else if (p.Tag.Equals("Player2"))
            {
                P2Points.Val = p.pointsToOut;
            }
        }

        public void UndoThrow(Player p)
        {
            if (p.Tag.Equals("Player1"))
            {
                if(P1Throws?.UndoThrow(ScoresChanged) ?? false)
                {
                    P2Throws?.RestoreWhiteboard(ScoresChanged);
                }
            }
            else if (p.Tag.Equals("Player2"))
            {
                if (P2Throws?.UndoThrow(ScoresChanged) ?? false)
                {
                    P1Throws?.RestoreWhiteboard(ScoresChanged);
                }
            }
        }

        public void ClearScores(int pointsToGo)
        {
            P1Throws.ClearWhiteboard(pointsToGo);
            P2Throws.ClearWhiteboard(pointsToGo);

            P1Points.Val = pointsToGo;
            P2Points.Val = pointsToGo;
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
    }
}
