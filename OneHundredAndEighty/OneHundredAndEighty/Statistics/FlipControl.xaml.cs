using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneHundredAndEighty.Statistics
{
    /// <summary>
    /// Interaktionslogik für CounterControl.xaml
    /// </summary>
    public partial class FlipControl : Grid, INotifyPropertyChanged
    {
        public static readonly DependencyProperty FrontProperty =
            DependencyProperty.Register("Front", typeof(FrameworkElement), typeof(FlipControl), new UIPropertyMetadata(null));
        public FrameworkElement Front
        {
            get { return (FrameworkElement)GetValue(FrontProperty); }
            set { SetValue(FrontProperty, value); }
        }
        public static readonly DependencyProperty BackProperty =
            DependencyProperty.Register("Back", typeof(FrameworkElement), typeof(FlipControl), new UIPropertyMetadata(null));
        public FrameworkElement Back
        {
            get { return (FrameworkElement)GetValue(BackProperty); }
            set { SetValue(BackProperty, value); }
        }
        public static readonly DependencyProperty FlipDurationProperty =
            DependencyProperty.Register("FlipDuration", typeof(Duration), typeof(FlipControl), new UIPropertyMetadata((Duration)TimeSpan.FromSeconds(0.5)));
        public Duration FlipDuration
        {
            get { return (Duration)GetValue(FlipDurationProperty); }
            set { SetValue(FlipDurationProperty, value); }
        }

        private bool _isFlipped = false;
        public bool IsFlipped
        {
            get { return _isFlipped; }
            private set
            {
                if (value != _isFlipped)
                {
                    _isFlipped = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsFlipped"));
                }
            }
        }

        private IEasingFunction EasingFunction = new SineEase() { EasingMode = EasingMode.EaseInOut };

        public FlipControl()
        {
            InitializeComponent();
        }

        //public void FlipX()
        //{
        //    var animation = new DoubleAnimation()
        //    {
        //        Duration = FlipDuration,
        //        EasingFunction = EasingFunction,
        //    };
        //    animation.To = IsFlipped ? 1 : -1;
        //    (FindResource("transform") as ScaleTransform).BeginAnimation(ScaleTransform.ScaleXProperty, animation);
        //    IsFlipped = !IsFlipped;
        //    OnFlipped(new EventArgs());
        //}

        public void FlipY(TimeSpan delay)
        {
            var animation = new DoubleAnimation()
            {
                Duration = FlipDuration,
                EasingFunction = EasingFunction,
                FillBehavior = FillBehavior.HoldEnd,
                BeginTime = delay,
                To = IsFlipped ? 1 : -1
            };
            animation.Completed += (s, e) => OnFlipped(new EventArgs());
            IsFlipped = !IsFlipped;

            (FindResource("transform") as ScaleTransform).BeginAnimation(ScaleTransform.ScaleYProperty, animation);
        }

        public void ResetFlip()
        {
            (FindResource("transform") as ScaleTransform).BeginAnimation(ScaleTransform.ScaleYProperty, null);
            IsFlipped = false;
        }

        public event EventHandler Flipped;

        protected virtual void OnFlipped(EventArgs e)
        {
            if (this.Flipped != null)
            {
                this.Flipped(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        public static Task BeginAsync(Storyboard storyboard)
        {
            System.Threading.Tasks.TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            if (storyboard == null)
                tcs.SetException(new ArgumentNullException());
            else
            {
                EventHandler onComplete = null;
                onComplete = (s, e) => {
                    storyboard.Completed -= onComplete;
                    tcs.SetResult(true);
                };
                storyboard.Completed += onComplete;
                storyboard.Begin();
            }
            return tcs.Task;
        }
    }

    public class LessThanXToTrueConverter : IValueConverter
    {
        public double X { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value < X;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class LessThanYToTrueConverter : IValueConverter
    {
        public double Y { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)value < Y;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
