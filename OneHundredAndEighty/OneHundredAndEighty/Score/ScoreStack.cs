using MyToolkit.Model;
using OneHundredAndEighty.Statistics;
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

        public ScoreStack()
        {
            throws = new Stack<Throw>();
            oldWhiteboards = new Stack<ObservableCollection<WhiteboardScore>>();
            WhiteboardScores = new ObservableCollection<WhiteboardScore>();
            Stats = new ScoreStatistics();
        }

        public void Init(Player p)
        {
            PlayerTag = p?.Tag ?? "";
            Reset();
        }

        public void Reset() 
        {
            throws.Clear();
            WhiteboardScores.Clear();
            oldWhiteboards.Clear();
            Stats.Clear();
        }

        public void AddThrow(Throw t, Player p, EventHandler<WhiteboardScore?> callback)
        {
            throws.Push(t);
            Stats.UpdatePlayerStatistics(throws);

            if (Game.IsTurnThrow(t))
            {
                List<Throw> turnThrows;
                if (!GetThrowsOfRecentTurn(out turnThrows))
                {
                    Console.WriteLine("Error querying turn throws");
                    turnThrows = new List<Throw>();
                }
                WhiteboardScore wbs;
                if (t.IsFault)
                {
                    wbs = new WhiteboardScore(p, 0, p.pointsToOut, 3 * WhiteboardScores.Count, turnThrows, false);
                }
                else
                {
                    wbs = new WhiteboardScore(p, p.handPoints, p.pointsToOut, 3 * WhiteboardScores.Count - p.ThrowsLeft, turnThrows, t.IsLegWon);

                    //Stats
                    if(!t.IsLegWon) ThrowEvent.FireTurnEvents(this, turnThrows, p.Name, Stats);
                }

                WhiteboardScores.Add(wbs);
                callback?.Invoke(this, wbs);
            }
        }

        //Returns true if an old whiteboard was restored
        public bool UndoThrow(Player p, EventHandler<WhiteboardScore?> callback)
        {
            Throw t = throws.Pop();
            Stats.UpdatePlayerStatistics(throws);

            //If the undone throw was a turn throw remove the whiteboard panel and notify player switch (by returning true)
            if (Game.IsTurnThrow(t))
            {
                if(WhiteboardScores.Count > 1) 
                {
                    WhiteboardScores.Remove(WhiteboardScores.Last());
                    callback?.Invoke(this, WhiteboardScores.Last());
                }
                else if(oldWhiteboards.Count > 0)
                {
                    RestoreWhiteboard(null);
                    WhiteboardScores.Remove(WhiteboardScores.Last());
                    callback?.Invoke(this, WhiteboardScores.Last());
                    return true;
                }
            }
            return false;
        }

        public void RestoreWhiteboard(EventHandler<WhiteboardScore?> callback)
        {
            WhiteboardScores.Clear();
            var prevWB = oldWhiteboards.Pop();

            foreach(WhiteboardScore wbs in prevWB)
            {
                WhiteboardScores.Add(wbs);
            }
            callback?.Invoke(this, null);
        }

        public void ClearWhiteboard(Player p, int pointsToGo)
        {
            oldWhiteboards.Push(new ObservableCollection<WhiteboardScore>(WhiteboardScores));

            WhiteboardScores.Clear();
            WhiteboardScores.Add(new WhiteboardScore(p, 0, pointsToGo, 0, new List<Throw>(), false));
        }

        public bool GetThrowsOfRecentTurn(out List<Throw> result)
        {
            result = new List<Throw>();
            //if (!Game.IsTurnThrow(throws.Peek())) return false; //The current throw must be a turn throw

            //result.Add(throws.ElementAt(0)); //Add the most recent throw
            int i = 0;
            while(true)
            {
                if (i == throws.Count) break; //turn was first turn of match

                if(Game.IsTurnThrow(throws.ElementAt(i))) 
                {
                    break;
                }
                else
                {
                    result.Add(throws.ElementAt(i));
                    i++;
                }
            }

            result.Reverse();
            return true;
        }
    }
}
