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

        public CounterControl()
        {
            InitializeComponent();
        }

        public void Show(string label, int from, int to)
        {
            FlipElement.ResetFlip();
            Label = label;
            FrontSide = from;
            BackSide = to;
            FlipElement.FlipY();
        }
    }
}
