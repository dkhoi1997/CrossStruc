using CrossStruc.ConcreteColumn.Function;
using System;
using System.Collections.Generic;
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
using ExtMaterial = CrossStruc.Extensions.Material;

namespace CrossStruc.ConcreteColumn
{
    public partial class DetailWindow : Window
    {
        string[] arrCol;
        List<(double[], double[,], double[,])> listSend;
        public DetailWindow(List<(double[], double[,], double[,])> listSendmainF, string[] arrColmainF)
        {
            InitializeComponent();
            arrCol = arrColmainF;
            listSend = listSendmainF;

            Window_Loaded();
        }

        public void Window_Loaded()
        {
            // Get data from main form which sent

            string concGrade = arrCol[0];
            string Rb = arrCol[1];
            string Rbt = arrCol[2];
            string lRebarGrade = arrCol[4];
            string Rs = arrCol[5];
            string Rsc = arrCol[6];
            string sRebarGrade = arrCol[8];
            string Rsw = arrCol[9];

            string secShape = arrCol[10];
            string mLayer = arrCol[11];
            double Cx = Convert.ToDouble(arrCol[12]);
            double Cy = Convert.ToDouble(arrCol[13]);
            string Lx = arrCol[14];
            string Ly = arrCol[15];
            string kx = arrCol[16];
            string ky = arrCol[17];
            double acv = Convert.ToDouble(arrCol[18]);
            int nx = Convert.ToInt32(arrCol[19]);
            int ny = Convert.ToInt32(arrCol[20]);
            int dmain = Convert.ToInt32(arrCol[21]);
            int dstir = Convert.ToInt32(arrCol[22]);
            int tw = Convert.ToInt32(arrCol[23]);
            string sw = arrCol[24];
            string nsx = arrCol[25];
            string nsy = arrCol[26];
            int sumn = Convert.ToInt32(arrCol[27]);
            double ved_limit = Convert.ToDouble(arrCol[29]);

            double Ab; double As; double u;

            // Material data
            material_tb.Inlines.Add("Concrete " + concGrade + ", ");
            material_tb.Inlines.Add("R");
            material_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
            material_tb.Inlines.Add(" = " + Rb + " (MPa), ");
            material_tb.Inlines.Add("R");
            material_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "bt" });
            material_tb.Inlines.Add(" = " + Rbt + " (MPa)");
            material_tb.Inlines.Add(new LineBreak());

            material_tb.Inlines.Add("Longitudinal " + lRebarGrade + ", ");
            material_tb.Inlines.Add("R");
            material_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "s" });
            material_tb.Inlines.Add(" = " + Rs + " (MPa), ");
            material_tb.Inlines.Add("R");
            material_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "sc" });
            material_tb.Inlines.Add(" = " + Rsc + " (MPa)");
            material_tb.Inlines.Add(new LineBreak());

            material_tb.Inlines.Add("Stirrup " + sRebarGrade + ", ");
            material_tb.Inlines.Add("R");
            material_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "sw" });
            material_tb.Inlines.Add(" = " + Rsw + " (MPa)");

            // Section parameter
            if (secShape == "Rec")
            {
                Ab = Math.Round(Cx * Cy, 0);
                sect_tb.Inlines.Add("C");
                sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
                sect_tb.Inlines.Add(" = " + Cx + " (mm), ");
                sect_tb.Inlines.Add("C");
                sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
                sect_tb.Inlines.Add(" = " + Cy + " (mm), ");
            }
            else
            {
                Ab = Math.Round(Math.PI * Math.Pow(Cx, 2) / 4, 0);
                sect_tb.Inlines.Add("D");
                sect_tb.Inlines.Add(" = " + Cx + " (mm), ");
            }
            sect_tb.Inlines.Add("A");
            sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
            sect_tb.Inlines.Add(" = " + Ab + " (mm");
            sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Top, FontSize = 10, Text = "2" });
            sect_tb.Inlines.Add(")");
            sect_tb.Inlines.Add(new LineBreak());

            sect_tb.Inlines.Add("L");
            sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
            sect_tb.Inlines.Add(" = " + Lx + " (mm), ");
            sect_tb.Inlines.Add("L");
            sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
            sect_tb.Inlines.Add(" = " + Ly + " (mm), ");
            sect_tb.Inlines.Add("k");
            sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
            sect_tb.Inlines.Add(" = " + kx + ", ");
            sect_tb.Inlines.Add("k");
            sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
            sect_tb.Inlines.Add(" = " + ky);

            // Rebar arrangement
            As = Math.Round(sumn * Math.PI * Math.Pow(dmain, 2) / 4, 0);
            u = Math.Round(100 * As / Ab, 2);
            rebar_tb.Inlines.Add(sumn + "Ø" + dmain + ", ");
            rebar_tb.Inlines.Add("A");
            rebar_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "s" });
            rebar_tb.Inlines.Add(" = " + As + " (mm");
            rebar_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Top, FontSize = 10, Text = "2" });
            rebar_tb.Inlines.Add("), ");
            rebar_tb.Inlines.Add("µ");
            rebar_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "s" });
            rebar_tb.Inlines.Add(" = " + u + " (%)");
            rebar_tb.Inlines.Add(new LineBreak());

            rebar_tb.Inlines.Add("Ø" + dstir + "a" + sw + ", ");
            rebar_tb.Inlines.Add("n");
            rebar_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
            rebar_tb.Inlines.Add(" = " + nsx + ", ");
            rebar_tb.Inlines.Add("n");
            rebar_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
            rebar_tb.Inlines.Add(" = " + nsy);

            // Flexural check
            double DC = listSend.Max(t => t.Item1[15]);
            int Pxy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[1]);
            int Mx = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[4]);
            int My = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[5]);
            double e0x = listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[6];
            double e0y = listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[7];
            double etax = listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[8];
            double etay = listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[9];
            int Mux = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[10]);
            int Muy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[11]);
            int Muxy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[12]);
            int Pnxy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[13]);
            int Mnxy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[15] == DC).Item1[14]);

            flexural_tb.Inlines.Add("P");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "xy" });
            flexural_tb.Inlines.Add(" = " + Pxy + " (kN), ");
            flexural_tb.Inlines.Add("M");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
            flexural_tb.Inlines.Add(" = " + Mx + " (kNm), ");
            flexural_tb.Inlines.Add("M");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
            flexural_tb.Inlines.Add(" = " + My + " (kNm)");
            flexural_tb.Inlines.Add(new LineBreak());

            flexural_tb.Inlines.Add("e");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "0x" });
            flexural_tb.Inlines.Add(" = " + e0x + " (mm), ");
            flexural_tb.Inlines.Add("η");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
            flexural_tb.Inlines.Add(" = " + etax + ", ");
            flexural_tb.Inlines.Add("M");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "ux" });
            flexural_tb.Inlines.Add(" = " + Mux + " (kNm)");
            flexural_tb.Inlines.Add(new LineBreak());

            flexural_tb.Inlines.Add("e");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "0y" });
            flexural_tb.Inlines.Add(" = " + e0y + " (mm), ");
            flexural_tb.Inlines.Add("η");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
            flexural_tb.Inlines.Add(" = " + etay + ", ");
            flexural_tb.Inlines.Add("M");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "uy" });
            flexural_tb.Inlines.Add(" = " + Muy + " (kNm)");
            flexural_tb.Inlines.Add(new LineBreak());

            flexural_tb.Inlines.Add("M");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "uxy" });
            flexural_tb.Inlines.Add(" = Sqrt(M");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "ux" });
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Top, FontSize = 10, Text = "2" });
            flexural_tb.Inlines.Add(" + ");
            flexural_tb.Inlines.Add("M");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "uy" });
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Top, FontSize = 10, Text = "2" });
            flexural_tb.Inlines.Add(") = " + Muxy + " (kNm)");
            flexural_tb.Inlines.Add(new LineBreak());

            flexural_tb.Inlines.Add("P");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nxy" });
            flexural_tb.Inlines.Add(" = " + Pnxy + " (kN), ");
            flexural_tb.Inlines.Add("M");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nxy" });
            flexural_tb.Inlines.Add(" = " + Mnxy + " (kNm)");
            flexural_tb.Inlines.Add(new LineBreak());

            flexural_tb.Inlines.Add("DC");
            flexural_tb.Inlines.Add(" = Sqrt(P");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "xy" });
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Top, FontSize = 10, Text = "2" });
            flexural_tb.Inlines.Add(" + ");
            flexural_tb.Inlines.Add("M");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "uxy" });
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Top, FontSize = 10, Text = "2" });

            flexural_tb.Inlines.Add(") /");
            flexural_tb.Inlines.Add(" Sqrt(P");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nxy" });
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Top, FontSize = 10, Text = "2" });
            flexural_tb.Inlines.Add(" + ");
            flexural_tb.Inlines.Add("M");
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nxy" });
            flexural_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Top, FontSize = 10, Text = "2" });
            flexural_tb.Inlines.Add(") = ");
            if (DC <= 1)
            {
                flexural_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(DC) });
            }
            else
            {
                flexural_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(DC) });
            }
            

            // Shear check
            double DCs = listSend.Max(t => t.Item1[24]);
            int PQ = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[24] == DCs).Item1[1]);
            int Qx = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[24] == DCs).Item1[2]);
            int Qy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[24] == DCs).Item1[3]);
            double sigma = listSend.FirstOrDefault(t => t.Item1[24] == DCs).Item1[16];
            double phin = listSend.FirstOrDefault(t => t.Item1[24] == DCs).Item1[17];
            int cx = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[24] == DCs).Item1[18]);
            int cy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[24] == DCs).Item1[19]);
            int Qbx = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[24] == DCs).Item1[20]);
            int Qsx = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[24] == DCs).Item1[21]);
            int Qby = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[24] == DCs).Item1[22]);
            int Qsy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[24] == DCs).Item1[23]);

            shear_tb.Inlines.Add("P");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "Q" });
            shear_tb.Inlines.Add(" = " + PQ + " (kN), ");
            shear_tb.Inlines.Add("Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
            shear_tb.Inlines.Add(" = " + Qx + " (kN), ");
            shear_tb.Inlines.Add("Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
            shear_tb.Inlines.Add(" = " + Qy + " (kN)");
            shear_tb.Inlines.Add(new LineBreak());

            shear_tb.Inlines.Add("σ");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
            shear_tb.Inlines.Add(" = " + sigma + " (MPa), ");
            shear_tb.Inlines.Add("φ");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "n" });
            shear_tb.Inlines.Add(" = " + phin);
            shear_tb.Inlines.Add(new LineBreak());

            shear_tb.Inlines.Add("c");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
            shear_tb.Inlines.Add(" = " + cx + " (mm), ");
            shear_tb.Inlines.Add("Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "bx" });
            shear_tb.Inlines.Add(" = " + Qbx + " (kN), ");
            shear_tb.Inlines.Add("Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "sx" });
            shear_tb.Inlines.Add(" = " + Qsx + " (kN)");
            shear_tb.Inlines.Add(new LineBreak());

            shear_tb.Inlines.Add("c");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
            shear_tb.Inlines.Add(" = " + cy + " (mm), ");
            shear_tb.Inlines.Add("Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "by" });
            shear_tb.Inlines.Add(" = " + Qby + " (kN), ");
            shear_tb.Inlines.Add("Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "sy" });
            shear_tb.Inlines.Add(" = " + Qsy + " (kN)");
            shear_tb.Inlines.Add(new LineBreak());

            shear_tb.Inlines.Add("DC");
            shear_tb.Inlines.Add(" = (Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
            shear_tb.Inlines.Add(" / (Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "bx" });
            shear_tb.Inlines.Add(" + Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "sx" });
            shear_tb.Inlines.Add("))");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Top, FontSize = 10, Text = "2" });
            shear_tb.Inlines.Add(" + ");
            shear_tb.Inlines.Add("(Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
            shear_tb.Inlines.Add(" / (Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "by" });
            shear_tb.Inlines.Add(" + Q");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "sy" });
            shear_tb.Inlines.Add("))");
            shear_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Top, FontSize = 10, Text = "2" });
            shear_tb.Inlines.Add(" = ");
            if (DCs <= 1)
            {
                shear_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(DCs) });
            }
            else
            {
                shear_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(DCs) });
            }

            // ACR check
            double ved = listSend.Max(t => t.Item1[26]);
            if (ved != 0)
            {
                int Ped = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[26] == ved).Item1[1]);
                double fcd = listSend.FirstOrDefault(t => t.Item1[26] == ved).Item1[25];
                acr_tb.Inlines.Add("P");
                acr_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "ed" });
                acr_tb.Inlines.Add(" = " + Ped + " (kN), ");
                acr_tb.Inlines.Add("f");
                acr_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "cd" });
                acr_tb.Inlines.Add(" = " + fcd + " (MPa)");
                acr_tb.Inlines.Add(new LineBreak());

                acr_tb.Inlines.Add("v");
                acr_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "ed" });
                acr_tb.Inlines.Add(" = ");
                acr_tb.Inlines.Add("P");
                acr_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "ed" });
                acr_tb.Inlines.Add(" / ( ");
                acr_tb.Inlines.Add("f");
                acr_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "cd" });
                acr_tb.Inlines.Add(" × ");
                acr_tb.Inlines.Add("A");
                acr_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
                acr_tb.Inlines.Add(") = ");
                if (ved <= ved_limit)
                {
                    acr_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(ved) });
                }
                else
                {
                    acr_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(ved) });
                }
            }
            else
            {
                acr_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = "Not applicable" });
            }

            // Section chart
            Sect_Plot.DataContext = new ChartSectColumn(secShape, mLayer, tw, Cx, Cy, nx, ny, acv, dmain, dstir);

            // ID chart
            double[,] output_PMxy = listSend.FirstOrDefault(t => t.Item1[15] == DC).Item2;
            double[,] output_MxMy = listSend.FirstOrDefault(t => t.Item1[15] == DC).Item3;
            IDChartPM_Plot.DataContext = new ChartIDColumn("P-Mxy", output_PMxy, output_MxMy, Muxy, Pxy, Mux, Muy, Cx, Cy);
            IDChartMM_Plot.DataContext = new ChartIDColumn("Mx-My", output_PMxy, output_MxMy, Muxy, Pxy, Mux, Muy, Cx, Cy);
        }
    }
}
