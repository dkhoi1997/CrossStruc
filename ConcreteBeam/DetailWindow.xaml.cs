using CrossStruc.ConcreteBeam.Function;
using RobotOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using ExtMaterial = CrossStruc.Extensions.Material;

namespace CrossStruc.ConcreteBeam
{
    public partial class DetailWindow : Window
    {
        string[] arrBeam;
        List<double[]> listSend;
        public DetailWindow(List<double[]> listSendmainF, string[] arrBeammainF)
        {
            InitializeComponent();
            arrBeam = arrBeammainF;
            listSend = listSendmainF;

            Window_Loaded();
        }

        public void Window_Loaded()
        {
            // Get data from main form which sent

            string concGrade = arrBeam[0];
            string lRebarGrade = arrBeam[1];
            string sRebarGrade = arrBeam[2];

            (double Rbn, double Rbtn, double Eb) = ExtMaterial.GetConcrete(concGrade);
            (double Rsn, double Rscn, double foo, double Es) = ExtMaterial.GetRebar(lRebarGrade);
            double Rswn = ExtMaterial.GetRebar(sRebarGrade).Item3;

            double[] u = new double[4];

            int b = Convert.ToInt32(arrBeam[3]);
            int h = Convert.ToInt32(arrBeam[4]);
            bool Tsect = Convert.ToBoolean(arrBeam[5]);
            bool revertTsect = Convert.ToBoolean(arrBeam[6]);
            int bs = Convert.ToInt32(arrBeam[8]);
            int bf = 2 * bs + b;
            int tf = Convert.ToInt32(arrBeam[9]);
            double acv = Convert.ToDouble(arrBeam[10]);
            double tw = Convert.ToDouble(arrBeam[11]);
            double acrcSlim = Convert.ToDouble(arrBeam[14]);
            double acrcLlim = Convert.ToDouble(arrBeam[15]);

            int n1top = Convert.ToInt32(arrBeam[16]);
            int d1top = Convert.ToInt32(arrBeam[17]);
            int n1bot = Convert.ToInt32(arrBeam[18]);
            int d1bot = Convert.ToInt32(arrBeam[19]);

            int Ln2top = Convert.ToInt32(arrBeam[20]);
            int Ld2top = Convert.ToInt32(arrBeam[21]);
            int Ln3top = Convert.ToInt32(arrBeam[22]);
            int Ld3top = Convert.ToInt32(arrBeam[23]);
            int Ln2bot = Convert.ToInt32(arrBeam[24]);
            int Ld2bot = Convert.ToInt32(arrBeam[25]);
            int Ln3bot = Convert.ToInt32(arrBeam[26]);
            int Ld3bot = Convert.ToInt32(arrBeam[27]);
            int Lds = Convert.ToInt32(arrBeam[28]);
            int Lns = Convert.ToInt32(arrBeam[29]);
            int Lsw = Convert.ToInt32(arrBeam[30]);

            int Mn2top = Convert.ToInt32(arrBeam[31]);
            int Md2top = Convert.ToInt32(arrBeam[32]);
            int Mn3top = Convert.ToInt32(arrBeam[33]);
            int Md3top = Convert.ToInt32(arrBeam[34]);
            int Mn2bot = Convert.ToInt32(arrBeam[35]);
            int Md2bot = Convert.ToInt32(arrBeam[36]);
            int Mn3bot = Convert.ToInt32(arrBeam[37]);
            int Md3bot = Convert.ToInt32(arrBeam[38]);
            int Mds = Convert.ToInt32(arrBeam[39]);
            int Mns = Convert.ToInt32(arrBeam[40]);
            int Msw = Convert.ToInt32(arrBeam[41]);


            int[,] M = new int[2, 2];
            double[,] naDepth = new double[2, 2];
            int[,] Mn = new int[2, 2];
            double[,] DC = new double[2, 2];

            M[0, 0] = Convert.ToInt32(listSend[0][0]);
            M[0, 1] = Convert.ToInt32(listSend[0][1]);
            M[1, 0] = Convert.ToInt32(listSend[1][0]);
            M[1, 1] = Convert.ToInt32(listSend[1][1]);

            naDepth[0, 0] = listSend[0][8];
            naDepth[0, 1] = listSend[0][9];
            naDepth[1, 0] = listSend[1][8];
            naDepth[1, 1] = listSend[1][9];

            Mn[0, 0] = Convert.ToInt32(listSend[0][10]);
            Mn[0, 1] = Convert.ToInt32(listSend[0][11]);
            Mn[1, 0] = Convert.ToInt32(listSend[1][10]);
            Mn[1, 1] = Convert.ToInt32(listSend[1][11]);

            DC[0, 0] = listSend[0][49];
            DC[0, 1] = listSend[0][50];
            DC[1, 0] = listSend[1][49];
            DC[1, 1] = listSend[1][50];

            int[] Q = new int[2];
            int[] T = new int[2];
            double[] c = new double[2];
            int[] Qb = new int[2];
            int[] Qs = new int[2];
            double[] torCap = new double[2];
            int[] Tn = new int[2];
            double[] DCs = new double[2];

            Q[0] = Convert.ToInt32(listSend[0][6]);
            Q[1] = Convert.ToInt32(listSend[1][6]);
            T[0] = Convert.ToInt32(listSend[0][7]);
            T[1] = Convert.ToInt32(listSend[1][7]);

            c[0] = listSend[0][12];
            c[1] = listSend[1][12];
            Qb[0] = Convert.ToInt32(listSend[0][13]);
            Qb[1] = Convert.ToInt32(listSend[1][13]);
            Qs[0] = Convert.ToInt32(listSend[0][14]);
            Qs[1] = Convert.ToInt32(listSend[1][14]);

            torCap[0] = listSend[0][15];
            torCap[1] = listSend[1][15];
            Tn[0] = Convert.ToInt32(listSend[0][16]);
            Tn[1] = Convert.ToInt32(listSend[1][16]);

            DCs[0] = listSend[0][51];
            DCs[1] = listSend[1][51];

            int[,] Ms = new int[2, 2];
            int[,] Ml = new int[2, 2];
            int[,] Mcrc = new int[2, 2];
            double[,] acrcS = new double[2, 2];
            double[,] acrcL = new double[2, 2];

            Ms[0, 0] = Convert.ToInt32(listSend[0][2]);
            Ms[0, 1] = Convert.ToInt32(listSend[0][3]);
            Ms[1, 0] = Convert.ToInt32(listSend[1][2]);
            Ms[1, 1] = Convert.ToInt32(listSend[1][3]);

            Ml[0, 0] = Convert.ToInt32(listSend[0][4]);
            Ml[0, 1] = Convert.ToInt32(listSend[0][5]);
            Ml[1, 0] = Convert.ToInt32(listSend[1][4]);
            Ml[1, 1] = Convert.ToInt32(listSend[1][5]);

            Mcrc[0, 0] = Convert.ToInt32(listSend[0][17]);
            Mcrc[0, 1] = Convert.ToInt32(listSend[0][18]);
            Mcrc[1, 0] = Convert.ToInt32(listSend[1][17]);
            Mcrc[1, 1] = Convert.ToInt32(listSend[1][18]);

            acrcS[0, 0] = listSend[0][39];
            acrcS[0, 1] = listSend[0][42];
            acrcS[1, 0] = listSend[1][39];
            acrcS[1, 1] = listSend[1][42];

            acrcL[0, 0] = listSend[0][45];
            acrcL[0, 1] = listSend[0][48];
            acrcL[1, 0] = listSend[1][45];
            acrcL[1, 1] = listSend[1][48];

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

            material_tb.Inlines.Add("Stirrup " + sRebarGrade + ", ");
            material_tb.Inlines.Add("R");
            material_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "swn" });
            material_tb.Inlines.Add(" = " + Rswn + " (MPa)");


            // Section parameter
            sect_tb.Inlines.Add("b");
            sect_tb.Inlines.Add(" × ");
            sect_tb.Inlines.Add("h");
            sect_tb.Inlines.Add(" = " + b + " × " + h + " (mm)");
            if (Tsect == true)
            {
                sect_tb.Inlines.Add(", ");
                sect_tb.Inlines.Add("b");
                sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "f" });
                sect_tb.Inlines.Add(" × ");
                sect_tb.Inlines.Add("t");
                sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "f" });
                sect_tb.Inlines.Add(" = " + bf + " × " + tf + " (mm)");
            }


            // Rebar arrangement at support - Top
            u[0] = SubExtensions.RebarProperty(b, h, acv, tw, n1top, d1top, Ln2top, Ld2top, Ln3top, Ld3top, Lds).Item2;
            u[1] = SubExtensions.RebarProperty(b, h, acv, tw, n1bot, d1bot, Ln2bot, Ld2bot, Ln3bot, Ld3bot, Lds).Item2;

            rebarTopSup_tb.Inlines.Add(n1top + "Ø" + d1top);
            if (Ln2top != 0)
            {
                rebarTopSup_tb.Inlines.Add(" + ");
                rebarTopSup_tb.Inlines.Add(Ln2top + "Ø" + Ld2top);
                if (Ln3top != 0)
                {
                    rebarTopSup_tb.Inlines.Add(new LineBreak());
                    rebarTopSup_tb.Inlines.Add(" + ");
                    rebarTopSup_tb.Inlines.Add(Ln3top + "Ø" + Ld3top);
                }
            }

            rebarTopSup_tb.Inlines.Add(new LineBreak());
            rebarTopSup_tb.Inlines.Add("µ");
            rebarTopSup_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "t" });
            rebarTopSup_tb.Inlines.Add(" = " + u[0] + " (%)");

            // Rebar arrangement at support - Bot
            rebarBotSup_tb.Inlines.Add("µ");
            rebarBotSup_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
            rebarBotSup_tb.Inlines.Add(" = " + u[1] + " (%)");

            if (Ln3bot != 0)
            {
                rebarBotSup_tb.Inlines.Add(new LineBreak());
                rebarBotSup_tb.Inlines.Add(" + ");
                rebarBotSup_tb.Inlines.Add(Ln3bot + "Ø" + Ld3bot);
            }

            rebarBotSup_tb.Inlines.Add(new LineBreak());
            rebarBotSup_tb.Inlines.Add(n1bot + "Ø" + d1bot);

            if (Ln2bot != 0)
            {
                rebarBotSup_tb.Inlines.Add(" + ");
                rebarBotSup_tb.Inlines.Add(Ln2bot + "Ø" + Ld2bot);
            }

            // Rebar arrangement at support - Stirrup
            rebarStirSup_tb.Inlines.Add("Ø" + Lds + "a" + Lsw + ", ");
            rebarStirSup_tb.Inlines.Add("n");
            rebarStirSup_tb.Inlines.Add(" = " + Lns);


            // Rebar arrangement at mid - Top
            u[2] = SubExtensions.RebarProperty(b, h, acv, tw, n1top, d1top, Mn2top, Md2top, Mn3top, Md3top, Mds).Item2;
            u[3] = SubExtensions.RebarProperty(b, h, acv, tw, n1bot, d1bot, Mn2bot, Md2bot, Mn3bot, Md3bot, Mds).Item2;

            rebarTopMid_tb.Inlines.Add(n1top + "Ø" + d1top);
            if (Mn2top != 0)
            {
                rebarTopMid_tb.Inlines.Add(" + ");
                rebarTopMid_tb.Inlines.Add(Mn2top + "Ø" + Md2top);
                if (Mn3top != 0)
                {
                    rebarTopMid_tb.Inlines.Add(new LineBreak());
                    rebarTopMid_tb.Inlines.Add(" + ");
                    rebarTopMid_tb.Inlines.Add(Mn3top + "Ø" + Md3top);
                }
            }

            rebarTopMid_tb.Inlines.Add(new LineBreak());
            rebarTopMid_tb.Inlines.Add("µ");
            rebarTopMid_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "t" });
            rebarTopMid_tb.Inlines.Add(" = " + u[2] + " (%)");

            // Rebar arrangement at mid - Bot
            rebarBotMid_tb.Inlines.Add("µ");
            rebarBotMid_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
            rebarBotMid_tb.Inlines.Add(" = " + u[3] + " (%)");

            if (Mn3bot != 0)
            {
                rebarBotMid_tb.Inlines.Add(new LineBreak());
                rebarBotMid_tb.Inlines.Add(" + ");
                rebarBotMid_tb.Inlines.Add(Mn3bot + "Ø" + Md3bot);
            }

            rebarBotMid_tb.Inlines.Add(new LineBreak());
            rebarBotMid_tb.Inlines.Add(n1bot + "Ø" + d1bot);

            if (Mn2bot != 0)
            {
                rebarBotMid_tb.Inlines.Add(" + ");
                rebarBotMid_tb.Inlines.Add(Mn2bot + "Ø" + Md2bot);
            }

            // Rebar arrangement at mid - Stirrup
            rebarStirMid_tb.Inlines.Add("Ø" + Mds + "a" + Msw + ", ");
            rebarStirMid_tb.Inlines.Add("n");
            rebarStirMid_tb.Inlines.Add(" = " + Mns);


            // Flexural check support - Top
            flexuralSupTop_tb.Inlines.Add("M");
            flexuralSupTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "t" });
            flexuralSupTop_tb.Inlines.Add(" = " + M[0, 0] + " (kNm)");
            if (M[0, 0] != 0)
            {
                flexuralSupTop_tb.Inlines.Add(new LineBreak());
                flexuralSupTop_tb.Inlines.Add("x");
                flexuralSupTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "dt" });
                flexuralSupTop_tb.Inlines.Add(" = " + naDepth[0, 0] + " (mm)");
                flexuralSupTop_tb.Inlines.Add(new LineBreak());
                flexuralSupTop_tb.Inlines.Add("M");
                flexuralSupTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nt" });
                flexuralSupTop_tb.Inlines.Add(" = " + Mn[0, 0] + " (kNm)");
                flexuralSupTop_tb.Inlines.Add(new LineBreak());
                flexuralSupTop_tb.Inlines.Add("DC");
                flexuralSupTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "t" });
                flexuralSupTop_tb.Inlines.Add(" = ");
                flexuralSupTop_tb.Inlines.Add("M");
                flexuralSupTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "t" });
                flexuralSupTop_tb.Inlines.Add(" / ");
                flexuralSupTop_tb.Inlines.Add("M");
                flexuralSupTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nt" });
                flexuralSupTop_tb.Inlines.Add(" = ");

                if (DC[0, 0] <= 1)
                {
                    flexuralSupTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(DC[0, 0]) });
                }
                else
                {
                    flexuralSupTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(DC[0, 0]) });
                }
            }

            // Flexural check support - Bot
            flexuralSupBot_tb.Inlines.Add("M");
            flexuralSupBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
            flexuralSupBot_tb.Inlines.Add(" = " + M[0, 1] + " (kNm)");
            if (M[0, 1] != 0)
            {
                flexuralSupBot_tb.Inlines.Add(new LineBreak());
                flexuralSupBot_tb.Inlines.Add("x");
                flexuralSupBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "db" });
                flexuralSupBot_tb.Inlines.Add(" = " + naDepth[0, 1] + " (mm)");
                flexuralSupBot_tb.Inlines.Add(new LineBreak());
                flexuralSupBot_tb.Inlines.Add("M");
                flexuralSupBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nb" });
                flexuralSupBot_tb.Inlines.Add(" = " + Mn[0, 1] + " (kNm)");
                flexuralSupBot_tb.Inlines.Add(new LineBreak());
                flexuralSupBot_tb.Inlines.Add("DC");
                flexuralSupBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
                flexuralSupBot_tb.Inlines.Add(" = ");
                flexuralSupBot_tb.Inlines.Add("M");
                flexuralSupBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
                flexuralSupBot_tb.Inlines.Add(" / ");
                flexuralSupBot_tb.Inlines.Add("M");
                flexuralSupBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nb" });
                flexuralSupBot_tb.Inlines.Add(" = ");

                if (DC[0, 1] <= 1)
                {
                    flexuralSupBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(DC[0, 1]) });
                }
                else
                {
                    flexuralSupBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(DC[0, 1]) });
                }
            }

            // Flexural check mid - Top
            flexuralMidTop_tb.Inlines.Add("M");
            flexuralMidTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "t" });
            flexuralMidTop_tb.Inlines.Add(" = " + M[1, 0] + " (kNm)");
            if (M[1, 0] != 0)
            {
                flexuralMidTop_tb.Inlines.Add(new LineBreak());
                flexuralMidTop_tb.Inlines.Add("x");
                flexuralMidTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "dt" });
                flexuralMidTop_tb.Inlines.Add(" = " + naDepth[1, 0] + " (mm)");
                flexuralMidTop_tb.Inlines.Add(new LineBreak());
                flexuralMidTop_tb.Inlines.Add("M");
                flexuralMidTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nt" });
                flexuralMidTop_tb.Inlines.Add(" = " + Mn[1, 0] + " (kNm)");
                flexuralMidTop_tb.Inlines.Add(new LineBreak());
                flexuralMidTop_tb.Inlines.Add("DC");
                flexuralMidTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "t" });
                flexuralMidTop_tb.Inlines.Add(" = ");
                flexuralMidTop_tb.Inlines.Add("M");
                flexuralMidTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "t" });
                flexuralMidTop_tb.Inlines.Add(" / ");
                flexuralMidTop_tb.Inlines.Add("M");
                flexuralMidTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nt" });
                flexuralMidTop_tb.Inlines.Add(" = ");

                if (DC[1, 0] <= 1)
                {
                    flexuralMidTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(DC[1, 0]) });
                }
                else
                {
                    flexuralMidTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(DC[1, 0]) });
                }
            }

            // Flexural check mid - Bot
            flexuralMidBot_tb.Inlines.Add("M");
            flexuralMidBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
            flexuralMidBot_tb.Inlines.Add(" = " + M[1, 1] + " (kNm)");
            if (M[1, 1] != 0)
            {
                flexuralMidBot_tb.Inlines.Add(new LineBreak());
                flexuralMidBot_tb.Inlines.Add("x");
                flexuralMidBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "db" });
                flexuralMidBot_tb.Inlines.Add(" = " + naDepth[1, 1] + " (mm)");
                flexuralMidBot_tb.Inlines.Add(new LineBreak());
                flexuralMidBot_tb.Inlines.Add("M");
                flexuralMidBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nb" });
                flexuralMidBot_tb.Inlines.Add(" = " + Mn[1, 1] + " (kNm)");
                flexuralMidBot_tb.Inlines.Add(new LineBreak());
                flexuralMidBot_tb.Inlines.Add("DC");
                flexuralMidBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
                flexuralMidBot_tb.Inlines.Add(" = ");
                flexuralMidBot_tb.Inlines.Add("M");
                flexuralMidBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
                flexuralMidBot_tb.Inlines.Add(" / ");
                flexuralMidBot_tb.Inlines.Add("M");
                flexuralMidBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "nb" });
                flexuralMidBot_tb.Inlines.Add(" = ");

                if (DC[1, 1] <= 1)
                {
                    flexuralMidBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(DC[1, 1]) });
                }
                else
                {
                    flexuralMidBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(DC[1, 1]) });
                }
            }


            // Shear check support
            sheartorsionSup_tb.Inlines.Add("Q");
            sheartorsionSup_tb.Inlines.Add(" = " + Q[0] + " (kN), ");
            sheartorsionSup_tb.Inlines.Add("T");
            sheartorsionSup_tb.Inlines.Add(" = " + T[0] + " (kNm)");
            if (Q[0] != 0 || T[0] != 0)
            {
                sheartorsionSup_tb.Inlines.Add(new LineBreak());
                sheartorsionSup_tb.Inlines.Add("c");
                sheartorsionSup_tb.Inlines.Add(" = " + c[0] + " (mm), ");
                sheartorsionSup_tb.Inlines.Add("Q");
                sheartorsionSup_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
                sheartorsionSup_tb.Inlines.Add(" = " + Qb[0] + " (kN), ");
                sheartorsionSup_tb.Inlines.Add("Q");
                sheartorsionSup_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "s" });
                sheartorsionSup_tb.Inlines.Add(" = " + Qs[0] + " (kN)");
                sheartorsionSup_tb.Inlines.Add(new LineBreak());
                sheartorsionSup_tb.Inlines.Add("N");
                sheartorsionSup_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "s" });
                sheartorsionSup_tb.Inlines.Add(" = " + torCap[0] + " (kN), ");
                sheartorsionSup_tb.Inlines.Add("T");
                sheartorsionSup_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "n" });
                sheartorsionSup_tb.Inlines.Add(" = " + Tn[0] + " (kNm)");
                sheartorsionSup_tb.Inlines.Add(new LineBreak());
                sheartorsionSup_tb.Inlines.Add("DC");
                sheartorsionSup_tb.Inlines.Add(" = ");
                sheartorsionSup_tb.Inlines.Add("Q / ");
                sheartorsionSup_tb.Inlines.Add("(Q");
                sheartorsionSup_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
                sheartorsionSup_tb.Inlines.Add(" + ");
                sheartorsionSup_tb.Inlines.Add("Q");
                sheartorsionSup_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "s" });
                sheartorsionSup_tb.Inlines.Add(") +");
                sheartorsionSup_tb.Inlines.Add("T / ");
                sheartorsionSup_tb.Inlines.Add("T");
                sheartorsionSup_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "n" });
                sheartorsionSup_tb.Inlines.Add(" = ");

                if (DCs[0] <= 1)
                {
                    sheartorsionSup_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(DCs[0]) });
                }
                else
                {
                    sheartorsionSup_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(DCs[0]) });
                }
            }
            
            // Shear check mid
            sheartorsionMid_tb.Inlines.Add("Q");
            sheartorsionMid_tb.Inlines.Add(" = " + Q[1] + " (kN), ");
            sheartorsionMid_tb.Inlines.Add("T");
            sheartorsionMid_tb.Inlines.Add(" = " + T[1] + " (kNm)");
            if (Q[1] != 0 || T[1] != 0)
            {
                sheartorsionMid_tb.Inlines.Add(new LineBreak());
                sheartorsionMid_tb.Inlines.Add("c");
                sheartorsionMid_tb.Inlines.Add(" = " + c[1] + " (mm), ");
                sheartorsionMid_tb.Inlines.Add("Q");
                sheartorsionMid_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
                sheartorsionMid_tb.Inlines.Add(" = " + Qb[1] + " (kN), ");
                sheartorsionMid_tb.Inlines.Add("Q");
                sheartorsionMid_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "s" });
                sheartorsionMid_tb.Inlines.Add(" = " + Qs[1] + " (kN)");
                sheartorsionMid_tb.Inlines.Add(new LineBreak());
                sheartorsionMid_tb.Inlines.Add("N");
                sheartorsionMid_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "s" });
                sheartorsionMid_tb.Inlines.Add(" = " + torCap[1] + " (kN), ");
                sheartorsionMid_tb.Inlines.Add("T");
                sheartorsionMid_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "n" });
                sheartorsionMid_tb.Inlines.Add(" = " + Tn[1] + " (kNm)");
                sheartorsionMid_tb.Inlines.Add(new LineBreak());
                sheartorsionMid_tb.Inlines.Add("DC");
                sheartorsionMid_tb.Inlines.Add(" = ");
                sheartorsionMid_tb.Inlines.Add("Q / ");
                sheartorsionMid_tb.Inlines.Add("(Q");
                sheartorsionMid_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "b" });
                sheartorsionMid_tb.Inlines.Add(" + ");
                sheartorsionMid_tb.Inlines.Add("Q");
                sheartorsionMid_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "s" });
                sheartorsionMid_tb.Inlines.Add(") +");
                sheartorsionMid_tb.Inlines.Add("T / ");
                sheartorsionMid_tb.Inlines.Add("T");
                sheartorsionMid_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "n" });
                sheartorsionMid_tb.Inlines.Add(" = ");

                if (DCs[1] <= 1)
                {
                    sheartorsionMid_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(DCs[1]) });
                }
                else
                {
                    sheartorsionMid_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(DCs[1]) });
                }
            }
            

            // Crack width support - Top
            crackSupTop_tb.Inlines.Add("M");
            crackSupTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "st" });
            crackSupTop_tb.Inlines.Add(" = " + Ms[0, 0] + " (kNm)");
            crackSupTop_tb.Inlines.Add(new LineBreak());
            crackSupTop_tb.Inlines.Add("M");
            crackSupTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "lt" });
            crackSupTop_tb.Inlines.Add(" = " + Ml[0, 0] + " (kNm)");
            if (Ms[0, 0] != 0 && Ml[0, 0] != 0)
            {
                crackSupTop_tb.Inlines.Add(new LineBreak());
                crackSupTop_tb.Inlines.Add("M");
                crackSupTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crct" });
                crackSupTop_tb.Inlines.Add(" = " + Mcrc[0, 0] + " (kNm)");
                if (acrcS[0, 0] != 0)
                {
                    crackSupTop_tb.Inlines.Add(new LineBreak());
                    crackSupTop_tb.Inlines.Add("a");
                    crackSupTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crct" });
                    crackSupTop_tb.Inlines.Add(" = ");
                    if (acrcS[0, 0] <= acrcSlim)
                    {
                        crackSupTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(acrcS[0, 0]) });
                    }
                    else
                    {
                        crackSupTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(acrcS[0, 0]) });
                    }
                    crackSupTop_tb.Inlines.Add(" (mm)");
                }
                if (acrcL[0, 0] != 0)
                {
                    crackSupTop_tb.Inlines.Add(new LineBreak());
                    crackSupTop_tb.Inlines.Add("a");
                    crackSupTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crct,1" });
                    crackSupTop_tb.Inlines.Add(" = ");
                    if (acrcL[0, 0] <= acrcLlim)
                    {
                        crackSupTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(acrcL[0, 0]) });
                    }
                    else
                    {
                        crackSupTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(acrcL[0, 0]) });
                    }
                    crackSupTop_tb.Inlines.Add(" (mm)");
                }
                if (acrcS[0, 0] == 0 && acrcL[0, 0] == 0) // Uncracked
                {
                    crackSupTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString("Uncracked") });
                }
            }


            // Crack width support - Bot
            crackSupBot_tb.Inlines.Add("M");
            crackSupBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "sb" });
            crackSupBot_tb.Inlines.Add(" = " + Ms[0, 1] + " (kNm)");
            crackSupBot_tb.Inlines.Add(new LineBreak());
            crackSupBot_tb.Inlines.Add("M");
            crackSupBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "lb" });
            crackSupBot_tb.Inlines.Add(" = " + Ml[0, 1] + " (kNm)");
            if (Ms[0, 1] != 0 && Ml[0, 1] != 0)
            {
                crackSupBot_tb.Inlines.Add(new LineBreak());
                crackSupBot_tb.Inlines.Add("M");
                crackSupBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crcb" });
                crackSupBot_tb.Inlines.Add(" = " + Mcrc[0, 1] + " (kNm)");
                if (acrcS[0, 1] != 0)
                {
                    crackSupBot_tb.Inlines.Add(new LineBreak());
                    crackSupBot_tb.Inlines.Add("a");
                    crackSupBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crcb" });
                    crackSupBot_tb.Inlines.Add(" = ");
                    if (acrcS[0, 1] <= acrcSlim)
                    {
                        crackSupBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(acrcS[0, 1]) });
                    }
                    else
                    {
                        crackSupBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(acrcS[0, 1]) });
                    }
                    crackSupBot_tb.Inlines.Add(" (mm)");
                }
                if (acrcL[0, 1] != 0)
                {
                    crackSupBot_tb.Inlines.Add(new LineBreak());
                    crackSupBot_tb.Inlines.Add("a");
                    crackSupBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crcb,1" });
                    crackSupBot_tb.Inlines.Add(" = ");
                    if (acrcL[0, 1] <= acrcLlim)
                    {
                        crackSupBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(acrcL[0, 1]) });
                    }
                    else
                    {
                        crackSupBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(acrcL[0, 1]) });
                    }
                    crackSupBot_tb.Inlines.Add(" (mm)");
                }
                if (acrcS[0, 1] == 0 && acrcL[0, 1] == 0) // Uncracked
                {
                    crackSupBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString("Uncracked") });
                }
            }

            // Crack width mid - Top
            crackMidTop_tb.Inlines.Add("M");
            crackMidTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "st" });
            crackMidTop_tb.Inlines.Add(" = " + Ms[1, 0] + " (kNm)");
            crackMidTop_tb.Inlines.Add(new LineBreak());
            crackMidTop_tb.Inlines.Add("M");
            crackMidTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "lt" });
            crackMidTop_tb.Inlines.Add(" = " + Ml[1, 0] + " (kNm)");
            if (Ms[1, 0] != 0 && Ml[1, 0] != 0)
            {
                crackMidTop_tb.Inlines.Add(new LineBreak());
                crackMidTop_tb.Inlines.Add("M");
                crackMidTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crct" });
                crackMidTop_tb.Inlines.Add(" = " + Mcrc[1, 0] + " (kNm)");
                if (acrcS[1, 0] != 0)
                {
                    crackMidTop_tb.Inlines.Add(new LineBreak());
                    crackMidTop_tb.Inlines.Add("a");
                    crackMidTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crct" });
                    crackMidTop_tb.Inlines.Add(" = ");
                    if (acrcS[1, 0] <= acrcSlim)
                    {
                        crackMidTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(acrcS[1, 0]) });
                    }
                    else
                    {
                        crackMidTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(acrcS[1, 0]) });
                    }
                    crackMidTop_tb.Inlines.Add(" (mm)");
                }
                if (acrcL[1, 0] != 0)
                {
                    crackMidTop_tb.Inlines.Add(new LineBreak());
                    crackMidTop_tb.Inlines.Add("a");
                    crackMidTop_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crct,1" });
                    crackMidTop_tb.Inlines.Add(" = ");
                    if (acrcL[1, 0] <= acrcLlim)
                    {
                        crackMidTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(acrcL[1, 0]) });
                    }
                    else
                    {
                        crackMidTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(acrcL[1, 0]) });
                    }
                    crackMidTop_tb.Inlines.Add(" (mm)");
                }
                if (acrcS[1, 0] == 0 && acrcL[1, 0] == 0) // Uncracked
                {
                    crackMidTop_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString("Uncracked") });
                }
            }

            // Crack width mid - Bot
            crackMidBot_tb.Inlines.Add("M");
            crackMidBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "sb" });
            crackMidBot_tb.Inlines.Add(" = " + Ms[1, 1] + " (kNm)");
            crackMidBot_tb.Inlines.Add(new LineBreak());
            crackMidBot_tb.Inlines.Add("M");
            crackMidBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "lb" });
            crackMidBot_tb.Inlines.Add(" = " + Ml[1, 1] + " (kNm)");
            if (Ms[1, 1] != 0 && Ml[1, 1] != 0)
            {
                crackMidBot_tb.Inlines.Add(new LineBreak());
                crackMidBot_tb.Inlines.Add("M");
                crackMidBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crcb" });
                crackMidBot_tb.Inlines.Add(" = " + Mcrc[1, 1] + " (kNm)");
                if (acrcS[1, 1] != 0)
                {
                    crackMidBot_tb.Inlines.Add(new LineBreak());
                    crackMidBot_tb.Inlines.Add("a");
                    crackMidBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crcb" });
                    crackMidBot_tb.Inlines.Add(" = ");
                    if (acrcS[1, 1] <= acrcSlim)
                    {
                        crackMidBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(acrcS[1, 1]) });
                    }
                    else
                    {
                        crackMidBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(acrcS[1, 1]) });
                    }
                    crackMidBot_tb.Inlines.Add(" (mm)");
                }
                if (acrcL[1, 1] != 0)
                {
                    crackMidBot_tb.Inlines.Add(new LineBreak());
                    crackMidBot_tb.Inlines.Add("a");
                    crackMidBot_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "crcb,1" });
                    crackMidBot_tb.Inlines.Add(" = ");
                    if (acrcL[1, 1] <= acrcLlim)
                    {
                        crackMidBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString(acrcL[1, 1]) });
                    }
                    else
                    {
                        crackMidBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Red, Text = Convert.ToString(acrcL[1, 1]) });
                    }
                    crackMidBot_tb.Inlines.Add(" (mm)");
                }
                if (acrcS[1, 1] == 0 && acrcL[1, 1] == 0) // Uncracked
                {
                    crackMidBot_tb.Inlines.Add(new Run() { Foreground = Brushes.Green, Text = Convert.ToString("Uncracked") });
                }
            }


            // Section chart
            SectL_Plot.DataContext = new ChartSectBeam(b, h, Tsect, revertTsect, tf, acv, tw,
               n1top, d1top, Ln2top, Ld2top, Ln3top, Ld3top, n1bot, d1bot, Ln2bot, Ld2bot, Ln3bot, Ld3bot, Lds);
            SectM_Plot.DataContext = new ChartSectBeam(b, h, Tsect, revertTsect, tf, acv, tw,
                n1top, d1top, Mn2top, Md2top, Mn3top, Md3top, n1bot, d1bot, Mn2bot, Md2bot, Mn3bot, Md3bot, Mds);

        }
    }
}
