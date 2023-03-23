using MyToolkit.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty.Score
{
    public class ScoreStack
    {
        private Stack<Throw> throws;
        private Stack<ObservableCollection<WhiteboardScore>> oldWhiteboards;

        public string PlayerTag { get; set; }
        public ObservableCollection<WhiteboardScore> WhiteboardScores { get; set; } 

        public ScoreStatistics Stats { get; private set; }

        public ScoreStack(Player p)
        {
            throws = new Stack<Throw>();
            oldWhiteboards = new Stack<ObservableCollection<WhiteboardScore>>();

            PlayerTag = p?.Tag ?? "";
            WhiteboardScores = new ObservableCollection<WhiteboardScore>();
            Stats = new ScoreStatistics();
        }

        public void AddThrow(Throw t, Player p, EventHandler callback)
        {
            throws.Push(t);
            updatePlayerStatistics();

            if (isTurnThrow(t))
            {
                WhiteboardScore wbs;
                if (t.IsFault)
                {
                    wbs = new WhiteboardScore(0, p.pointsToOut, 3 * WhiteboardScores.Count, 3);
                }
                else
                {
                    wbs = new WhiteboardScore(p.handPoints, p.pointsToOut, 3 * WhiteboardScores.Count - p.ThrowsLeft, p.ThrowCount);
                }

                WhiteboardScores.Add(wbs);
                callback?.Invoke(this, new EventArgs());
            }
        }

        //Returns true if an old whiteboard was restored
        public bool UndoThrow(EventHandler callback)
        {
            Throw t = throws.Pop();
            updatePlayerStatistics();

            if (isTurnThrow(t))
            {
                if(WhiteboardScores.Count > 1) 
                {
                    WhiteboardScores.Remove(WhiteboardScores.Last());
                    callback?.Invoke(this, new EventArgs());
                }
                else if(oldWhiteboards.Count > 0)
                {
                    RestoreWhiteboard(null);
                    WhiteboardScores.Remove(WhiteboardScores.Last());
                    callback?.Invoke(this, new EventArgs());
                    return true;
                }
            }
            return false;
        }

        public void RestoreWhiteboard(EventHandler callback)
        {
            WhiteboardScores.Clear();
            var prevWB = oldWhiteboards.Pop();

            foreach(WhiteboardScore wbs in prevWB)
            {
                WhiteboardScores.Add(wbs);
            }
            callback?.Invoke(this, new EventArgs());
        }

        public void ClearWhiteboard(int pointsToGo)
        {
            oldWhiteboards.Push(new ObservableCollection<WhiteboardScore>(WhiteboardScores));

            WhiteboardScores.Clear();
            WhiteboardScores.Add(new WhiteboardScore(0, pointsToGo, 0, 0));
        }

        private bool isTurnThrow(Throw t)
        {
            return (t.HandNumber == 3 || t.IsFault || t.IsLegWon);
        }

        private void updatePlayerStatistics()
        {
            List<int> points = new List<int>();
            List<int> turnPoints = new List<int>();

            for(int i = throws.Count - 1; i>=0; i--)
            {
                turnPoints.Add(throws.ElementAt(i).Points ?? 0);

                while (!isTurnThrow(throws.ElementAt(i)))
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

            Stats.Avg.Val = 3 * points.Average();
        }
    }
}
