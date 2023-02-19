using CrossStruc.ConcreteColumn.Function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExtRobot = CrossStruc.Extensions.RobotInteractive;
using ExtMaterial = CrossStruc.Extensions.Material;
using CrossStruc.Extensions;

namespace CrossStruc.ConcreteColumn
{

    public partial class MainWindow : Window
    {
        public List<(string[], List<int[]>)> listCol;
        public List<(string[], List<(double[], double[,], double[,])>)> listResult;
        public string[] arrCol = new string[24];

        public MainWindow()
        {
            InitializeComponent();
            Closing += OnWindowClosing;
            Loaded += DynamicChange;
            shapeSect_cbb.SelectionChanged += DynamicChange;
            mLayer_cbb.SelectionChanged += DynamicChange;
            tw_txt.TextChanged += DynamicChange;
            Cx_txt.TextChanged += DynamicChange;
            Cy_txt.TextChanged += DynamicChange;
            nx_txt.TextChanged += DynamicChange;
            ny_txt.TextChanged += DynamicChange;
            acv_txt.TextChanged += DynamicChange;
            dmain_cbb.SelectionChanged += DynamicChange;
            dstir_cbb.SelectionChanged += DynamicChange;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure want to quit ?", "RC Column Design", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void DynamicChange(object sender, EventArgs e) // Handle cho các thay đổi thông tin trên Form
        {
            string secShape = (shapeSect_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string mLayer = (mLayer_cbb.SelectedItem as ComboBoxItem).Content.ToString();

            if (mLayer != "No")
            {
                tw_txt.IsEnabled = true;
            }
            else
            {
                tw_txt.IsEnabled = false;
            }

            if (secShape == "Rec")
            {
                Cx_txt.IsEnabled = true;
                Cy_txt.IsEnabled = true;
                nx_txt.IsEnabled = true;
                ny_txt.IsEnabled = true;
                Cx_lbl.Content = "Dimension along X-axis";
                Cy_lbl.Content = "Dimension along Y-axis";
                nx_lbl.Content = "N.o rebar along X axis";
                ny_lbl.Content = "N.o rebar along Y axis";
            }
            else
            {
                Cx_txt.IsEnabled = true;
                Cy_txt.IsEnabled = false;
                nx_txt.IsEnabled = true;
                ny_txt.IsEnabled = false;
                Cx_lbl.Content = "Diameter";
                Cy_lbl.Content = null;
                nx_lbl.Content = "N.o rebar along perimeter";
                ny_lbl.Content = null;
            }

            int tw = Convert.ToInt32(tw_txt.Text.ZeroIfEmpty());
            double Cx = Convert.ToInt32(Cx_txt.Text.ZeroIfEmpty());
            double Cy = Convert.ToInt32(Cy_txt.Text.ZeroIfEmpty());
            int nx = Convert.ToInt32(nx_txt.Text.ZeroIfEmpty());
            int ny = Convert.ToInt32(ny_txt.Text.ZeroIfEmpty());
            double acv = Convert.ToInt32(acv_txt.Text.ZeroIfEmpty());
            int dmain = Convert.ToInt32((dmain_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int dstir = Convert.ToInt32((dstir_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));

            List<int[]> rebarlist = ElementPosition.Rebar(secShape, mLayer, tw, Cx, Cy, nx, ny, acv, dmain, dstir);

            double percent = SubExtensions.RebarPercent(secShape, Cx, Cy, rebarlist.Count, dmain);
            percent_txt.Text = Convert.ToString(percent);
            total_txt.Text = Convert.ToString(rebarlist.Count) + (dmain_cbb.SelectedItem as ComboBoxItem).Content.ToString();

            DataContext = new ChartSectColumn(secShape, mLayer, tw, Cx, Cy, nx, ny, acv, dmain, dstir);


        }

        private void DataGridCellMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)dataGrid.SelectedItem;
            string barname = Convert.ToString(dataRowView.Row[0]);
            double vedlimit = Convert.ToDouble(limitACR_txt.Text);
            List<(double[], double[,], double[,])> listsend = listResult.FirstOrDefault(t => t.Item1[0] == barname).Item2;
            DetailWindow mywindow = new(listsend, arrCol);
            mywindow.ShowDialog();
        }

        private void ReCalculate()
        {
            string concGrade = (concGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string lRebarGrade = (longituGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string sRebarGrade = (stirrupGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();

            string secShape = (shapeSect_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string mLayer = (mLayer_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            bool localAxis = false;
            if (rotateAxis_cb.IsChecked == true)
            {
                localAxis = true;
            }
            double Cx = Convert.ToDouble(Cx_txt.Text.ZeroIfEmpty());
            double Cy = Convert.ToDouble(Cy_txt.Text.ZeroIfEmpty());
            double Lx = Convert.ToDouble(Lx_txt.Text.ZeroIfEmpty());
            double Ly = Convert.ToDouble(Ly_txt.Text.ZeroIfEmpty());
            double kx = Convert.ToDouble(kx_txt.Text.ZeroIfEmpty());
            double ky = Convert.ToDouble(ky_txt.Text.ZeroIfEmpty());
            double acv = Convert.ToDouble(acv_txt.Text.ZeroIfEmpty());

            int nx = Convert.ToInt32(nx_txt.Text.ZeroIfEmpty());
            int ny = Convert.ToInt32(ny_txt.Text.ZeroIfEmpty());
            int dmain = Convert.ToInt32((dmain_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int dstir = Convert.ToInt32((dstir_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int tw = Convert.ToInt32(tw_txt.Text.ZeroIfEmpty());
            int sw = Convert.ToInt32(sw_txt.Text.ZeroIfEmpty());
            int nsx = Convert.ToInt32(nsx_txt.Text.ZeroIfEmpty());
            int nsy = Convert.ToInt32(nsy_txt.Text.ZeroIfEmpty());
            string vedlimit = limitACR_txt.Text;
            string combACR = combACR_txt.Text.Replace(" q", "");

            listResult = Solve.GetResultColumn(listCol, concGrade, lRebarGrade, sRebarGrade,
                secShape, mLayer, Cx, Cy, Lx, Ly, kx, ky, acv, nx, ny, dmain, dstir, tw, sw, nsx, nsy, combACR, localAxis);

            DataTable dtcol = new DataTable();
            dtcol.Columns.Add("Name");
            dtcol.Columns.Add("Section");
            dtcol.Columns.Add("Shape");
            dtcol.Columns.Add("Case");
            dtcol.Columns.Add("Flexural");
            dtcol.Columns.Add("Shear");
            dtcol.Columns.Add("ACR");

            foreach (var item in listResult)
            {
                string[] add = new string[7];
                add[0] = item.Item1[0]; // Name
                add[1] = item.Item1[1]; // Section
                add[2] = item.Item1[2]; // Shape

                double maxDC = item.Item2.Max(t => t.Item1[17]); // Position in array
                double maxDCs = item.Item2.Max(t => t.Item1[26]); // Position in array
                double maxved = item.Item2.Max(t => t.Item1[28]); // Position in array
                string caseDC = Convert.ToString(item.Item2.FirstOrDefault(t => t.Item1[17] == maxDC).Item1[0]);
                string caseDCs = Convert.ToString(item.Item2.FirstOrDefault(t => t.Item1[26] == maxDCs).Item1[0]);
                string caseved = Convert.ToString(item.Item2.FirstOrDefault(t => t.Item1[28] == maxved).Item1[0]);

                add[4] = Convert.ToString(maxDC);
                add[5] = Convert.ToString(maxDCs);
                add[6] = Convert.ToString(maxved);
                if (string.IsNullOrEmpty(combACR))
                {
                    add[3] = caseDC + "-" + caseDCs;
                }
                else
                {
                    add[3] = caseDC + "-" + caseDCs + "-" + caseved;
                }
                dtcol.Rows.Add(add);

                dataGrid.DataContext = dtcol.DefaultView;
            }

            // Save data for detail form
            arrCol[0] = concGrade;
            arrCol[1] = lRebarGrade;
            arrCol[2] = sRebarGrade;
            arrCol[3] = secShape;
            arrCol[4] = mLayer;
            arrCol[5] = Convert.ToString(Cx);
            arrCol[6] = Convert.ToString(Cy);
            arrCol[7] = Convert.ToString(Lx);
            arrCol[8] = Convert.ToString(Ly);
            arrCol[9] = Convert.ToString(kx);
            arrCol[10] = Convert.ToString(ky);
            arrCol[11] = Convert.ToString(acv);
            arrCol[12] = Convert.ToString(nx);
            arrCol[13] = Convert.ToString(ny);
            arrCol[14] = Convert.ToString(dmain);
            arrCol[15] = Convert.ToString(dstir);
            arrCol[16] = Convert.ToString(tw);
            arrCol[17] = Convert.ToString(sw);
            arrCol[18] = Convert.ToString(nsx);
            arrCol[19] = Convert.ToString(nsy);
            arrCol[20] = Convert.ToString(ElementPosition.Rebar(secShape, mLayer, tw, Cx, Cy, nx, ny, acv, dmain, dstir).Count);
            arrCol[21] = combACR;
            arrCol[22] = vedlimit;
            arrCol[23] = Convert.ToString(localAxis);
        }

        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;

            else
                e.Handled = true;
        }

        // Button

        private void GetForceClick(object sender, RoutedEventArgs e)
        {
            string comb = combULS_txt.Text;
            if (string.IsNullOrEmpty(comb))
            {
                ManualForceWindow mywindow = new();
                mywindow.ShowDialog();
                listCol = mywindow.SendBack();
            }
            else
            {
                listCol = ExtRobot.GetConcColumnForceRobot(comb);
            }
            ReCalculate();
        }

        private void CheckClick(object sender, RoutedEventArgs e)
        {
            ReCalculate();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            ImportExport.SaveFile(listCol, arrCol);
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            (listCol, arrCol) = ImportExport.LoadFile();

            if (listCol.Count > 0)
            {
                concGrade_cbb.Text = arrCol[0];
                longituGrade_cbb.Text = arrCol[1];
                stirrupGrade_cbb.Text = arrCol[2];

                shapeSect_cbb.Text = arrCol[3];
                mLayer_cbb.Text = arrCol[4];
                Cx_txt.Text = arrCol[5];
                Cy_txt.Text = arrCol[6];
                Lx_txt.Text = arrCol[7];
                Ly_txt.Text = arrCol[8];
                kx_txt.Text = arrCol[9];
                ky_txt.Text = arrCol[10];
                acv_txt.Text = arrCol[11];

                nx_txt.Text = arrCol[12];
                ny_txt.Text = arrCol[13];
                dmain_cbb.Text = "Ø" + arrCol[14];
                dstir_cbb.Text = "Ø" + arrCol[15];
                tw_txt.Text = arrCol[16];
                sw_txt.Text = arrCol[17];
                nsx_txt.Text = arrCol[18];
                nsy_txt.Text = arrCol[19];

                combACR_txt.Text = arrCol[21];
                limitACR_txt.Text = arrCol[22];
                if (arrCol[23] == "True")
                {
                    rotateAxis_cb.IsChecked = true;
                }
                else
                {
                    rotateAxis_cb.IsChecked = false;
                }
                ReCalculate();
            }
        }
    }
}
