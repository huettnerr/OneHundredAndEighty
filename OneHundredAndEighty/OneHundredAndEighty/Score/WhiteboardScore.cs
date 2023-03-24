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
        public List<Throw> Throws { get; set; }
        public string ThrowsString
        {
            get => String.Join("|", Throws);
        }

        public WhiteboardScore(int pointsThrown, int pointsToGo, int dartCount, List<Throw> throws)
        {
            PointsThrown = pointsThrown;
            PointsToGo = pointsToGo;
            LegDartCount = dartCount;
            Throws = throws;
        }
    }
}
