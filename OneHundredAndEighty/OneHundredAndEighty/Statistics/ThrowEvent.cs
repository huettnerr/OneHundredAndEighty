using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty.Statistics
{
    public enum ThrowEventType
    {
        _3,
        _26,
        _26Waschmaschine,
        _100,
        _140,
        _180
    }

    public class ThrowEvent
    {
        public ThrowEventType Type;
        public string Who;
        public int OldCount;
        public int NewCount;
        //public double Value;

        public ThrowEvent(ThrowEventType type, string who, int oldCount, int newCount/*, double value*/)
        {
            Type = type;
            Who = who;
            OldCount = oldCount;
            NewCount = newCount;
            //Value = value;
        }

        public static void FireEvents(object sender, EventHandler<ThrowEvent> Handler, List<Throw> throws, string name, ScoreStatistics s)
        {
            int score = throws.Select(t => t.Points).ToList().Sum() ?? 0;

            if (score == 3) Handler?.Invoke(sender, new ThrowEvent(ThrowEventType._3, name, 0,0));
            if (score == 26)
            {
                if(throws.Where(t => t.Points == 1).Count() == 1 && throws.Where(t => t.Points == 5).Count() == 1 && throws.Where(t => t.Points == 20).Count() == 1)
                {
                    Handler?.Invoke(sender, new ThrowEvent(ThrowEventType._26Waschmaschine, name, s._26.Val - 1, s._26.Val));
                }
                else
                {
                    Handler?.Invoke(sender, new ThrowEvent(ThrowEventType._26, name, s._26.Val - 1, s._26.Val));
                }
            }
            if (score >= 100 && score < 140) Handler?.Invoke(sender, new ThrowEvent(ThrowEventType._100, name, s._100Plus.Val - 1, s._100Plus.Val));
            if (score >= 140 && score < 180) Handler?.Invoke(sender, new ThrowEvent(ThrowEventType._140, name, s._140Plus.Val - 1, s._140Plus.Val));
            if (score == 180) Handler?.Invoke(sender, new ThrowEvent(ThrowEventType._180, name, s._180.Val - 1, s._180.Val));
        }
    }
}
