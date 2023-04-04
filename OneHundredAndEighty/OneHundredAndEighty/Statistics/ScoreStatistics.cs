using OneHundredAndEighty.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty.Statistics
{
    public class ScoreStatistics
    {
        public ViewProperty<double> Avg { get; set; }
        public ViewProperty<int> _26 { get; set; }
        public ViewProperty<int> _100Plus { get; set; }
        public ViewProperty<int> _140Plus { get; set; }
        public ViewProperty<int> _180 { get; set; }

        public ScoreStatistics()
        {
            Avg = new ViewProperty<double>();
            _26 = new ViewProperty<int>();
            _100Plus = new ViewProperty<int>();
            _140Plus = new ViewProperty<int>();
            _180 = new ViewProperty<int>();
        }

        public void Clear()
        {
            Avg.Val = 0;
            _26.Val = 0;
            _100Plus.Val = 0;
            _140Plus.Val = 0;
            _180.Val = 0;
        }

        public void UpdatePlayerStatistics(Stack<Throw> throws)
        {
            Clear();
            if (throws.Count == 0) return;

            List<int> points = new List<int>();
            List<int> turnPoints = new List<int>();

            for (int i = throws.Count - 1; i >= 0; i--)
            {
                turnPoints.Add(throws.ElementAt(i).Points ?? 0);

                while (!Game.IsTurnThrow(throws.ElementAt(i)))
                {
                    if (i == 0) break;

                    //if we have more elements, add them
                    i--;
                    turnPoints.Add(throws.ElementAt(i).Points ?? 0);

                }

                //We have a Turn
                //Check if it was faulty and clear points if neccessary
                if (throws.ElementAt(i).IsFault)
                {
                    points.AddRange(Enumerable.Repeat(0, 3));
                }
                else
                {
                    points.AddRange(turnPoints);
                    int score = turnPoints.Sum();

                    if (score == 26) _26.Val++; 
                    if (score >= 100 && score < 140) _100Plus.Val++; 
                    if (score >= 140 && score < 180) _140Plus.Val++; 
                    if (score == 180) _180.Val++; 
                }

                turnPoints.Clear();
            }

            Avg.Val = 3 * points.Average();
        }
    }
}
