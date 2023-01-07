using CrossStruc.ConcreteColumn.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CrossStruc.ConcreteColumn
{
    public partial class DetailWindow : Window
    {
        double[,] output_PMxy;
        double[,] output_MxMy;
        double Cx; double Cy; int Mux; int Muy; int P; int Muxy;
        public DetailWindow(List<(double[], double[,], double[,])> listSend, string[] arrCol)
        {
            InitializeComponent();
            this.MouseRightButtonDown += MenuItem_PreviewRightMouseButtonDown;

            // Lấy thông tin cột từ form chính ném qua
            string concGrade = arrCol[0];
            string lRebarGrade = arrCol[1];
            string sRebarGrade = arrCol[2];
            string secShape = arrCol[3];
            string mLayer = arrCol[4];
            Cx = Convert.ToDouble(arrCol[5]);
            Cy = Convert.ToDouble(arrCol[6]);
            string Lx = arrCol[7];
            string Ly = arrCol[8];
            double acv = Convert.ToDouble(arrCol[11]);
            int nx = Convert.ToInt32(arrCol[12]);
            int ny = Convert.ToInt32(arrCol[13]);
            int dmain = Convert.ToInt32(arrCol[14]);
            int dstir = Convert.ToInt32(arrCol[15]);
            int tw = Convert.ToInt32(arrCol[16]);
            string sw = arrCol[17];
            string nsx = arrCol[18];
            string nsy = arrCol[19];
            int sumn = Convert.ToInt32(arrCol[20]);
            double ved_limit = Convert.ToDouble(arrCol[22]);

            // Thông tin chung
            concGrade_lbl.Content = concGrade;
            rebarGrade_lbl.Content = lRebarGrade;
            stirGrade_lbl.Content = sRebarGrade;
            sumRebar_lbl.Content = sumn + "Ø" + dmain + " - " + SubExtensions.RebarPercent(secShape, Cx, Cy, sumn, dmain) + " %";
            stirRebar_lbl.Content = "Ø" + dstir + "a" + sw;
            stirLegged_lbl.Content = nsx + " - " + nsy;

            // Kích thước cột (măc định là Rec)
            if (secShape == "Cir")
            {
                dimX_lbl.Content = "Diameter";
                dimY_lbl.Content = "Radius";
                Cx_tb.Text = "D";
                Cy_tb.Text = "r";
                Cx_lbl.Content = Convert.ToString(Cx);
                Cy_lbl.Content = Convert.ToString(Cx / 2);
            }
            else
            {
                Cx_lbl.Content = Convert.ToString(Cx);
                Cy_lbl.Content = Convert.ToString(Cy);
            }
            Lx_lbl.Content = Lx;
            Ly_lbl.Content = Ly;
            acv_lbl.Content = Convert.ToString(acv);


            // Flexural
            double DC = listSend.Max(t => t.Item1[14]);
            P = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[14] == DC).Item1[1]);
            Mux = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[14] == DC).Item1[6]);
            Muy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[14] == DC).Item1[7]);
            Muxy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[14] == DC).Item1[8]);

            Pxy_lbl.Content = Convert.ToString(P);
            Mx_lbl.Content = Convert.ToString(listSend.FirstOrDefault(t => t.Item1[14] == DC).Item1[4]);
            My_lbl.Content = Convert.ToString(listSend.FirstOrDefault(t => t.Item1[14] == DC).Item1[5]);
            Mux_lbl.Content = Convert.ToString(Mux);
            Muy_lbl.Content = Convert.ToString(Muy);
            Muxy_lbl.Content = Convert.ToString(Muxy);
            Pnxy_lbl.Content = Convert.ToString(listSend.FirstOrDefault(t => t.Item1[14] == DC).Item1[9]);
            Mnxy_lbl.Content = Convert.ToString(listSend.FirstOrDefault(t => t.Item1[14] == DC).Item1[10]);
            DC_lbl.Content = Convert.ToString(DC);
            if (DC > 1)
            {
                DC_lbl.Foreground = Brushes.Red;
            }

            // Shear strength result
            double DCs = listSend.Max(t => t.Item1[15]);
            Pq_lbl.Content = Convert.ToString(listSend.FirstOrDefault(t => t.Item1[15] == DCs).Item1[1]);
            Qx_lbl.Content = Convert.ToString(listSend.FirstOrDefault(t => t.Item1[15] == DCs).Item1[2]);
            Qy_lbl.Content = Convert.ToString(listSend.FirstOrDefault(t => t.Item1[15] == DCs).Item1[3]);
            Qnx_lbl.Content = Convert.ToString(listSend.FirstOrDefault(t => t.Item1[15] == DCs).Item1[11]);
            Qny_lbl.Content = Convert.ToString(listSend.FirstOrDefault(t => t.Item1[15] == DCs).Item1[12]);
            phi_lbl.Content = Convert.ToString(listSend.FirstOrDefault(t => t.Item1[15] == DCs).Item1[13]);
            DCs_lbl.Content = Convert.ToString(DCs);
            if (DCs > 1)
            {
                DCs_lbl.Foreground = Brushes.Red;
            }

            // ACR result
            double ved = listSend.Max(t => t.Item1[16]);
            if (ved != 0)
            {
                Ped_lbl.Content = Convert.ToString(listSend.FirstOrDefault(t => t.Item1[16] == ved).Item1[1]);
                ved_lbl.Content = Convert.ToString(ved);
                if (ved > ved_limit)
                {
                    ved_lbl.Foreground = Brushes.Red;
                }
            }

            // Chart
            output_PMxy = listSend.FirstOrDefault(t => t.Item1[14] == DC).Item2;
            output_MxMy = listSend.FirstOrDefault(t => t.Item1[14] == DC).Item3;

            Sect_Plot.DataContext = new ChartSectColumn(secShape, mLayer, tw, Cx, Cy, nx, ny, acv, dmain, dstir);
            IDChart_Plot.DataContext = new ChartIDColumn("P-Mxy", output_PMxy, output_MxMy, Muxy, P, Mux, Muy, Cx, Cy);
        }

        private void MenuItem_PreviewRightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
            cm.IsOpen = true;
        }

        private void PMxy_Click(object sender, RoutedEventArgs e)
        {
            X_axis_tb.FontSize = 12;
            X_axis_tb.Text = "M";
            X_axis_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "xy" });
            X_axis_tb.Inlines.Add(" (kNm)");

            Y_axis_tb.FontSize = 12;
            Y_axis_tb.Text = "P";
            Y_axis_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "xy" });
            Y_axis_tb.Inlines.Add(" (kN)");

            IDChart_Plot.DataContext = new ChartIDColumn("P-Mxy", output_PMxy, output_MxMy, Muxy, P, Mux, Muy, Cx, Cy);
        }

        private void MxMy_Click(object sender, RoutedEventArgs e)
        {
            X_axis_tb.FontSize = 12;
            X_axis_tb.Text = "M";
            X_axis_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
            X_axis_tb.Inlines.Add(" (kNm)");

            Y_axis_tb.FontSize = 12;
            Y_axis_tb.Text = "M";
            Y_axis_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
            Y_axis_tb.Inlines.Add(" (kNm)");

            IDChart_Plot.DataContext = new ChartIDColumn("Mx-My", output_PMxy, output_MxMy, Muxy, P, Mux, Muy, Cx, Cy);
        }
    }
}
