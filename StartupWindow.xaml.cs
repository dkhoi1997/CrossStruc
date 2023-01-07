using System.Windows;
using ConcColumn = CrossStruc.ConcreteColumn;
using ConcBeam = CrossStruc.ConcreteBeam;

namespace CrossStruc
{
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }
        private void ConcreteColumnClick(object sender, RoutedEventArgs e)
        {
            ConcColumn.MainWindow mywindows = new ();
            mywindows.ShowDialog();
        }

        private void ConcreteBeamClick(object sender, RoutedEventArgs e)
        {
            ConcBeam.MainWindow mywindows = new();
            mywindows.ShowDialog();
        }

    }
}
