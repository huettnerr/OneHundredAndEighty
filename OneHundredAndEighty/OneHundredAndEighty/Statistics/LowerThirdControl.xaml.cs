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
    public partial class LowerThirdControl : UserControl
    {
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(LowerThirdControl), new UIPropertyMetadata(""));
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(LowerThirdControl), new UIPropertyMetadata(""));
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty SubLabelProperty =
            DependencyProperty.Register("SubLabel", typeof(string), typeof(LowerThirdControl), new UIPropertyMetadata(""));
        public string SubLabel
        {
            get { return (string)GetValue(SubLabelProperty); }
            set { SetValue(SubLabelProperty, value); }
        }

        private TimeSpan FadeDuration = TimeSpan.FromSeconds(1.0);
        private TimeSpan HoldTime = TimeSpan.FromSeconds(5.0);

        public LowerThirdControl()
        {
            InitializeComponent();
        }

        public void Show(string label, string message, string sublabel = "")
        {
            Label = label;
            Message = message;
            SubLabel = sublabel;

            this.Visibility = Visibility.Visible;
            var fadeInAnimation = new DoubleAnimation(0, 1, FadeDuration);
            fadeInAnimation.Completed += ShowFinished;

            this.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation);
        }

        private void ShowFinished(object sender, EventArgs e)
        {
            var fadeOutAnimatin = new DoubleAnimation(1, 0, FadeDuration);
            fadeOutAnimatin.BeginTime = HoldTime;
            fadeOutAnimatin.Completed += (s2, e2) => this.Visibility = Visibility.Hidden;

            this.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimatin);
        }
    }
}
