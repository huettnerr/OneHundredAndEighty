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
    #region Finishes
    public static class FinishHelper
    {
        public struct Finish
        {
            public string HelpText;
            public int MinThrows;
            public string ObsScene;

            public Finish(string helpText, int minThrows, string obsScene = "")
            {
                this.HelpText = helpText;
                this.MinThrows = minThrows;
                this.ObsScene = obsScene;
            }
        }

        private static readonly SortedList<int, Finish> checkoutTableThreeThrows = new SortedList<int, Finish>()
        {
            [2] = new Finish("D1", 1, "D1/5/20"),
            [4] = new Finish("D2", 1, "D2/15/17"),
            [6] = new Finish("D3", 1, "D3/7/19"),
            [8] = new Finish("D4", 1, "D4/18"),
            [10] = new Finish("D5", 1, "D1/5/20"),
            [12] = new Finish("D6", 1, "D6/10/13"),
            [14] = new Finish("D7", 1, "D3/7/19"),
            [16] = new Finish("D8", 1, "D8/16"),
            [18] = new Finish("D9", 1, "D9/12"),
            [20] = new Finish("D10", 1, "D6/10/13"),
            [22] = new Finish("D11", 1, "D11/14"),
            [24] = new Finish("D12", 1, "D9/12"),
            [26] = new Finish("D13", 1, "D6/10/13"),
            [28] = new Finish("D14", 1, "D11/14"),
            [30] = new Finish("D15", 1, "D2/15/17"),
            [32] = new Finish("D16", 1, "D8/16"),
            [34] = new Finish("D17", 1, "D2/15/17"),
            [36] = new Finish("D18", 1, "D4/18"),
            [38] = new Finish("D19", 1, "D3/7/19"),
            [40] = new Finish("D20", 1, "D1/5/20"),
            [3] = new Finish("1 D1", 2),
            [5] = new Finish("1 D2", 2),
            [7] = new Finish("3 D2", 2),
            [9] = new Finish("1 D4", 2),
            [11] = new Finish("3 D4", 2),
            [13] = new Finish("5 D4", 2),
            [15] = new Finish("7 D4", 2),
            [17] = new Finish("9 D4", 2),
            [19] = new Finish("3 D8", 2),
            [21] = new Finish("5 D8", 2),
            [23] = new Finish("7 D8", 2),
            [25] = new Finish("1 D12", 2),
            [27] = new Finish("3 D12", 2),
            [29] = new Finish("5 D12", 2),
            [31] = new Finish("7 D12", 2),
            [33] = new Finish("1 D16", 2),
            [35] = new Finish("3 D16", 2),
            [37] = new Finish("5 D16", 2),
            [39] = new Finish("7 D6", 2),
            [41] = new Finish("9 D16", 2),
            [42] = new Finish("10 D16", 2),
            [43] = new Finish("3 D20", 2),
            [44] = new Finish("4 D20", 2),
            [45] = new Finish("13 D16", 2),
            [46] = new Finish("6 D20", 2),
            [47] = new Finish("7 D20", 2),
            [48] = new Finish("16 D16", 2),
            [49] = new Finish("17 D16", 2),
            [50] = new Finish("18 D16", 2),
            [51] = new Finish("19 D16", 2),
            [52] = new Finish("20 D16", 2),
            [53] = new Finish("13 D20", 2),
            [54] = new Finish("14 D20", 2),
            [55] = new Finish("15 D20", 2),
            [56] = new Finish("16 D20", 2),
            [57] = new Finish("17 D20", 2),
            [58] = new Finish("18 D20", 2),
            [59] = new Finish("19 D20", 2),
            [60] = new Finish("20 D20", 2),
            [61] = new Finish("T15 D8", 2),
            [62] = new Finish("T10 D16", 2),
            [63] = new Finish("T13 D12", 2),
            [64] = new Finish("T16 D8", 2),
            [65] = new Finish("T19 D4", 2),
            [66] = new Finish("T14 D12", 2),
            [67] = new Finish("T17 D8", 2),
            [68] = new Finish("T20 D4", 2),
            [69] = new Finish("T19 D6", 2),
            [70] = new Finish("T18 D8", 2),
            [71] = new Finish("T13 16", 2),
            [72] = new Finish("T16 D12", 2),
            [73] = new Finish("T19 D8", 2),
            [74] = new Finish("T14 D16", 2),
            [75] = new Finish("T17 D12", 2),
            [76] = new Finish("T20 D8", 2),
            [77] = new Finish("T19 D10", 2),
            [78] = new Finish("T18 D12", 2),
            [79] = new Finish("T19 D11", 2),
            [80] = new Finish("T20 D10", 2),
            [81] = new Finish("T19 D12", 2),
            [82] = new Finish("Bull D16", 2),
            [83] = new Finish("T17 D16", 2),
            [84] = new Finish("T20 D12", 2),
            [85] = new Finish("T15 D20", 2),
            [86] = new Finish("T18 D18", 2),
            [87] = new Finish("T17 D18", 2),
            [88] = new Finish("T20 D14", 2),
            [89] = new Finish("T19 D16", 2),
            [90] = new Finish("T20 D15", 2),
            [91] = new Finish("T17 D20", 2),
            [92] = new Finish("T20 D16", 2),
            [93] = new Finish("T19 D18", 2),
            [94] = new Finish("T18 D20", 2),
            [95] = new Finish("T19 D19", 2),
            [96] = new Finish("T20 D18", 2),
            [97] = new Finish("T19 D20", 2),
            [98] = new Finish("T20 D19", 2),
            [100] = new Finish("T20 D20", 2),
            [99] = new Finish("T19 10 D16", 3),
            [101] = new Finish("T20 9 D16", 3),
            [102] = new Finish("T16 14 D20", 3),
            [103] = new Finish("T19 6 D20", 3),
            [104] = new Finish("T16 16 D20", 3),
            [105] = new Finish("T20 13 D16", 3),
            [106] = new Finish("T20 6 D20", 3),
            [107] = new Finish("T19 10 D20", 3),
            [108] = new Finish("T20 16 D16", 3),
            [109] = new Finish("T20 17 D16", 3),
            [110] = new Finish("T20 10 D20", 3),
            [111] = new Finish("T19 14 D20", 3),
            [112] = new Finish("T20 20 D16", 3),
            [113] = new Finish("T19 16 D20", 3),
            [114] = new Finish("T20 14 D20", 3),
            [115] = new Finish("T20 15 D20", 3),
            [116] = new Finish("T20 16 D20", 3),
            [117] = new Finish("T20 17 D20", 3),
            [118] = new Finish("T20 18 D20", 3),
            [119] = new Finish("T19 12 Bull", 3),
            [120] = new Finish("T20 20 D20", 3),
            [121] = new Finish("T20 11 Bull", 3),
            [122] = new Finish("T18 18 Bull", 3),
            [123] = new Finish("T19 16 Bull", 3),
            [124] = new Finish("T20 14 Bull", 3),
            [125] = new Finish("25 T20 D20", 3),
            [126] = new Finish("T19 19 Bull", 3),
            [127] = new Finish("T20 17 Bull", 3),
            [128] = new Finish("18 T20 Bull", 3),
            [129] = new Finish("19 T20 Bull", 3),
            [130] = new Finish("T20 20 Bull", 3),
            [131] = new Finish("T20 T13 D16", 3),
            [132] = new Finish("25 T19 Bull", 3),
            [133] = new Finish("T20 T19 D8", 3),
            [134] = new Finish("T20 T14 D16", 3),
            [135] = new Finish("25 T20 Bull", 3),
            [136] = new Finish("T20 T20 D8", 3),
            [137] = new Finish("T20 T19 D10", 3),
            [138] = new Finish("T20 T18 D12", 3),
            [139] = new Finish("T19 T14 D20", 3),
            [140] = new Finish("T20 T20 D10", 3),
            [141] = new Finish("T20 T19 D12", 3),
            [142] = new Finish("T20 T14 D20", 3),
            [143] = new Finish("T20 T17 D16", 3),
            [144] = new Finish("T20 T20 D12", 3),
            [145] = new Finish("T20 T15 D20", 3),
            [146] = new Finish("T20 T18 D16", 3),
            [147] = new Finish("T20 T17 D18", 3),
            [148] = new Finish("T20 T20 D14", 3),
            [149] = new Finish("T20 T19 D16", 3),
            [150] = new Finish("T20 T18 D18", 3),
            [151] = new Finish("T20 T17 D20", 3),
            [152] = new Finish("T20 T20 D16", 3),
            [153] = new Finish("T20 T19 D18", 3),
            [154] = new Finish("T20 T18 D20", 3),
            [155] = new Finish("T20 T19 D19", 3),
            [156] = new Finish("T20 T20 D18", 3),
            [157] = new Finish("T20 T19 D20", 3),
            [158] = new Finish("T20 T20 D19", 3),
            [160] = new Finish("T20 T20 D20", 3),
            [161] = new Finish("T20 T17 Bull", 3),
            [164] = new Finish("T20 T18 Bull", 3),
            [167] = new Finish("T20 T19 Bull", 3),
            [170] = new Finish("T20 T20 Bull", 3),
        }; //  Коллекция закрытия сета на один бросок
    }

    #endregion

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

        public void AddScore(Player p)
        {
            if(p.throw3 is object)
            {
                WhiteboardScore wbs = new WhiteboardScore(p);
                if (wbs.PlayerTag.Equals("Player1"))
                {
                    P1Points.Val = wbs.PointsToGo;
                    wbs.DartCount = 3 * whiteboardScoresP1.Count;
                    whiteboardScoresP1.Add(wbs);
                    RaisePropertyChanged(nameof(whiteboardScoresP1));
                }
                else if (wbs.PlayerTag.Equals("Player2"))
                {
                    P2Points.Val = wbs.PointsToGo;
                    wbs.DartCount = 3 * whiteboardScoresP2.Count;
                    whiteboardScoresP2.Add(wbs);
                }

                allMatchScores.Add(wbs);
                updatePlayerStatistics(p.Tag);
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

            allMatchScores.Remove(allMatchScores.Last());
        }

        public void ClearScores(int p)
        {
            whiteboardScoresP1.Clear();
            whiteboardScoresP1.Add(new WhiteboardScore(p));

            whiteboardScoresP2.Clear();
            whiteboardScoresP2.Add(new WhiteboardScore(p));

            P1Points.Val = p;
            P2Points.Val = p;
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
