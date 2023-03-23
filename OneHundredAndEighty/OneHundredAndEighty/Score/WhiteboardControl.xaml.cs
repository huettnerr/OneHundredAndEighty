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
            ScrollToEnd(Player1Whiteboard);
            ScrollToEnd(Player2Whiteboard);
            return;

            if (Player1Whiteboard.Items.Count > Player2Whiteboard.Items.Count)
            {
                ScrollToEnd(Player1Whiteboard);
                Player2Whiteboard.ScrollIntoView(Player2Whiteboard.Items[Player2Whiteboard.Items.Count - 1]);
            }
            else if (Player1Whiteboard.Items.Count < Player2Whiteboard.Items.Count)
            {
                Player1Whiteboard.ScrollIntoView(Player1Whiteboard.Items[Player1Whiteboard.Items.Count - 1]);
                ScrollToEnd(Player2Whiteboard);
            }
            else
            {
                ScrollToEnd(Player1Whiteboard);
                ScrollToEnd(Player2Whiteboard);
            }
        }

        private void ScrollToEnd(DataGrid g)
        {
            int i = g.Items.Count > 1 ? g.Items.Count - 2 : g.Items.Count - 1;
            if (i < 0) return;

            g.ScrollIntoView(g.Items[i]);
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
