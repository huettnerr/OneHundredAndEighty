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
            PointsThrown = p.handPoints;
            PointsToGo = p.pointsToOut;
            DartCount = 0;
        }
    }

    public class ScoreViewModel : ViewModelBase
    {
        public EventHandler ScoresChanged;

        public ViewProperty<bool> IsSetMode { get; set; }
        public ViewProperty<string> BeginningPlayer { get; set; }
        public ViewProperty<string> ActivePlayer { get; set; }


        //List of Scores
        public static List<string> DartCountList = new List<string>() { 
            "", "3", "6", "9", "12", "15", "18", "21", "24", "27", "30", 
            "33", "36", "39", "42", "45", "48", "51", "54", "57", "60",
            "63", "66", "69", "72", "75", "78", "81", "84", "87", "90",
            "93", "96", "99", "102", "105", "108", "111", "114", "117", "120",
        };

        #region individual scores
        private ObservableCollection<WhiteboardScore> _whiteboardScoresP1;
        public ObservableCollection<WhiteboardScore> whiteboardScoresP1
        {
            get => _whiteboardScoresP1;
            set => Set(ref _whiteboardScoresP1, value);
        }

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

        public bool UpdateFinishHelp(Player p, out string helpText)
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

        public void AddScore(Player p)
        {
            if (p.ThrowsLeft == 0 && p.pointsToOut > 0 || p.fa)
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
            else
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

        private void updatePlayerStatistics(string tag)
        {
            List<int> throws = allMatchScores.Where((s) => s.PlayerTag.Equals(tag)).Select(s => s.PointsThrown).ToList();
            double avg = throws.Average();

            if (tag.Equals("Player1")) P1Avg.Val = avg;
            else if (tag.Equals("Player2")) P2Avg.Val = avg;
        }
    }
}
