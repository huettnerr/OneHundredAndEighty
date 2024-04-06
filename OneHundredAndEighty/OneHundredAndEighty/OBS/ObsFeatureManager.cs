using OneHundredAndEighty.Score;
using OneHundredAndEighty.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneHundredAndEighty.OBS
{
    public static class ObsFeatureManager
    {
        //public static bool WasEventOnThrow { get; set; } = false;

        public static void NewThrowEventHandler(object sender, ThrowEvent t)
        {
            switch (t.Type)
            {
                case ThrowEventType._20for26:
                case ThrowEventType._5for26:
                case ThrowEventType._1for26:
                    //WasEventOnThrow = true;
                    ObsManager.ChangeBoardView("26");
                    break;
                case ThrowEventType._T60for180:
                    //WasEventOnThrow = true;
                    ObsManager.ChangeBoardView("T20");
                    break;
                case ThrowEventType.Double:
                    //WasEventOnThrow = true;
                    ObsManager.ChangeBoardView(t.Finish.ObsScene);
                    break;
                case ThrowEventType.None:
                    ObsManager.NormalBoardView();
                    break;
            }
        }
    }
}
