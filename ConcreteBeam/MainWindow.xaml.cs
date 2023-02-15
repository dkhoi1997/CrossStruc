using CrossStruc.ConcreteBeam.Function;
using CrossStruc.Extensions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace CrossStruc.ConcreteBeam
{
    public partial class MainWindow : Window
    {
        public static List<(string[], List<int[]>)> listBeam;
        public static List<(string[], List<int[]>)> listSub;
        public static List<(string[], List<double[]>)> listResult;
        public static string[] arrBeam = new string[39];
        public static int[] arrCTrebar = new int[4];
        public static int[] arrLrebar = new int[11];
        public static int[] arrMrebar = new int[11];



        public MainWindow()
        {
            InitializeComponent();

            Closing += OnWindowClosing;
            Loaded += DynamicChange;
            Tsect_cb.Click += DynamicChange;
            TsectRev_cb.Click += DynamicChange;
            tf_txt.SelectionChanged += DynamicChange;
            b_txt.TextChanged += DynamicChange;
            h_txt.TextChanged += DynamicChange;
            tw_txt.TextChanged += DynamicChange;
            acv_txt.TextChanged += DynamicChange;

            n1top_txt.TextChanged += DynamicChange;
            d1top_cbb.SelectionChanged += DynamicChange;

            n1bot_txt.TextChanged += DynamicChange;
            d1bot_cbb.SelectionChanged += DynamicChange;

            Lc2top_cb.Click += DynamicChange;
            Ln2top_txt.TextChanged += DynamicChange;
            Ld2top_cbb.SelectionChanged += DynamicChange;

            Lc3top_cb.Click += DynamicChange;
            Ln3top_txt.TextChanged += DynamicChange;
            Ld3top_cbb.SelectionChanged += DynamicChange;

            Lc2bot_cb.Click += DynamicChange;
            Ln2bot_txt.TextChanged += DynamicChange;
            Ld2bot_cbb.SelectionChanged += DynamicChange;

            Lc3bot_cb.Click += DynamicChange;
            Ln3bot_txt.TextChanged += DynamicChange;
            Ld3bot_cbb.SelectionChanged += DynamicChange;

            Mc2top_cb.Click += DynamicChange;
            Mn2top_txt.TextChanged += DynamicChange;
            Md2top_cbb.SelectionChanged += DynamicChange;

            Mc3top_cb.Click += DynamicChange;
            Mn3top_txt.TextChanged += DynamicChange;
            Md3top_cbb.SelectionChanged += DynamicChange;

            Mc2bot_cb.Click += DynamicChange;
            Mn2bot_txt.TextChanged += DynamicChange;
            Md2bot_cbb.SelectionChanged += DynamicChange;

            Mc3bot_cb.Click += DynamicChange;
            Mn3bot_txt.TextChanged += DynamicChange;
            Md3bot_cbb.SelectionChanged += DynamicChange;

            Lds_cbb.SelectionChanged += DynamicChange;
            Lns_txt.TextChanged += DynamicChange;
            Lsw_txt.TextChanged += DynamicChange;

            Mds_cbb.SelectionChanged += DynamicChange;
            Mns_txt.TextChanged += DynamicChange;
            Lsw_txt.TextChanged += DynamicChange;
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure want to quit ?", "RC Beam Design", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void DataGridCellMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)dataGrid.SelectedItem;
            string barname = Convert.ToString(dataRowView.Row[0]);
            List<double[]> listsend = listResult.FirstOrDefault(t => t.Item1[0] == barname).Item2;
            DetailWindow mywindow = new(listsend, arrBeam);
            mywindow.ShowDialog();
        }



        private void OnlyNumber(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (regex.IsMatch(e.Text) && !(e.Text == "-" && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;

            else
                e.Handled = true;
        }

        private void DynamicChange(object sender, EventArgs e)
        {

            // Section parameter
            int b = Convert.ToInt32(b_txt.Text);
            int h = Convert.ToInt32(h_txt.Text);
            int hs = 0;
            double tw = Convert.ToDouble(tw_txt.Text);
            double acv = Convert.ToDouble(acv_txt.Text);
            bool Tsect = false; bool revertTsect = false;
            if (Tsect_cb.IsChecked == true)
            {
                Tsect = true;
                tf_txt.IsEnabled = true;
                TsectRev_cb.IsEnabled = true;
                bs_txt.IsEnabled = true;
                hs = Convert.ToInt32(tf_txt.Text);
                if (TsectRev_cb.IsChecked == true)
                {
                    revertTsect = true;

                }
            }
            else
            {
                tf_txt.IsEnabled = false;
                TsectRev_cb.IsEnabled = false;
                bs_txt.IsEnabled = false;
            }

            // Main rebar top layer
            int n1top = Convert.ToInt32(n1top_txt.Text);
            int d1top = Convert.ToInt32((d1top_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));

            // Main rebar bot layer
            int n1bot = Convert.ToInt32(n1bot_txt.Text);
            int d1bot = Convert.ToInt32((d1bot_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));

            // Extra rebar top layer (support)
            int Ln2top = 0;
            int Ld2top = 0;
            int Ln3top = 0;
            int Ld3top = 0;
            if (Lc2top_cb.IsChecked == true)
            {
                Ln2top_txt.IsEnabled = true;
                Ld2top_cbb.IsEnabled = true;
                Ln2top = Convert.ToInt32(Ln2top_txt.Text);
                Ld2top = Convert.ToInt32((Ld2top_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
                Lc3top_cb.IsEnabled = true;
                if (Lc3top_cb.IsChecked == true)
                {
                    Ln3top_txt.IsEnabled = true;
                    Ld3top_cbb.IsEnabled = true;
                    Ln3top = Convert.ToInt32(Ln3top_txt.Text);
                    Ld3top = Convert.ToInt32((Ld3top_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
                }
                else
                {
                    Ln3top_txt.IsEnabled = false;
                    Ld3top_cbb.IsEnabled = false;
                }
            }
            else
            {
                Ln2top_txt.IsEnabled = false;
                Ld2top_cbb.IsEnabled = false;
                Lc3top_cb.IsEnabled = false;
                Ln3top_txt.IsEnabled = false;
                Ld3top_cbb.IsEnabled = false;
            }

            // Extra rebar bot layer (support)
            int Ln2bot = 0;
            int Ld2bot = 0;
            int Ln3bot = 0;
            int Ld3bot = 0;
            if (Lc2bot_cb.IsChecked == true)
            {
                Ln2bot_txt.IsEnabled = true;
                Ld2bot_cbb.IsEnabled = true;
                Ln2bot = Convert.ToInt32(Ln2bot_txt.Text);
                Ld2bot = Convert.ToInt32((Ld2bot_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
                Lc3bot_cb.IsEnabled = true;
                if (Lc3bot_cb.IsChecked == true)
                {
                    Ln3bot_txt.IsEnabled = true;
                    Ld3bot_cbb.IsEnabled = true;
                    Ln3bot = Convert.ToInt32(Ln3bot_txt.Text);
                    Ld3bot = Convert.ToInt32((Ld3bot_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
                }
                else
                {
                    Ln3bot_txt.IsEnabled = false;
                    Ld3bot_cbb.IsEnabled = false;
                }
            }
            else
            {
                Ln2bot_txt.IsEnabled = false;
                Ld2bot_cbb.IsEnabled = false;
                Lc3bot_cb.IsEnabled = false;
                Ln3bot_txt.IsEnabled = false;
                Ld3bot_cbb.IsEnabled = false;
            }

            // Extra rebar top layer (mid)
            int Mn2top = 0;
            int Md2top = 0;
            int Mn3top = 0;
            int Md3top = 0;
            if (Mc2top_cb.IsChecked == true)
            {
                Mn2top_txt.IsEnabled = true;
                Md2top_cbb.IsEnabled = true;
                Mn2top = Convert.ToInt32(Mn2top_txt.Text);
                Md2top = Convert.ToInt32((Md2top_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
                Mc3top_cb.IsEnabled = true;
                if (Mc3top_cb.IsChecked == true)
                {
                    Mn3top_txt.IsEnabled = true;
                    Md3top_cbb.IsEnabled = true;
                    Mn3top = Convert.ToInt32(Mn3top_txt.Text);
                    Md3top = Convert.ToInt32((Md3top_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
                }
                else
                {
                    Mn3top_txt.IsEnabled = false;
                    Md3top_cbb.IsEnabled = false;
                }
            }
            else
            {
                Mn2top_txt.IsEnabled = false;
                Md2top_cbb.IsEnabled = false;
                Mc3top_cb.IsEnabled = false;
                Mn3top_txt.IsEnabled = false;
                Md3top_cbb.IsEnabled = false;
            }

            // Extra rebar bot layer (mid)
            int Mn2bot = 0;
            int Md2bot = 0;
            int Mn3bot = 0;
            int Md3bot = 0;
            if (Mc2bot_cb.IsChecked == true)
            {
                Mn2bot_txt.IsEnabled = true;
                Md2bot_cbb.IsEnabled = true;
                Mn2bot = Convert.ToInt32(Mn2bot_txt.Text);
                Md2bot = Convert.ToInt32((Md2bot_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
                Mc3bot_cb.IsEnabled = true;
                if (Mc3bot_cb.IsChecked == true)
                {
                    Mn3bot_txt.IsEnabled = true;
                    Md3bot_cbb.IsEnabled = true;
                    Mn3bot = Convert.ToInt32(Mn3bot_txt.Text);
                    Md3bot = Convert.ToInt32((Md3bot_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
                }
                else
                {
                    Mn3bot_txt.IsEnabled = false;
                    Md3bot_cbb.IsEnabled = false;
                }
            }
            else
            {
                Mn2bot_txt.IsEnabled = false;
                Md2bot_cbb.IsEnabled = false;
                Mc3bot_cb.IsEnabled = false;
                Mn3bot_txt.IsEnabled = false;
                Md3bot_cbb.IsEnabled = false;
            }


            // Stirrup
            int Lds = Convert.ToInt32((Lds_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int Lns = Convert.ToInt32(Lns_txt.Text);
            int Lsw = Convert.ToInt32(Lsw_txt.Text);
            int Mds = Convert.ToInt32((Mds_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int Mns = Convert.ToInt32(Mns_txt.Text);
            int Msw = Convert.ToInt32(Msw_txt.Text);

            // Section chart
            SectL_Plot.DataContext = new ChartSectBeam(b, h, Tsect, revertTsect, hs, acv, tw,
                n1top, d1top, Ln2top, Ld2top, Ln3top, Ld3top, n1bot, d1bot, Ln2bot, Ld2bot, Ln3bot, Ld3bot, Lds);
            SectM_Plot.DataContext = new ChartSectBeam(b, h, Tsect, revertTsect, hs, acv, tw,
                n1top, d1top, Mn2top, Md2top, Mn3top, Md3top, n1bot, d1bot, Mn2bot, Md2bot, Mn3bot, Md3bot, Mds);

            // Rebar percentage
            double LperTop = SubExtensions.RebarProperty(b, h, acv, tw, n1top, d1top, Ln2top, Ld2top, Ln3top, Ld3top, Lds).Item2;
            double LperBot = SubExtensions.RebarProperty(b, h, acv, tw, n1bot, d1bot, Ln2bot, Ld2bot, Ln3bot, Ld3bot, Lds).Item2;
            double MperTop = SubExtensions.RebarProperty(b, h, acv, tw, n1top, d1top, Mn2top, Md2top, Mn3top, Md3top, Mds).Item2;
            double MperBot = SubExtensions.RebarProperty(b, h, acv, tw, n1bot, d1bot, Mn2bot, Md2bot, Mn3bot, Md3bot, Mds).Item2;
            LtopPer_lbl.Content = Convert.ToString(LperTop) + " %";
            LbotPer_lbl.Content = Convert.ToString(LperBot) + " %";
            MtopPer_lbl.Content = Convert.ToString(MperTop) + " %";
            MbotPer_lbl.Content = Convert.ToString(MperBot) + " %";

            // Data for calc
            arrCTrebar = new int[] { n1top, d1top, n1bot, d1bot };
            arrLrebar = new int[] { Ln2top, Ld2top, Ln3top, Ld3top, Ln2bot, Ld2bot, Ln3bot, Ld3bot, Lds, Lns, Lsw };
            arrMrebar = new int[] { Mn2top, Md2top, Mn3top, Md3top, Mn2bot, Md2bot, Mn3bot, Md3bot, Mds, Mns, Msw };
        }

        private void Recalculate()
        {
            string concGrade = (concGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string lRebarGrade = (longituGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string sRebarGrade = (stirrupGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string hmClass = (Humidity_cbb.SelectedItem as ComboBoxItem).Content.ToString();

            // Section property
            double b = Convert.ToInt32(b_txt.Text);
            double h = Convert.ToInt32(h_txt.Text);
            double tf = 0;
            double bs = 0;
            double tw = Convert.ToDouble(tw_txt.Text);
            double acv = Convert.ToDouble(acv_txt.Text);
            bool Tsect = false;
            bool revertTsect = false;
            bool compressBar = false;

            // Crack width parameter check
            double acrcSlim = Convert.ToDouble(ast_txt.Text);
            double acrcLlim = Convert.ToDouble(alt_txt.Text);

            if (Tsect_cb.IsChecked == true)
            {
                Tsect = true;
                tf = Convert.ToInt32(tf_txt.Text);
                bs = Convert.ToInt32(bs_txt.Text);
                if (TsectRev_cb.IsChecked == true)
                {
                    revertTsect = true;
                }
            }

            if (compRebar_cb.IsChecked == true)
            {
                compressBar = true;
            }

            string ULScomb = combULS_txt.Text;
            string SLScomb = combSLS_txt.Text;
            bool enveDesign = false;
            List<int> listULScomb = Other.ExtractCombRobot(ULScomb);
            List<int> listSLScomb = Other.ExtractCombRobot(SLScomb);

            if (enveCheck_cb.IsChecked == true)
            {
                enveDesign = true;
            }
            listSub = SubExtensions.FilterBeamForce(listBeam, enveDesign, listULScomb, listSLScomb);
            listResult = Solve.GetResultBeam(listSub, concGrade, lRebarGrade, sRebarGrade, hmClass, b, h, tf, bs, Tsect,
                revertTsect, compressBar, acv, tw, acrcSlim, acrcLlim, arrCTrebar, arrLrebar, arrMrebar);

            DataTable dtcol = new DataTable();
            dtcol.Columns.Add("Name");
            dtcol.Columns.Add("Section");
            dtcol.Columns.Add("Shape");
            dtcol.Columns.Add("Position");
            dtcol.Columns.Add("Flexural");
            dtcol.Columns.Add("Shear + Tor");
            dtcol.Columns.Add("CrWidth-S");
            dtcol.Columns.Add("CrWidth-L");

            foreach (var item in listResult)
            {
                for (int i = 0; i < 2; i++)
                {
                    string[] add = new string[8];
                    add[0] = item.Item1[0]; // Name
                    add[1] = item.Item1[1]; // Section
                    add[2] = item.Item1[2]; // Shape

                    if (i == 0)
                    {
                        add[3] = "Sup";
                    }
                    else
                    {
                        add[3] = "Mid";
                    }

                    double maxDC = Math.Max(item.Item2[i][48], item.Item2[i][49]);
                    double maxDCs = item.Item2[i][50];

                    double maxDCacrcS = Math.Max(item.Item2[i][51], item.Item2[i][52]);
                    double maxDCacrcL = Math.Max(item.Item2[i][53], item.Item2[i][54]);

                    add[4] = Convert.ToString(maxDC);
                    add[5] = Convert.ToString(maxDCs);
                    add[6] = Convert.ToString(maxDCacrcS);
                    add[7] = Convert.ToString(maxDCacrcL);

                    dtcol.Rows.Add(add);
                }

                dataGrid.DataContext = dtcol.DefaultView;
            }

            // Save data for detail form
            arrBeam[0] = concGrade;
            arrBeam[1] = lRebarGrade;
            arrBeam[2] = sRebarGrade;
            arrBeam[3] = Convert.ToString(b);
            arrBeam[4] = Convert.ToString(h);
            arrBeam[5] = Convert.ToString(Tsect);
            arrBeam[6] = Convert.ToString(compressBar);
            arrBeam[7] = Convert.ToString(bs);
            arrBeam[8] = Convert.ToString(tf);
            arrBeam[9] = Convert.ToString(acv);
            arrBeam[10] = Convert.ToString(tw);
            arrBeam[11] = Convert.ToString(acrcSlim);
            arrBeam[12] = Convert.ToString(acrcLlim);

            arrBeam[13] = Convert.ToString(arrCTrebar[0]);
            arrBeam[14] = Convert.ToString(arrCTrebar[1]);
            arrBeam[15] = Convert.ToString(arrCTrebar[2]);
            arrBeam[16] = Convert.ToString(arrCTrebar[3]);

            arrBeam[17] = Convert.ToString(arrLrebar[0]);
            arrBeam[18] = Convert.ToString(arrLrebar[1]);
            arrBeam[19] = Convert.ToString(arrLrebar[2]);
            arrBeam[20] = Convert.ToString(arrLrebar[3]);
            arrBeam[21] = Convert.ToString(arrLrebar[4]);
            arrBeam[22] = Convert.ToString(arrLrebar[5]);
            arrBeam[23] = Convert.ToString(arrLrebar[6]);
            arrBeam[24] = Convert.ToString(arrLrebar[7]);
            arrBeam[25] = Convert.ToString(arrLrebar[8]);
            arrBeam[26] = Convert.ToString(arrLrebar[9]);
            arrBeam[27] = Convert.ToString(arrLrebar[10]);

            arrBeam[28] = Convert.ToString(arrMrebar[0]);
            arrBeam[29] = Convert.ToString(arrMrebar[1]);
            arrBeam[30] = Convert.ToString(arrMrebar[2]);
            arrBeam[31] = Convert.ToString(arrMrebar[3]);
            arrBeam[32] = Convert.ToString(arrMrebar[4]);
            arrBeam[33] = Convert.ToString(arrMrebar[5]);
            arrBeam[34] = Convert.ToString(arrMrebar[6]);
            arrBeam[35] = Convert.ToString(arrMrebar[7]);
            arrBeam[36] = Convert.ToString(arrMrebar[8]);
            arrBeam[37] = Convert.ToString(arrMrebar[9]);
            arrBeam[38] = Convert.ToString(arrMrebar[10]);

        }

        private void GetForceClick(object sender, RoutedEventArgs e)
        {
            string ULScomb = combULS_txt.Text;
            string SLScomb = combSLS_txt.Text;
            string combine = ULScomb + " " + SLScomb;

            listBeam = RobotInteractive.GetConcBeamForceRobot(combine);

            Recalculate();
        }

        private void CheckClick(object sender, RoutedEventArgs e)
        {
            Recalculate();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                RestoreDirectory = true,
                Filter = "Text Files (*.csv)|*.csv",
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string title1 = "/// Beam Data & Internal Force ///";
                string space = default;
                string title2 = "/// Design Data ///";
                using (StreamWriter sw = File.CreateText(saveFileDialog.FileName))
                {
                    sw.WriteLine(title2);
                    sw.WriteLine(space);
                    sw.WriteLine(title1);
                    foreach (var item in listResult)
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

        private void LoadClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
