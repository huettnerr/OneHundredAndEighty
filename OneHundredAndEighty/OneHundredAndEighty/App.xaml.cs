using OneHundredAndEighty.OBS;
using System;
using System.Windows;

namespace OneHundredAndEighty
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Game Game;

        public App()
        {
            ObsManager.Init();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Perform tasks at application exit
            ObsManager.Close();
        }
    }
}
