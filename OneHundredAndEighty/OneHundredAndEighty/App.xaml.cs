using OneHundredAndEighty.OBS;
using OneHundredAndEighty.Score;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace OneHundredAndEighty
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Game Game;
        public ScoreViewModel ScoreVM { get; private set; }

        public App()
        {
            ObsManager.Init();

            ScoreVM = new ScoreViewModel();

            this.Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            // Perform tasks at application exit
            ObsManager.Close();
        }
    }
}
