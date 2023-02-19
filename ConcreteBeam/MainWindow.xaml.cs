using CrossStruc.ConcreteBeam.Function;
using CrossStruc.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace CrossStruc.ConcreteBeam
{
    public partial class MainWindow : Window
    {
        public List<(string[], List<int[]>)> listBeam;
        public List<(string[], List<double[]>)> listResult;
        public string[] arrBeam = new string[42];
        public int[] arrCTrebar = new int[4];
        public int[] arrLrebar = new int[11];
        public int[] arrMrebar = new int[11];

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

            Ln2top_txt.TextChanged += DynamicChange;
            Ld2top_cbb.SelectionChanged += DynamicChange;

            Ln3top_txt.TextChanged += DynamicChange;
            Ld3top_cbb.SelectionChanged += DynamicChange;

            Ln2bot_txt.TextChanged += DynamicChange;
            Ld2bot_cbb.SelectionChanged += DynamicChange;

            Ln3bot_txt.TextChanged += DynamicChange;
            Ld3bot_cbb.SelectionChanged += DynamicChange;

            Mn2top_txt.TextChanged += DynamicChange;
            Md2top_cbb.SelectionChanged += DynamicChange;

            Mn3top_txt.TextChanged += DynamicChange;
            Md3top_cbb.SelectionChanged += DynamicChange;

            Mn2bot_txt.TextChanged += DynamicChange;
            Md2bot_cbb.SelectionChanged += DynamicChange;

            Mn3bot_txt.TextChanged += DynamicChange;
            Md3bot_cbb.SelectionChanged += DynamicChange;

            Lds_cbb.SelectionChanged += DynamicChange;
            Lns_txt.TextChanged += DynamicChange;
            Lsw_txt.TextChanged += DynamicChange;

            Mds_cbb.SelectionChanged += DynamicChange;
            Mns_txt.TextChanged += DynamicChange;
            Msw_txt.TextChanged += DynamicChange;
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
            int b = Convert.ToInt32(b_txt.Text.ZeroIfEmpty());
            int h = Convert.ToInt32(h_txt.Text.ZeroIfEmpty());
            int hs = 0;
            double tw = Convert.ToDouble(tw_txt.Text.ZeroIfEmpty());
            double acv = Convert.ToDouble(acv_txt.Text.ZeroIfEmpty());
            bool Tsect = false; bool revertTsect = false;
            if (Tsect_cb.IsChecked == true)
            {
                Tsect = true;
                tf_txt.IsEnabled = true;
                TsectRev_cb.IsEnabled = true;
                bs_txt.IsEnabled = true;
                hs = Convert.ToInt32(tf_txt.Text.ZeroIfEmpty());
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
            int n1top = Convert.ToInt32(n1top_txt.Text.ZeroIfEmpty());
            int d1top = Convert.ToInt32((d1top_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int n1bot = Convert.ToInt32(n1bot_txt.Text.ZeroIfEmpty());
            int d1bot = Convert.ToInt32((d1bot_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));


            // Extra rebar top layer (support)
            int Ln2top = Convert.ToInt32(Ln2top_txt.Text.ZeroIfEmpty());
            int Ld2top = Convert.ToInt32((Ld2top_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int Ln3top = Convert.ToInt32(Ln3top_txt.Text.ZeroIfEmpty());
            int Ld3top = Convert.ToInt32((Ld3top_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));

            // Extra rebar bot layer (support)
            int Ln2bot = Convert.ToInt32(Ln2bot_txt.Text.ZeroIfEmpty());
            int Ld2bot = Convert.ToInt32((Ld2bot_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int Ln3bot = Convert.ToInt32(Ln3bot_txt.Text.ZeroIfEmpty());
            int Ld3bot = Convert.ToInt32((Ld3bot_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));

            // Extra rebar top layer (mid)
            int Mn2top = Convert.ToInt32(Mn2top_txt.Text.ZeroIfEmpty());
            int Md2top = Convert.ToInt32((Md2top_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int Mn3top = Convert.ToInt32(Mn3top_txt.Text.ZeroIfEmpty());
            int Md3top = Convert.ToInt32((Md3top_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));

            // Extra rebar bot layer (mid)
            int Mn2bot = Convert.ToInt32(Mn2bot_txt.Text.ZeroIfEmpty());
            int Md2bot = Convert.ToInt32((Md2bot_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int Mn3bot = Convert.ToInt32(Mn3bot_txt.Text.ZeroIfEmpty());
            int Md3bot = Convert.ToInt32((Md3bot_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));

            // Stirrup
            int Lds = Convert.ToInt32((Lds_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int Lns = Convert.ToInt32(Lns_txt.Text.ZeroIfEmpty());
            int Lsw = Convert.ToInt32(Lsw_txt.Text.ZeroIfEmpty());
            int Mds = Convert.ToInt32((Mds_cbb.SelectedItem as ComboBoxItem).Content.ToString().TrimStart('Ø'));
            int Mns = Convert.ToInt32(Mns_txt.Text.ZeroIfEmpty());
            int Msw = Convert.ToInt32(Msw_txt.Text.ZeroIfEmpty());

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

        private void ReCalculate()
        {
            string concGrade = (concGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string lRebarGrade = (longituGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string sRebarGrade = (stirrupGrade_cbb.SelectedItem as ComboBoxItem).Content.ToString();
            string hmClass = (Humidity_cbb.SelectedItem as ComboBoxItem).Content.ToString();

            // Section property
            double b = Convert.ToInt32(b_txt.Text.ZeroIfEmpty());
            double h = Convert.ToInt32(h_txt.Text.ZeroIfEmpty());
            double tf = 0;
            double bs = 0;
            double tw = Convert.ToDouble(tw_txt.Text.ZeroIfEmpty());
            double acv = Convert.ToDouble(acv_txt.Text.ZeroIfEmpty());
            bool Tsect = false;
            bool revertTsect = false;
            bool compressBar = false;
            bool enveDesign = false;

            // Crack width parameter check
            double acrcSlim = Convert.ToDouble(ast_txt.Text.ZeroIfEmpty());
            double acrcLlim = Convert.ToDouble(alt_txt.Text.ZeroIfEmpty());

            if (Tsect_cb.IsChecked == true)
            {
                Tsect = true;
                tf = Convert.ToInt32(tf_txt.Text.ZeroIfEmpty());
                bs = Convert.ToInt32(bs_txt.Text.ZeroIfEmpty());
                if (TsectRev_cb.IsChecked == true)
                {
                    revertTsect = true;
                }
            }

            if (compRebar_cb.IsChecked == true)
            {
                compressBar = true;
            }

            List<(string[], List<int[]>)> listBeamForce = listBeam;

            if (enveCheck_cb.IsChecked == true)
            {
                enveDesign = true;
                listBeamForce = SubExtensions.FilterEnveBeamForce(listBeam);
            }
            listResult = Solve.GetResultBeam(listBeamForce, concGrade, lRebarGrade, sRebarGrade, hmClass, b, h, tf, bs, Tsect,
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

                    double maxDC = Math.Max(item.Item2[i][49], item.Item2[i][50]);
                    double maxDCs = item.Item2[i][51];

                    double maxDCacrcS = Math.Max(item.Item2[i][52], item.Item2[i][53]);
                    double maxDCacrcL = Math.Max(item.Item2[i][54], item.Item2[i][55]);

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
            arrBeam[6] = Convert.ToString(revertTsect);
            arrBeam[7] = Convert.ToString(compressBar);
            arrBeam[8] = Convert.ToString(bs);
            arrBeam[9] = Convert.ToString(tf);
            arrBeam[10] = Convert.ToString(acv);
            arrBeam[11] = Convert.ToString(tw);
            arrBeam[12] = Convert.ToString(hmClass);
            arrBeam[13] = Convert.ToString(enveDesign);
            arrBeam[14] = Convert.ToString(acrcSlim);
            arrBeam[15] = Convert.ToString(acrcLlim);

            arrBeam[16] = Convert.ToString(arrCTrebar[0]);
            arrBeam[17] = Convert.ToString(arrCTrebar[1]);
            arrBeam[18] = Convert.ToString(arrCTrebar[2]);
            arrBeam[19] = Convert.ToString(arrCTrebar[3]);

            arrBeam[20] = Convert.ToString(arrLrebar[0]);
            arrBeam[21] = Convert.ToString(arrLrebar[1]);
            arrBeam[22] = Convert.ToString(arrLrebar[2]);
            arrBeam[23] = Convert.ToString(arrLrebar[3]);
            arrBeam[24] = Convert.ToString(arrLrebar[4]);
            arrBeam[25] = Convert.ToString(arrLrebar[5]);
            arrBeam[26] = Convert.ToString(arrLrebar[6]);
            arrBeam[27] = Convert.ToString(arrLrebar[7]);
            arrBeam[28] = Convert.ToString(arrLrebar[8]);
            arrBeam[29] = Convert.ToString(arrLrebar[9]);
            arrBeam[30] = Convert.ToString(arrLrebar[10]);

            arrBeam[31] = Convert.ToString(arrMrebar[0]);
            arrBeam[32] = Convert.ToString(arrMrebar[1]);
            arrBeam[33] = Convert.ToString(arrMrebar[2]);
            arrBeam[34] = Convert.ToString(arrMrebar[3]);
            arrBeam[35] = Convert.ToString(arrMrebar[4]);
            arrBeam[36] = Convert.ToString(arrMrebar[5]);
            arrBeam[37] = Convert.ToString(arrMrebar[6]);
            arrBeam[38] = Convert.ToString(arrMrebar[7]);
            arrBeam[39] = Convert.ToString(arrMrebar[8]);
            arrBeam[40] = Convert.ToString(arrMrebar[9]);
            arrBeam[41] = Convert.ToString(arrMrebar[10]);

        }

        // Button

        private void GetForceClick(object sender, RoutedEventArgs e)
        {
            string ULScomb = combULS_txt.Text;
            string SLScomb = combSLS_txt.Text;
            if (string.IsNullOrEmpty(ULScomb) || string.IsNullOrEmpty(SLScomb))
            {

            }
            else
            {
                string combine = ULScomb + " " + SLScomb;
                List<int> listULScomb = Other.ExtractCombRobot(ULScomb);
                List<int> listSLScomb = Other.ExtractCombRobot(SLScomb);
                List<(string[], List<int[]>)> listSub = RobotInteractive.GetConcBeamForceRobot(combine);
                listBeam = SubExtensions.FilterBeamForce(listSub, listULScomb, listSLScomb);
            }
            ReCalculate();
        }

        private void CheckClick(object sender, RoutedEventArgs e)
        {
            ReCalculate();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            ImportExport.SaveFile(listBeam, arrBeam);
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            (listBeam, arrBeam) = ImportExport.LoadFile();

            if (listBeam.Count > 0)
            {
                concGrade_cbb.Text = arrBeam[0];
                longituGrade_cbb.Text = arrBeam[1];
                stirrupGrade_cbb.Text = arrBeam[2];

                b_txt.Text = arrBeam[3];
                h_txt.Text = arrBeam[4];

                acv_txt.Text = arrBeam[10];
                tw_txt.Text = arrBeam[11];
                Humidity_cbb.Text = arrBeam[12];

                ast_txt.Text = arrBeam[14];
                alt_txt.Text = arrBeam[15];

                // T-sect data
                if (arrBeam[5] == "True")
                {
                    Tsect_cb.IsChecked = true;
                    tf_txt.IsEnabled = true;
                    bs_txt.IsEnabled = true;
                    if (arrBeam[6] == "True")
                    {
                        TsectRev_cb.IsEnabled = true;
                        TsectRev_cb.IsChecked = true;
                    }
                    else
                    {
                        TsectRev_cb.IsEnabled = false;
                        TsectRev_cb.IsChecked = false;
                    }
                    bs_txt.Text = arrBeam[8];
                    tf_txt.Text = arrBeam[9];
                }
                else
                {
                    Tsect_cb.IsChecked = false;
                    tf_txt.IsEnabled = false;
                    bs_txt.IsEnabled = false;
                }

                // Consider compress rebar
                if (arrBeam[7] == "True")
                {
                    compRebar_cb.IsChecked = true;
                }
                else
                {
                    compRebar_cb.IsChecked = false;
                }

                // Envelop design style
                if (arrBeam[13] == "True")
                {
                    enveCheck_cb.IsChecked = true;
                }
                else
                {
                    enveCheck_cb.IsChecked = false;
                }

                // Main rebar
                n1top_txt.Text = arrBeam[16];
                d1top_cbb.Text = "Ø" + arrBeam[17];
                n1bot_txt.Text = arrBeam[18];
                d1bot_cbb.Text = "Ø" + arrBeam[19];

                // Extra rebar top
                Ln2top_txt.Text = arrBeam[20];
                Ld2top_cbb.Text = "Ø" + arrBeam[21];
                Ln3top_txt.Text = arrBeam[22];
                Ld3top_cbb.Text = "Ø" + arrBeam[23];
                Ln2bot_txt.Text = arrBeam[24];
                Ld2bot_cbb.Text = "Ø" + arrBeam[25];
                Ln3bot_txt.Text = arrBeam[26];
                Ld3bot_cbb.Text = "Ø" + arrBeam[27];

                // Stirrup - Support
                Lds_cbb.Text = "Ø" + arrBeam[28];
                Lns_txt.Text = arrBeam[29];
                Lsw_txt.Text = arrBeam[30];

                // Extra rebar bot
                Mn2top_txt.Text = arrBeam[31];
                Md2top_cbb.Text = "Ø" + arrBeam[32];
                Mn3top_txt.Text = arrBeam[33];
                Md3top_cbb.Text = "Ø" + arrBeam[34];
                Mn2bot_txt.Text = arrBeam[35];
                Md2bot_cbb.Text = "Ø" + arrBeam[36];
                Mn3bot_txt.Text = arrBeam[37];
                Md3bot_cbb.Text = "Ø" + arrBeam[38];

                // Stirrup - Mid
                Mds_cbb.Text = "Ø" + arrBeam[39];
                Mns_txt.Text = arrBeam[40];
                Msw_txt.Text = arrBeam[41];

                ReCalculate();
            }
        }
    }
}
