using System.Windows;
using ConcColumn = CrossStruc.ConcreteColumn;
using ConcBeam = CrossStruc.ConcreteBeam;
using ConcSlab = CrossStruc.ConcreteSlab;

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

        private void ConcreteSlabClick(object sender, RoutedEventArgs e)
        {
            ConcSlab.MainWindow mywindows = new();
            mywindows.ShowDialog();
        }

    }
}
