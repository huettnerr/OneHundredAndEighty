using MyToolkit.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            if (t.HandNumber == 3 || t.IsFault || t.IsLegWon || t.IsSetWon || t.IsMatchWon)
            {
                WhiteboardScore wbs = new WhiteboardScore(t.IsFault ? 0 : p.handPoints, p.pointsToOut, 3 * WhiteboardScores.Count);
                WhiteboardScores.Add(wbs);
                callback?.Invoke(this, new EventArgs());
            }

            updatePlayerStatistics();
        }

        public void UndoThrow()
        {

        }

        public void ClearWhiteboard(int pointsToGo)
        {
            oldWhiteboards.Push(new ObservableCollection<WhiteboardScore>(WhiteboardScores));

            WhiteboardScores.Clear();
            WhiteboardScores.Add(new WhiteboardScore(0, pointsToGo, 0));
        }

        private void updatePlayerStatistics()
        {
            Stats.Avg.Val = (throws.Select(t => t.Points).Average() * 3) ?? 0;
        }
    }
}
