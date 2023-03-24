using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OneHundredAndEighty.Score
{
    /// <summary>
    /// Interaktionslogik für WhiteboardControl.xaml
    /// </summary>
    public partial class WhiteboardControl : UserControl
    {
        public WhiteboardControl()
        {
            InitializeComponent();
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                ScoreViewModel scoreVM = ((App)Application.Current).Game.scoreVM;
                this.DataContext = scoreVM;
                scoreVM.ScoresChanged += (s, e) => UpdateScoresView();
            }
        }

        private void UpdateScoresView()
        {
            if (Player1Whiteboard.Items.Count > Player2Whiteboard.Items.Count)
            {
                double offset = ScrollToEnd(Player1Whiteboard);
                GetScrollViewer(Player2Whiteboard)?.ScrollToVerticalOffset(offset - 1);
            }
            else if (Player1Whiteboard.Items.Count < Player2Whiteboard.Items.Count)
            {
                double offset = ScrollToEnd(Player2Whiteboard);
                GetScrollViewer(Player1Whiteboard)?.ScrollToVerticalOffset(offset - 1);
            }
            else
            {
                ScrollToEnd(Player1Whiteboard);
                ScrollToEnd(Player2Whiteboard);
            }
        }

        private ScrollViewer GetScrollViewer(DataGrid g) 
        {
            var border = VisualTreeHelper.GetChild(g, 0) as Decorator;
            return border?.Child as ScrollViewer;
        }

        private double ScrollToEnd(DataGrid g)
        {
            ScrollViewer sw1 = GetScrollViewer(g);
            if (sw1 is null) return 0.0;

            sw1.ScrollToEnd();
            UpdateLayout();
            sw1.ScrollToVerticalOffset(sw1.VerticalOffset - 1);
            return sw1.VerticalOffset;
        }

        private void DataGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DataGrid g = sender as DataGrid;
            if (g is null) return;

            var headersPresenter = FindVisualChild<DataGridColumnHeadersPresenter>(g);
            double actualHeight = headersPresenter.ActualHeight;

            g.RowHeight = (g.ActualHeight - actualHeight) / 3;
        }

        public static T FindVisualChild<T>(DependencyObject current) where T : DependencyObject
        {
            if (current == null) return null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(current);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(current, i);
                if (child is T) return (T)child;
                T result = FindVisualChild<T>(child);
                if (result != null) return result;
            }
            return null;
        }
    }
}
