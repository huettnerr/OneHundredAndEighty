﻿using MyToolkit.Mvvm;
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

namespace OneHundredAndEighty.Score
{        
    public struct WhiteboardScore
    {
        public string PlayerTag { get; set; }
        public int PointsThrown { get; set; }
        public int PointsToGo { get; set; }
        public int DartCount { get; set; }

        public WhiteboardScore(int points)
        {
            PlayerTag = "";
            PointsThrown = 0;
            PointsToGo = points;
            DartCount = 0;
        }

        public WhiteboardScore(Player p)
        {
            PlayerTag = p.Tag;
            PointsThrown = p.IsTurnFault ? 0 : p.handPoints;
            PointsToGo = p.pointsToOut;
            DartCount = 0;
        }
    }

    public class ScoreViewModel : ViewModelBase
    {
        public ScoreControl ScoreControl { get => ((MainWindow)Application.Current.MainWindow).InfoControl.ScoreControl; }

        public EventHandler ScoresChanged;

        public ViewProperty<bool> IsSetMode { get; set; }
        public ViewProperty<string> BeginningPlayer { get; set; }
        public ViewProperty<string> ActivePlayer { get; set; }

        #region individual scores

        public ObservableCollection<WhiteboardScore> whiteboardScoresP1 { get; set; }
        public ObservableCollection<WhiteboardScore> whiteboardScoresP2 { get; set; }
        public ObservableCollection<WhiteboardScore> allMatchScores { get; set; }

        #endregion

        public ViewProperty<string> P1Name { get; set; }
        public ViewProperty<int> P1Sets { get; set; }
        public ViewProperty<int> P1Legs { get; set; }
        public ViewProperty<int> P1Points { get; set; }
        public ViewProperty<string> P1Help { get; set; }       
        public ViewProperty<double> P1Avg { get; set; }       
        
        
        public ViewProperty<string> P2Name { get; set; }
        public ViewProperty<int> P2Sets { get; set; }
        public ViewProperty<int> P2Legs { get; set; }
        public ViewProperty<int> P2Points { get; set; }
        public ViewProperty<string> P2Help { get; set; }
        public ViewProperty<double> P2Avg { get; set; }

        public ScoreViewModel()
        {
            IsSetMode = new ViewProperty<bool>();
            BeginningPlayer = new ViewProperty<string>();
            ActivePlayer = new ViewProperty<string>();

            whiteboardScoresP1 = new ObservableCollection<WhiteboardScore>();
            whiteboardScoresP2 = new ObservableCollection<WhiteboardScore>();
            allMatchScores = new ObservableCollection<WhiteboardScore>();

            P1Name = new ViewProperty<string>();
            P1Sets = new ViewProperty<int>();
            P1Legs = new ViewProperty<int>();
            P1Points = new ViewProperty<int>();
            P1Help = new ViewProperty<string>();
            P1Avg = new ViewProperty<double>();

            P2Name = new ViewProperty<string>();
            P2Sets = new ViewProperty<int>();
            P2Legs = new ViewProperty<int>();
            P2Points = new ViewProperty<int>();
            P2Help = new ViewProperty<string>();
            P2Avg = new ViewProperty<double>();
        }

        public void NewGame(int points, string legs, string sets, Player p1, Player p2, Player first)
        {
            ScoreControl.MainBoxSummary.Content = new StringBuilder().Append("First to ").Append(sets).Append(" sets in ").Append(legs).Append(" legs").ToString();
            IsSetMode.Val = true;
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
            if (!f?.ObsScene.Equals(String.Empty) ?? false)
            {
                //TODO: Set OBS
            }

            helpText = (f is object) ? f.HelpText : "";
            if (p.Tag.Equals("Player1")) P1Help.Val = helpText;
            else if (p.Tag.Equals("Player2")) P2Help.Val = helpText;

            return (f is object);
        }

        public void CountThrow(ref Player p, ref Throw t)
        {
            //Calculate new Points
            p.pointsToOut -= (int)t.Points; //  Вычитаем набраные за бросок очки игрока из общего результата лега
            p.handPoints += (int)t.Points; //  Плюсуем набраные за подход очки игрока

            if (p.pointsToOut <= 1) //  Если игрок ушел в минус или оставил единицу, или закрыл лег не корректно (не через удвоение или Bulleye)
            {
                t.IsFault = true; //  Помечаем бросок как штрафной 
                p.pointsToOut += p.handPoints; //  Отменяем подход игрока
            }

            //Update Live-Scores
            if (p.Tag.Equals("Player1"))
            {
                P1Points.Val = p.pointsToOut;
            }
            else if (p.Tag.Equals("Player2"))
            {
                P2Points.Val = p.pointsToOut;
            }
        }

        public void AddTurnScore(Player p)
        {
            WhiteboardScore wbs = new WhiteboardScore(p);
            if (wbs.PlayerTag.Equals("Player1"))
            {
                P1Points.Val = wbs.PointsToGo;
                wbs.DartCount = 3 * whiteboardScoresP1.Count;
                whiteboardScoresP1.Add(wbs);
            }
            else if (wbs.PlayerTag.Equals("Player2"))
            {
                P2Points.Val = wbs.PointsToGo;
                wbs.DartCount = 3 * whiteboardScoresP2.Count;
                whiteboardScoresP2.Add(wbs);
            }

            allMatchScores.Add(wbs);
            updatePlayerStatistics(p.Tag);
            ScoresChanged?.Invoke(this, new EventArgs());
        }

        public void UndoScore()
        {
            if(ActivePlayer.Equals("Player1") && whiteboardScoresP2.Count > 1)  whiteboardScoresP2.Remove(whiteboardScoresP2.Last());
            else if (ActivePlayer.Equals("Player2") && whiteboardScoresP1.Count > 1) whiteboardScoresP1.Remove(whiteboardScoresP1.Last());

            if (allMatchScores.Count > 1) allMatchScores.Remove(allMatchScores.Last());
            ScoresChanged?.Invoke(this, new EventArgs());
        }

        public void ClearScores(int p)
        {
            whiteboardScoresP1.Clear();
            whiteboardScoresP1.Add(new WhiteboardScore(p));

            whiteboardScoresP2.Clear();
            whiteboardScoresP2.Add(new WhiteboardScore(p));

            P1Points.Val = p;
            P2Points.Val = p;

            ScoresChanged?.Invoke(this, new EventArgs());
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

        private void updatePlayerStatistics(string tag)
        {
            List<int> throws = allMatchScores.Where((s) => s.PlayerTag.Equals(tag)).Select(s => s.PointsThrown).ToList();
            double avg = throws.Average();

            if (tag.Equals("Player1")) P1Avg.Val = avg;
            else if (tag.Equals("Player2")) P2Avg.Val = avg;
        }
    }
}
