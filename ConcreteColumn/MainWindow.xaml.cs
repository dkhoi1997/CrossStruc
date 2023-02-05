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

namespace CrossStruc.ConcreteColumn
{

    public partial class MainWindow : Window
    {
        public static List<(string[], List<int[]>)> listCol;
        public static List<(string[], List<(double[], double[,], double[,])>)> listResult;
        public static string[] arrCol = new string[31];

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

            int tw = Convert.ToInt32(tw_txt.Text);
            double Cx = Convert.ToInt32(Cx_txt.Text);
            double Cy = Convert.ToInt32(Cy_txt.Text);
            int nx = Convert.ToInt32(nx_txt.Text);
            int ny = Convert.ToInt32(ny_txt.Text);
            double acv = Convert.ToInt32(acv_txt.Text);
            int dmain = Convert.ToInt32((dmain_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int dstir = Convert.ToInt32((dstir_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));

            List<int[]> rebarlist = ElementPosition.Rebar(secShape, mLayer, tw, Cx, Cy, nx, ny, acv, dmain, dstir);

            double percent = SubExtensions.RebarPercent(secShape, Cx, Cy, rebarlist.Count, dmain);
            percent_txt.Text = Convert.ToString(percent);
            total_txt.Text = Convert.ToString(rebarlist.Count) + dmain_cbb.Text;

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
            string concgrade = (concGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string lRebargrade = (longituGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string sRebargrade = (stirrupGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();

            // Material
            (double Rbn, double Rbtn, double Eb) = ExtMaterial.GetConcrete(concgrade);
            (double Rsn, double Rscn, double foo, double Es) = ExtMaterial.GetRebar(lRebargrade);
            double Rswn = ExtMaterial.GetRebar(sRebargrade).Item3;

            // Design strength
            double Rb = Math.Round(Rbn / 1.3, 2);
            double Rbt = Math.Round(Rbtn / 1.5, 2);
            double Rs = Math.Round(Rsn / 1.15, 0);
            double Rsc = Math.Round(Rscn / 1.15, 0);
            double Rsw = Math.Round(Rswn / 1.15, 0);

            string secShape = (shapeSect_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string mLayer = (mLayer_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            bool localAxis = false;
            if (rotateAxis_cb.IsChecked == true)
            {
                localAxis = true;
            }
            double Cx = Convert.ToDouble(Cx_txt.Text);
            double Cy = Convert.ToDouble(Cy_txt.Text);
            double Lx = Convert.ToDouble(Lx_txt.Text);
            double Ly = Convert.ToDouble(Ly_txt.Text);
            double kx = Convert.ToDouble(kx_txt.Text);
            double ky = Convert.ToDouble(ky_txt.Text);
            double acv = Convert.ToDouble(acv_txt.Text);

            int nx = Convert.ToInt32(nx_txt.Text);
            int ny = Convert.ToInt32(ny_txt.Text);
            int dmain = Convert.ToInt32((dmain_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int dstir = Convert.ToInt32((dstir_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int tw = Convert.ToInt32(tw_txt.Text);
            int sw = Convert.ToInt32(sw_txt.Text);
            int nsx = Convert.ToInt32(nsx_txt.Text);
            int nsy = Convert.ToInt32(nsy_txt.Text);
            string vedlimit = limitACR_txt.Text;
            string combACR = combACR_txt.Text.Replace(" q", "");

            listResult = Solve.GetResultColumn(listCol, Rb, Rbt, Eb, Rs, Rsc, Es, Rsw,
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

                double maxDC = item.Item2.Max(t => t.Item1[15]); // Position in array
                double maxDCs = item.Item2.Max(t => t.Item1[24]); // Position in array
                double maxved = item.Item2.Max(t => t.Item1[26]); // Position in array
                string caseDC = Convert.ToString(item.Item2.FirstOrDefault(t => t.Item1[15] == maxDC).Item1[0]);
                string caseDCs = Convert.ToString(item.Item2.FirstOrDefault(t => t.Item1[24] == maxDCs).Item1[0]);
                string caseved = Convert.ToString(item.Item2.FirstOrDefault(t => t.Item1[26] == maxved).Item1[0]);

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
            arrCol[0] = concgrade;
            arrCol[1] = Convert.ToString(Rb);
            arrCol[2] = Convert.ToString(Rbt);
            arrCol[3] = Convert.ToString(Eb);
            arrCol[4] = lRebargrade;
            arrCol[5] = Convert.ToString(Rs);
            arrCol[6] = Convert.ToString(Rsc);
            arrCol[7] = Convert.ToString(Es);
            arrCol[8] = sRebargrade;
            arrCol[9] = Convert.ToString(Rsw);
            arrCol[10] = secShape;
            arrCol[11] = mLayer;
            arrCol[12] = Convert.ToString(Cx);
            arrCol[13] = Convert.ToString(Cy);
            arrCol[14] = Convert.ToString(Lx);
            arrCol[15] = Convert.ToString(Ly);
            arrCol[16] = Convert.ToString(kx);
            arrCol[17] = Convert.ToString(ky);
            arrCol[18] = Convert.ToString(acv);
            arrCol[19] = Convert.ToString(nx);
            arrCol[20] = Convert.ToString(ny);
            arrCol[21] = Convert.ToString(dmain);
            arrCol[22] = Convert.ToString(dstir);
            arrCol[23] = Convert.ToString(tw);
            arrCol[24] = Convert.ToString(sw);
            arrCol[25] = Convert.ToString(nsx);
            arrCol[26] = Convert.ToString(nsy);
            arrCol[27] = Convert.ToString(ElementPosition.Rebar(secShape, mLayer, tw, Cx, Cy, nx, ny, acv, dmain, dstir).Count);
            arrCol[28] = combACR;
            arrCol[29] = vedlimit;
            arrCol[30] = Convert.ToString(localAxis);
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
                longituGrade_cbb.Text = arrCol[5];
                stirrupGrade_cbb.Text = arrCol[8];

                shapeSect_cbb.Text = arrCol[10];
                mLayer_cbb.Text = arrCol[11];
                Cx_txt.Text = arrCol[12];
                Cy_txt.Text = arrCol[13];
                Lx_txt.Text = arrCol[14];
                Ly_txt.Text = arrCol[15];
                kx_txt.Text = arrCol[16];
                ky_txt.Text = arrCol[17];
                acv_txt.Text = arrCol[18];

                nx_txt.Text = arrCol[19];
                ny_txt.Text = arrCol[20];
                dmain_cbb.Text = "Ø" + arrCol[21];
                dstir_cbb.Text = "Ø" + arrCol[22];
                tw_txt.Text = arrCol[23];
                sw_txt.Text = arrCol[24];
                nsx_txt.Text = arrCol[25];
                nsy_txt.Text = arrCol[26];

                combACR_txt.Text = arrCol[28];
                limitACR_txt.Text = arrCol[29];
                if (arrCol[30] == "TRUE")
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
