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
    }
}
