using OneHundredAndEighty.Score;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneHundredAndEighty.Controls
{
    /// <summary>
    /// Interaktionslogik für InfoControl.xaml
    /// </summary>
    public partial class InfoControl : UserControl
    {
        public InfoControl()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                this.DataContext = ((App)Application.Current).Game;
                ((App)Application.Current).ScoreVM.ScoresChanged += LastScoreChangedEventHandler;
            };
        }

        private void LastScoreChangedEventHandler(object sender, WhiteboardScore? ws)
        {
            LastTurnPointsLabel.Dispatcher.BeginInvoke(new Action(() =>
            {
                if(!ws?.IsGameShot ?? true)
                {
                    LastTurnPointsLabel.Content = ws?.PointsThrown.ToString() ?? "Score";
                }
                else
                {
                    LastTurnPointsLabel.Content = "Game Shot!";

                }

            }));
        }

        private void EndMatchButton_Click(object sender, RoutedEventArgs e) //  Кнопка отмены матча
        {
            ((App)Application.Current).Game.AbortGameRequest();
        }

        private void UndoThrow_Click(object sender, RoutedEventArgs e) //  Кнопка отмены броска
        {
            ((App)Application.Current).Game.UndoThrow();
        }
    }
}
