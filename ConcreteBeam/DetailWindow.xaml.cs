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
            double acrcSlim = Convert.ToDouble(arrBeam[12]);
            double acrcLlim = Convert.ToDouble(arrBeam[13]);

            int n1top = Convert.ToInt32(arrBeam[14]);
            int d1top = Convert.ToInt32(arrBeam[15]);
            int n1bot = Convert.ToInt32(arrBeam[16]);
            int d1bot = Convert.ToInt32(arrBeam[17]);

            int Ln2top = Convert.ToInt32(arrBeam[18]);
            int Ld2top = Convert.ToInt32(arrBeam[19]);
            int Ln3top = Convert.ToInt32(arrBeam[20]);
            int Ld3top = Convert.ToInt32(arrBeam[21]);
            int Ln2bot = Convert.ToInt32(arrBeam[22]);
            int Ld2bot = Convert.ToInt32(arrBeam[23]);
            int Ln3bot = Convert.ToInt32(arrBeam[24]);
            int Ld3bot = Convert.ToInt32(arrBeam[25]);
            int Lds = Convert.ToInt32(arrBeam[26]);
            int Lns = Convert.ToInt32(arrBeam[27]);
            int Lsw = Convert.ToInt32(arrBeam[28]);

            int Mn2top = Convert.ToInt32(arrBeam[29]);
            int Md2top = Convert.ToInt32(arrBeam[30]);
            int Mn3top = Convert.ToInt32(arrBeam[31]);
            int Md3top = Convert.ToInt32(arrBeam[32]);
            int Mn2bot = Convert.ToInt32(arrBeam[33]);
            int Md2bot = Convert.ToInt32(arrBeam[34]);
            int Mn3bot = Convert.ToInt32(arrBeam[35]);
            int Md3bot = Convert.ToInt32(arrBeam[36]);
            int Mds = Convert.ToInt32(arrBeam[37]);
            int Mns = Convert.ToInt32(arrBeam[38]);
            int Msw = Convert.ToInt32(arrBeam[39]);

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




            // Flexural check



            // Section chart
            SectL_Plot.DataContext = new ChartSectBeam(b, h, Tsect, revertTsect, tf, acv, tw,
               n1top, d1top, Ln2top, Ld2top, Ln3top, Ld3top, n1bot, d1bot, Ln2bot, Ld2bot, Ln3bot, Ld3bot, Lds);
            SectM_Plot.DataContext = new ChartSectBeam(b, h, Tsect, revertTsect, tf, acv, tw,
                n1top, d1top, Mn2top, Md2top, Mn3top, Md3top, n1bot, d1bot, Mn2bot, Md2bot, Mn3bot, Md3bot, Mds);

        }
    }
}
