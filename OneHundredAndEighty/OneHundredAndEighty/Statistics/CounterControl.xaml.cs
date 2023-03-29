using OneHundredAndEighty.Controls;
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

namespace OneHundredAndEighty.Statistics
{
    /// <summary>
    /// Interaktionslogik für CounterControl.xaml
    /// </summary>
    public partial class CounterControl : UserControl
    {
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(CounterControl), new UIPropertyMetadata(""));
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty FrontSideProperty =
            DependencyProperty.Register("FrontSide", typeof(int), typeof(CounterControl), new UIPropertyMetadata(0));
        public int FrontSide
        {
            get { return (int)GetValue(FrontSideProperty); }
            set { SetValue(FrontSideProperty, value); }
        }

        public static readonly DependencyProperty BackSideProperty =
            DependencyProperty.Register("BackSide", typeof(int), typeof(CounterControl), new UIPropertyMetadata(0));
        public int BackSide
        {
            get { return (int)GetValue(BackSideProperty); }
            set { SetValue(BackSideProperty, value); }
        }

        private TimeSpan FadeDuration = TimeSpan.FromSeconds(1.0);
        private TimeSpan HoldTime = TimeSpan.FromSeconds(1.0);

        public CounterControl()
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
        }

        public void Show(string label, int from, int to)
        {
            Label = label;
            FrontSide = from;
            BackSide = to;
            FlipElement.ResetFlip();

            this.Visibility = Visibility.Visible;
            var fadeInAnimation = new DoubleAnimation(0, 1, FadeDuration);
            fadeInAnimation.Completed += (s, e) => flip();

            this.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
        }

        private void flip()
        {
            FlipElement.Flipped += FlipElement_Flipped;
            FlipElement.FlipY(HoldTime);
        }



        private void FlipElement_Flipped(object sender, EventArgs e)
        {
            FlipElement.Flipped -= FlipElement_Flipped;
            var fadeOutAnimatin = new DoubleAnimation(1, 0, FadeDuration);
            fadeOutAnimatin.BeginTime = HoldTime;
            fadeOutAnimatin.Completed += (s2, e2) => this.Visibility = Visibility.Hidden;

            this.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimatin);
        }
    }
}
