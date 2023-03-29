using OneHundredAndEighty.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty.Statistics
{
    public static class StatisticsOverlayManager
    {
        public static ScoreWindow scoreWindow;

        private static Random rnd = new Random();
        private static readonly List<string> _26erNames = new List<string>() { "Hassan:", "Betten mit Frühstück:", "Wäsche gewaschen:", "Mit Wasser gekocht:", "26er:", "Weltklasse bewiesen:", "20 + 5 + 1 = ???" };
       
        public static void NewThrowEventHandler(object sender, ThrowEvent t)
        {
            if(scoreWindow.LowerThird.Visibility == System.Windows.Visibility.Visible 
                || scoreWindow.StatsCounter.Visibility == System.Windows.Visibility.Visible)
            {
                Console.WriteLine("Cant show stat");
                return;
            }

            switch (t.Type)
            {
                case ThrowEventType._3:
                    scoreWindow?.LowerThird.Show("", $"{t.Who} hat seine Pfeile gezählt", "(alle da)");
                    break;
                case ThrowEventType._26Waschmaschine:
                    scoreWindow?.StatsCounter.Show(_26erNames.ElementAt(rnd.Next(_26erNames.Count)), t.OldCount, t.NewCount, t.Who);
                    break;
                case ThrowEventType._26:
                    scoreWindow?.StatsCounter.Show($"26er:", t.OldCount, t.NewCount, t.Who);
                    break;
                case ThrowEventType._100:
                    if ((t.NewCount % 3) == 0) scoreWindow?.StatsCounter.Show($"100er:", t.OldCount, t.NewCount, t.Who);
                    break;
                case ThrowEventType._140:
                    if ((t.NewCount % 2) == 0) scoreWindow?.StatsCounter.Show($"140er:", t.OldCount, t.NewCount, t.Who);
                    break;
                case ThrowEventType._180:
                    scoreWindow?.StatsCounter.Show($"180!!!", t.OldCount, t.NewCount, t.Who);
                    break;

            }
        }
    }
}
