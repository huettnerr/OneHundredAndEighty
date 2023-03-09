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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneHundredAndEighty.Score
{
    /// <summary>
    /// Interaktionslogik für ScoreControl.xaml
    /// </summary>
    public partial class ScoreControl : UserControl
    {
        private readonly TimeSpan throwSlideTime = TimeSpan.FromSeconds(0.15); //  Время анимации слайдера броска
        private readonly TimeSpan helpSlideTime = TimeSpan.FromSeconds(0.23); //  Время анимации слайда помощи
        private readonly TimeSpan helpFadeTime = TimeSpan.FromSeconds(0.23); //  Время анимации фейда помощи
        private readonly TimeSpan dotFadeTime = TimeSpan.FromSeconds(0.23); //  Время анимации фейда помощи

        public ScoreControl()
        {
            InitializeComponent();
            this.DataContext = ((App)Application.Current).Game.scoreVM;
        }

        public void DotSet(Player p)
        {
            if (p.Tag == "Player1" && Player1SetDot.Tag.ToString() == "OFF" || p.Tag == "Player2" && Player2SetDot.Tag.ToString() == "OFF")
            {
                var storyboard = new Storyboard();
                var fadein = new DoubleAnimation(0, 1, dotFadeTime);
                var fadeout = new DoubleAnimation(1, 0, dotFadeTime);
                Storyboard.SetTargetProperty(fadein, new PropertyPath(UIElement.OpacityProperty));
                Storyboard.SetTargetProperty(fadeout, new PropertyPath(UIElement.OpacityProperty));
                if (p.Tag == "Player1")
                {
                    Storyboard.SetTarget(fadeout, Player2SetDot);
                    Storyboard.SetTarget(fadein, Player1SetDot);
                    Player1SetDot.Tag = "ON";
                    Player2SetDot.Tag = "OFF";
                }

                if (p.Tag == "Player2")
                {
                    Storyboard.SetTarget(fadeout, Player1SetDot);
                    Storyboard.SetTarget(fadein, Player2SetDot);
                    Player2SetDot.Tag = "ON";
                    Player1SetDot.Tag = "OFF";
                }

                storyboard.Children.Add(fadein);
                storyboard.Children.Add(fadeout);

                storyboard.Begin();
            }
        } //  Установка точки начала сета

        public void WhoThrowSliderSet(Player p)
        {
            var slider = new Storyboard();

            var hide = new DoubleAnimation() { From = -WhoThrowSlider.ActualWidth, To = 0, Duration = throwSlideTime };
            Storyboard.SetTarget(hide, WhoThrowSlider);
            Storyboard.SetTargetProperty(hide, new PropertyPath(Canvas.RightProperty));

            var show = new DoubleAnimation() { From = 0, To = -WhoThrowSlider.ActualWidth, Duration = throwSlideTime, BeginTime = throwSlideTime };
            Storyboard.SetTarget(show, WhoThrowSlider);
            Storyboard.SetTargetProperty(show, new PropertyPath(Canvas.RightProperty));

            var fadeout = new DoubleAnimation(1, 0, helpFadeTime);
            Storyboard.SetTargetProperty(fadeout, new PropertyPath(UIElement.OpacityProperty));

            var fadein = new DoubleAnimation(0, 1, helpFadeTime);
            Storyboard.SetTargetProperty(fadein, new PropertyPath(UIElement.OpacityProperty));

            DoubleAnimation toggle;
            if (p.Tag == "Player2")
            {
                Storyboard.SetTarget(fadeout, Player1HelpBackground);
                Storyboard.SetTarget(fadein, Player2HelpBackground);

                toggle = new DoubleAnimation() { From = 0, To = WhoThrowSlider.ActualHeight, Duration = TimeSpan.FromSeconds(0), BeginTime = throwSlideTime };
                WhoThrowSlider.Tag = "Player1";
            }
            else
            {
                Storyboard.SetTarget(fadein, Player1HelpBackground);
                Storyboard.SetTarget(fadeout, Player2HelpBackground);

                toggle = new DoubleAnimation() { From = WhoThrowSlider.ActualHeight, To = 0, Duration = TimeSpan.FromSeconds(0), BeginTime = throwSlideTime };
                WhoThrowSlider.Tag = "Player2";
            }

            Storyboard.SetTarget(toggle, WhoThrowSlider);
            Storyboard.SetTargetProperty(toggle, new PropertyPath(Canvas.TopProperty));

            slider.Children.Add(fadeout);
            slider.Children.Add(hide);
            slider.Children.Add(toggle);
            slider.Children.Add(show);
            slider.Children.Add(fadein);
            slider.Begin();
        } //  Установка слайдера текущего броска


        public void HelpShow(Player p, string help)
        {
            UpdateLayout();

            Grid helpPanel = null;
            if (p.Tag.Equals("Player1")) helpPanel = Player1Help;
            else if (p.Tag.Equals("Player2")) helpPanel = Player2Help;

            if (helpPanel.Tag.Equals("OFF"))
            {
                Canvas.SetRight(helpPanel, 0);

                //var show = new DoubleAnimation(-helpPanel.ActualWidth, 0, helpSlideTime);
                //helpPanel.BeginAnimation(Canvas.RightProperty, show);

                helpPanel.Tag = "ON";
            }
        }

        public void HelpHide(Player p)
        {
            UpdateLayout();

            Grid helpPanel = null;
            if (p.Tag.Equals("Player1")) helpPanel = Player1Help;
            else if (p.Tag.Equals("Player2")) helpPanel = Player2Help;

            if (helpPanel?.Tag.Equals("ON") ?? false)
            {
                Canvas.SetRight(helpPanel, -helpPanel.ActualWidth);

                //var hide = new DoubleAnimation(0, -helpPanel.ActualWidth, helpSlideTime);
                //helpPanel.BeginAnimation(Canvas.RightProperty, hide);

                helpPanel.Tag = "OFF";
            }
        } 
    }
}
