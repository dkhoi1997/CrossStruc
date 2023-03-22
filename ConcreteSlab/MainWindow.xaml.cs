using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using ExtRobot = CrossStruc.Extensions.RobotInteractive;

namespace CrossStruc.ConcreteSlab
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetForceClick(object sender, RoutedEventArgs e)
        {
            HashSet<int> hashPanel = new HashSet<int>();
            List<int[]> listQuery = new List<int[]>();

            (hashPanel, listQuery) = ExtRobot.ExtractPanelResult("100to108");

            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                RestoreDirectory = true,
                Filter = "Text Files (*.csv)|*.csv",
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string title1 = "/// Column Data & Internal Force ///";
                string space = default;
                string title2 = "/// Design Data ///";
                using (StreamWriter sw = File.CreateText(saveFileDialog.FileName))
                {
                    sw.WriteLine(title2);
                    sw.WriteLine(string.Join(",", hashPanel));
                    sw.WriteLine(space);
                    sw.WriteLine(title1);
                    foreach (var item in listQuery)
                    {
                        sw.WriteLine(string.Join(",", item));
                    }
                }
            }
        }
    }
}
