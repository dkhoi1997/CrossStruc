using CrossStruc.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CrossStruc.ConcreteBeam
{
    public class InputForceBeam
    {
        public int Mt { get; set; }
        public int Mb { get; set; }
        public int Mts { get; set; }
        public int Mbs { get; set; }
        public int Q { get; set; }
        public int T { get; set; }
    }
    public partial class ManualForceWindow : Window
    {
        public ObservableCollection<InputForceBeam> dataInputForceBeam = new ObservableCollection<InputForceBeam>();
        public ManualForceWindow()
        {
            InitializeComponent();
            InitInputBeamForce();
            dataGridForce.ItemsSource = dataInputForceBeam;
        }

        private void Clear_Click(object sender, RoutedEventArgs e) // Clear all data in DataGrid
        {
            dataInputForceBeam.Clear();
        }

        private void Paste_Click(object sender, RoutedEventArgs e) // Paste data from struc like Excel
        {
            dataInputForceBeam.Clear();
            int count = 0; 

            string s = Clipboard.GetText();
            string[] lines = s.Split('\n');
            foreach (string item in lines)
            {
                string[] fields = item.Split('\t');
                bool add = true;
                foreach (string field in fields)
                {
                    if (string.IsNullOrEmpty(field))
                    {
                        add = false;
                        break;
                    }
                }
                if ((add == true) & (count < 2)) // Only paste two line
                {
                    count += 1;
                    InputForceBeam temp = new InputForceBeam();
                    temp.Mt = Convert.ToInt32(fields[0].ZeroIfEmpty());
                    temp.Mb = Convert.ToInt32(fields[1].ZeroIfEmpty());
                    temp.Mts = Convert.ToInt32(fields[2].ZeroIfEmpty());
                    temp.Mbs = Convert.ToInt32(fields[3].ZeroIfEmpty());
                    temp.Q = Convert.ToInt32(fields[4].ZeroIfEmpty());
                    temp.T = Convert.ToInt32(fields[5].ZeroIfEmpty());
                    dataInputForceBeam.Add(temp);
                }
            }
            dataGridForce.ItemsSource = dataInputForceBeam;
        }

        private void InitInputBeamForce()
        {
            dataInputForceBeam.Clear();
            for (int i = 0; i < 2; i++)
            {
                InputForceBeam temp = new InputForceBeam();
                dataInputForceBeam.Add(temp);
            }
            dataGridForce.ItemsSource = dataInputForceBeam;
        }

        public List<(string[], List<int[]>)> SendBack()
        {
            List<(string[], List<int[]>)> listBeam = new List<(string[], List<int[]>)>();
            string[] arrbeam = new string[6];
            arrbeam[0] = "Empty";
            List<int[]> listForce = new List<int[]>();

            foreach (InputForceBeam item in dataInputForceBeam)
            {
                MessageBox.Show(Convert.ToString(item.Mt));
                int[] force = new int[6] { item.Mt, item.Mb, item.Mts, item.Mbs, item.Q, item.T };
                listForce.Add(force);
            }

            listBeam.Add((arrbeam, listForce));

            return listBeam;

        }

        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
