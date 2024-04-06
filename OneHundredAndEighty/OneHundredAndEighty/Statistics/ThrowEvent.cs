using OneHundredAndEighty.OBS;
using OneHundredAndEighty.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty.Statistics
{
    public enum ThrowEventType
    {
        None,

        //Statistics
        _3,
        _26,
        _26Waschmaschine,
        _100,
        _140,
        _180,

        //Zooms
        _20for26,
        _5for26,
        _1for26,
        _T60for180,

        //Doubles
        Double
    }

    public class ThrowEvent
    {
        public static EventHandler<ThrowEvent> ThrowEvents;

        public ThrowEventType Type;
        public string Who;
        public int OldCount;
        public int NewCount;
        public Finish Finish;

        public ThrowEvent(ThrowEventType type, string who, int oldCount, int newCount, Finish finish)
        {
            Type = type;
            Who = who;
            OldCount = oldCount;
            NewCount = newCount;
            Finish = finish;
        }

        public static void FireTurnEvents(object sender, List<Throw> throws, string name, ScoreStatistics s)
        {
            int score = throws.Select(t => t.Points).ToList().Sum() ?? 0;

            if (score == 3)
            {
                if (throws.Where(t => t.Points == 1).Count() == 3) ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType._3, name, 0, 0, null));
            }
            if (score == 26)
            {
                if (throws.Where(t => t.Points == 1).Count() == 1 && throws.Where(t => t.Points == 5).Count() == 1 && throws.Where(t => t.Points == 20).Count() == 1)
                {
                    ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType._26Waschmaschine, name, s._26.Val - 1, s._26.Val, null));
                }
                else
                {
                    ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType._26, name, s._26.Val - 1, s._26.Val, null));
                }
            }
            if (score >= 100 && score < 140) ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType._100, name, s._100Plus.Val - 1, s._100Plus.Val, null));
            if (score >= 140 && score < 180) ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType._140, name, s._140Plus.Val - 1, s._140Plus.Val, null));
            if (score == 180) ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType._180, name, s._180.Val - 1, s._180.Val, null));
        }

        public static void FireLastDartEvents(object sender, List<Throw> throws, Player p)
        {
            // 26 close?
            if (throws.Where(t => t.Points == 1).Count() == 1 && throws.Where(t => t.Points == 5).Count() == 1)
            {
                //20 left
                ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType._20for26, p.Name, 0, 0, null));
            }
            else if (throws.Where(t => t.Points == 1).Count() == 1 && throws.Where(t => t.Points == 20).Count() == 1)
            {
                //5 left
                ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType._5for26, p.Name, 0, 0, null));
            }
            else if (throws.Where(t => t.Points == 5).Count() == 1 && throws.Where(t => t.Points == 20).Count() == 1)
            {
                //1 left
                ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType._1for26, p.Name, 0, 0, null));
            }

            // 180 possible?
            if (throws.Where(t => t.Points == 60 && t.Multiplier.Equals("Tremble")).Count() == 2 && p.pointsToOut > 61)
            {
                ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType._T60for180, p.Name, 0, 0, null));
            }
        }

        public static void FireDoubleEvents(object sender, string name, Finish f)
        {
            ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType.Double, name, 0, 0, f));
        }

        public static void CheckForEventsOnThrow(object sender, Player p, List<Throw> turnThrows)
        {
            //Check for Finish Events
            Finish f = FinishHelper.GetBestFinish(p.pointsToOut, p.ThrowsLeft);
            if (!f?.ObsScene.Equals(String.Empty) ?? false && p.ThrowsLeft > 0)
            {
                ThrowEvent.FireDoubleEvents(sender, p.Name, f);
            }             
            //Check for 2 dart events
            else if (turnThrows.Count == 2)
            {
                ThrowEvent.FireLastDartEvents(sender, turnThrows, p);
            }
            else
            {
                ThrowEvents?.Invoke(sender, new ThrowEvent(ThrowEventType.None, p.Name, 0, 0, f));
            }
        }
    }
}
