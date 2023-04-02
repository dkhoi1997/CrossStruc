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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetForceClick(object sender, RoutedEventArgs e)
        {
            List<(string[], List<int[]>)> listPanel = new List<(string[], List<int[]>)> ();

            listPanel = ExtRobot.GetConcSlabForceRobot("100to108");

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
                    sw.WriteLine(string.Join(",", "Haha"));
                    sw.WriteLine(space);
                    sw.WriteLine(title1);
                    foreach (var item in listPanel)
                    {
                        sw.WriteLine(string.Join(",", item.Item1));
                        for (int i = 0; i < item.Item2.Count; i++)
                        {
                            sw.WriteLine(string.Join(",", item.Item2[i]));
                        }
                    }
                }
            }
        }
    }
}
