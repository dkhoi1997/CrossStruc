using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CrossStruc.ConcreteColumn
{
    public partial class ManualForceWindow : Window
    {
        public ManualForceWindow()
        {
            InitializeComponent();
        }

        public List<(string[], List<int[]>)> SendBack()
        {
            List<(string[], List<int[]>)> listCol = new List<(string[], List<int[]>)>();
            string name = name_txt.Text;
            int P = Convert.ToInt32(P_txt.Text);
            int Qx = Convert.ToInt32(Qx_txt.Text);
            int Qy = Convert.ToInt32(Qy_txt.Text);
            int T = Convert.ToInt32(T_txt.Text);
            int Mx = Convert.ToInt32(Mx_txt.Text);
            int My = Convert.ToInt32(My_txt.Text);

            string[] arrcol = new string[6];
            int[] force = new int[7] { 0, P, Qx, Qy, T, Mx, My };
            arrcol[0] = name;
            List<int[]> listForce = new List<int[]>();
            listForce.Add(force);
            listCol.Add((arrcol, listForce));

            return listCol;

        }

        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;

            else
                e.Handled = true;
        }
    }
}
