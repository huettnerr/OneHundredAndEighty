using MyToolkit.Mvvm;
using OneHundredAndEighty.Classes;
using OneHundredAndEighty.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace OneHundredAndEighty.Score
{
    public enum Players { Player1,Player2 }

    public class ScoreViewModel : ViewModelBase
    {
        public ViewProperty<bool> IsSetMode { get; set; }
        public ViewProperty<Players> BeginningPlayer { get; set; }
        public ViewProperty<Players> ActivePlayer { get; set; }


        public ViewProperty<string> P1Name { get; set; }
        public ViewProperty<int> P1Sets { get; set; }
        public ViewProperty<int> P1Legs { get; set; }
        public ViewProperty<int> P1Points { get; set; }
        public ViewProperty<string> P1Help { get; set; }       
        
        
        public ViewProperty<string> P2Name { get; set; }
        public ViewProperty<int> P2Sets { get; set; }
        public ViewProperty<int> P2Legs { get; set; }
        public ViewProperty<int> P2Points { get; set; }
        public ViewProperty<string> P2Help { get; set; }

        public ScoreViewModel()
        {
            IsSetMode = new ViewProperty<bool>();
            BeginningPlayer = new ViewProperty<Players>();
            ActivePlayer = new ViewProperty<Players>();
            P1Name = new ViewProperty<string>();
            P1Sets = new ViewProperty<int>();
            P1Legs = new ViewProperty<int>();
            P1Points = new ViewProperty<int>();
            P1Help = new ViewProperty<string>();
            P2Name = new ViewProperty<string>();
            P2Sets = new ViewProperty<int>();
            P2Legs = new ViewProperty<int>();
            P2Points = new ViewProperty<int>();
            P2Help = new ViewProperty<string>();
        }
    }
}
