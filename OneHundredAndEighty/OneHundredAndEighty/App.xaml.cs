using OneHundredAndEighty.OBS;
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
    }
}
