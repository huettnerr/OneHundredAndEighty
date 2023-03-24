using OneHundredAndEighty.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty.Score
{
    public class ScoreStatistics
    {
        public ViewProperty<double> Avg { get; set; }

        public ScoreStatistics()
        {
            Avg = new ViewProperty<double>();
        }

        public void UpdatePlayerStatistics(Stack<Throw> throws)
        {
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
                    points.AddRange(turnPoints.Select(t => 0));
                }
                else
                {
                    points.AddRange(turnPoints);
                }

                turnPoints.Clear();
            }

            Avg.Val = 3 * points.Average();
        }
    }
}
