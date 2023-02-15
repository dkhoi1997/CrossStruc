using CrossStruc.ConcreteBeam.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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

            int b = Convert.ToInt32(arrBeam[3]);
            int h = Convert.ToInt32(arrBeam[4]);
            string Tsect = arrBeam[5];
            int bs = Convert.ToInt32(arrBeam[7]);
            int bf = 2 * bs + b;
            int tf = Convert.ToInt32(arrBeam[8]);
            double acv = Convert.ToDouble(arrBeam[9]);
            double tw = Convert.ToDouble(arrBeam[10]);
            double acrcSlim = Convert.ToDouble(arrBeam[11]);
            double acrcLlim = Convert.ToDouble(arrBeam[12]);

            int n1top = Convert.ToInt32(arrBeam[13]);
            int d1top = Convert.ToInt32(arrBeam[14]);
            int n1bot = Convert.ToInt32(arrBeam[15]);
            int d1bot = Convert.ToInt32(arrBeam[16]);

            int Ln2top = Convert.ToInt32(arrBeam[17]);
            int Ld2top = Convert.ToInt32(arrBeam[18]);
            int Ln3top = Convert.ToInt32(arrBeam[19]);
            int Ld3top = Convert.ToInt32(arrBeam[20]);
            int Ln2bot = Convert.ToInt32(arrBeam[21]);
            int Ld2bot = Convert.ToInt32(arrBeam[22]);
            int Ln3bot = Convert.ToInt32(arrBeam[23]);
            int Ld3bot = Convert.ToInt32(arrBeam[24]);
            int Lds = Convert.ToInt32(arrBeam[25]);
            int Lns = Convert.ToInt32(arrBeam[26]);
            int Lsw = Convert.ToInt32(arrBeam[27]);

            int Mn2top = Convert.ToInt32(arrBeam[28]);
            int Md2top = Convert.ToInt32(arrBeam[29]);
            int Mn3top = Convert.ToInt32(arrBeam[30]);
            int Md3top = Convert.ToInt32(arrBeam[31]);
            int Mn2bot = Convert.ToInt32(arrBeam[32]);
            int Md2bot = Convert.ToInt32(arrBeam[33]);
            int Mn3bot = Convert.ToInt32(arrBeam[34]);
            int Md3bot = Convert.ToInt32(arrBeam[35]);
            int Mds = Convert.ToInt32(arrBeam[36]);
            int Mns = Convert.ToInt32(arrBeam[37]);
            int Msw = Convert.ToInt32(arrBeam[38]);

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
            if (Tsect == "True")
            {
                sect_tb.Inlines.Add(", ");
                sect_tb.Inlines.Add("b");
                sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "f" });
                sect_tb.Inlines.Add(" × ");
                sect_tb.Inlines.Add("t");
                sect_tb.Inlines.Add(new Run() { BaselineAlignment = BaselineAlignment.Subscript, FontSize = 10, Text = "f" });
                sect_tb.Inlines.Add(" = " + bf + " × " + tf + " (mm)");
            }

            // Section chart


        }
    }
}
