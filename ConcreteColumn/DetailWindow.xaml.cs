﻿using CrossStruc.ConcreteColumn.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
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
            string lRebarGrade = arrCol[1];
            string sRebarGrade = arrCol[2];

            (double Rbn, double Rbtn, double Eb) = ExtMaterial.GetConcrete(concGrade);
            (double Rsn, double Rscn, double foo, double Es) = ExtMaterial.GetRebar(lRebarGrade);
            double Rswn = ExtMaterial.GetRebar(sRebarGrade).Item3;

            string secShape = arrCol[3];
            string mLayer = arrCol[4];
            double Cx = Convert.ToDouble(arrCol[5]);
            double Cy = Convert.ToDouble(arrCol[6]);
            string Lx = arrCol[7];
            string Ly = arrCol[8];
            string kx = arrCol[9];
            string ky = arrCol[10];
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

            double Ab; double As; double u;

            double DC = listSend.Max(t => t.Item1[17]);
            int Pxy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[1]);
            int Mx = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[4]);
            int My = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[5]);
            double e0x = listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[6];
            double e0y = listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[7];
            double Ncrx = listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[8];
            double Ncry = listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[9];
            double etax = listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[10];
            double etay = listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[11];
            int Mux = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[12]);
            int Muy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[13]);
            int Muxy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[14]);
            int Pnxy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[15]);
            int Mnxy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[17] == DC).Item1[16]);

            double DCs = listSend.Max(t => t.Item1[26]);
            int PQ = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[26] == DCs).Item1[1]);
            int Qx = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[26] == DCs).Item1[2]);
            int Qy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[26] == DCs).Item1[3]);
            double sigma = listSend.FirstOrDefault(t => t.Item1[26] == DCs).Item1[18];
            double phin = listSend.FirstOrDefault(t => t.Item1[26] == DCs).Item1[19];
            int cx = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[26] == DCs).Item1[20]);
            int cy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[26] == DCs).Item1[21]);
            int Qbx = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[26] == DCs).Item1[22]);
            int Qsx = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[26] == DCs).Item1[23]);
            int Qby = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[26] == DCs).Item1[24]);
            int Qsy = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[26] == DCs).Item1[25]);

            double ved = listSend.Max(t => t.Item1[28]);
            int Ped = Convert.ToInt32(listSend.FirstOrDefault(t => t.Item1[28] == ved).Item1[1]);
            double fcd = listSend.FirstOrDefault(t => t.Item1[28] == ved).Item1[27];

            double[,] output_PMxy = listSend.FirstOrDefault(t => t.Item1[17] == DC).Item2;
            double[,] output_MxMy = listSend.FirstOrDefault(t => t.Item1[17] == DC).Item3;


            // Material data
            material_tb.Inlines.Add("Concrete " + concGrade + ", ");
            material_tb.Inlines.Add("R");
            material_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "bn" });
            material_tb.Inlines.Add(" = " + Rbn + " (MPa), ");
            material_tb.Inlines.Add("R");
            material_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "btn" });
            material_tb.Inlines.Add(" = " + Rbtn + " (MPa)");
            material_tb.Inlines.Add(new LineBreak());

            material_tb.Inlines.Add("Main bars " + lRebarGrade + ", ");
            material_tb.Inlines.Add("R");
            material_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "sn" });
            material_tb.Inlines.Add(" = " + Rsn + " (MPa), ");
            material_tb.Inlines.Add("R");
            material_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "scn" });
            material_tb.Inlines.Add(" = " + Rscn + " (MPa)");
            material_tb.Inlines.Add(new LineBreak());

            material_tb.Inlines.Add("Ties " + sRebarGrade + ", ");
            material_tb.Inlines.Add("R");
            material_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "swn" });
            material_tb.Inlines.Add(" = " + Rswn + " (MPa)");

            // Section parameter
            if (secShape == "Rec")
            {
                Ab = Math.Round(Cx * Cy, 0);
                sect_tb.Inlines.Add("C");
                sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
                sect_tb.Inlines.Add(" × ");
                sect_tb.Inlines.Add("C");
                sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
                sect_tb.Inlines.Add(" = " + Cx + " × " + Cy + " (mm), ");
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
            sect_tb.Inlines.Add("k");
            sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "x" });
            sect_tb.Inlines.Add(" = " + kx + ", ");
            sect_tb.Inlines.Add("N");
            sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crx" });
            sect_tb.Inlines.Add(" = " + Ncrx + " (kN)");
            sect_tb.Inlines.Add(new LineBreak());

            sect_tb.Inlines.Add("L");
            sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
            sect_tb.Inlines.Add(" = " + Ly + " (mm), ");
            sect_tb.Inlines.Add("k");
            sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "y" });
            sect_tb.Inlines.Add(" = " + ky + ", ");
            sect_tb.Inlines.Add("N");
            sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "cry" });
            sect_tb.Inlines.Add(" = " + Ncry + " (kN)");

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
            if (ved != 0)
            {
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
            IDChartPM_Plot.DataContext = new ChartIDColumn("P-Mxy", output_PMxy, output_MxMy, Muxy, Pxy, Mux, Muy, Cx, Cy);
            IDChartMM_Plot.DataContext = new ChartIDColumn("Mx-My", output_PMxy, output_MxMy, Muxy, Pxy, Mux, Muy, Cx, Cy);
        }
    }
}
