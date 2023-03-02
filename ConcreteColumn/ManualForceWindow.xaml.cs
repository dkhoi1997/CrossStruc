using CrossStruc.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CrossStruc.ConcreteColumn
{
    public class InputForceColumn
    {
        public int P { get; set; }
        public int Qx { get; set; }
        public int Qy { get; set; }
        public int T { get; set; }
        public int Mx { get; set; }
        public int My { get; set; }
    }
    public partial class ManualForceWindow : Window
    {

        public ObservableCollection<InputForceColumn> dataInputForceColumn = new ObservableCollection<InputForceColumn>();
        public ManualForceWindow()
        {
            InitializeComponent();
            dataGridForce.ItemsSource = dataInputForceColumn;
        }

        private void Clear_Click(object sender, RoutedEventArgs e) // Clear all data in DataGrid
        {
            dataInputForceColumn.Clear();
        }

        private void Paste_Click(object sender, RoutedEventArgs e) // Paste data from struc like Excel
        {
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
                if (add == true)
                {
                    InputForceColumn temp = new InputForceColumn();
                    temp.P = Convert.ToInt32(fields[0].ZeroIfEmpty());
                    temp.Qx = Convert.ToInt32(fields[1].ZeroIfEmpty());
                    temp.Qy = Convert.ToInt32(fields[2].ZeroIfEmpty());
                    temp.T = Convert.ToInt32(fields[3].ZeroIfEmpty());
                    temp.Mx = Convert.ToInt32(fields[4].ZeroIfEmpty());
                    temp.My = Convert.ToInt32(fields[5].ZeroIfEmpty());
                    dataInputForceColumn.Add(temp);
                }
            }
            dataGridForce.ItemsSource = dataInputForceColumn;
        }

        public List<(string[], List<int[]>)> SendBack()
        {
            List<(string[], List<int[]>)> listCol = new List<(string[], List<int[]>)>();
            string[] arrcol = new string[6];
            arrcol[0] = "Empty";
            List<int[]> listForce = new List<int[]>();

            if (dataInputForceColumn.Count > 0)
            {
                foreach (InputForceColumn item in dataInputForceColumn)
                {
                    int[] force = new int[7] { 0, item.P, item.Qx, item.Qy, item.T, item.Mx, item.My };
                    listForce.Add(force);
                }
            }
            else
            {
                int[] force = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
                listForce.Add(force);
            }

            listCol.Add((arrcol, listForce));

            return listCol;

        }

        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
