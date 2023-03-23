using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty.Score
{
    public struct WhiteboardScore
    {
        public int PointsThrown { get; set; }
        public int PointsToGo { get; set; }
        public int LegDartCount { get; set; }
        public int DartsForScore { get; set; }

        public WhiteboardScore(int pointsThrown, int pointsToGo, int dartCount, int dartsForScore)
        {
            PointsThrown = pointsThrown;
            PointsToGo = pointsToGo;
            LegDartCount = dartCount;
            DartsForScore = dartsForScore;
        }
    }
}
