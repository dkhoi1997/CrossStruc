﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using ExtMaterial = CrossStruc.Extensions.Material;

namespace CrossStruc.ConcreteBeam.Function
{
    public class Solve
    {
        public static List<(string[], List<double[]>)> GetResultBeam(List<(string[], List<int[]>)> listbeamforce,
            string concgrade, string lRebargrade, string sRebargrade, string hmClass, double b, double h, double hs, double bs,
            bool Tsect, bool Tsectrevert, bool compressbar, double acv, double tw, double acrcSlim, double acrcLlim,
            int[] arrrebar, int[] arrLrebar, int[] arrMrebar) // Solve function for all input data
        {
            // Material
            (double Rbn, double Rbtn, double Eb) = ExtMaterial.GetConcrete(concgrade);
            (double Rsn, double Rscn, double foo, double Es) = ExtMaterial.GetRebar(lRebargrade);
            double Rswn = ExtMaterial.GetRebar(sRebargrade).Item3;

            // Design strength
            double Rb = Rbn / 1.3;
            double Rbt = Rbtn / 1.3;
            double Rs = Rsn / 1.15;
            double Rsc = Rscn / 1.15;
            double Rsw = Rswn / 1.15;

            // Strain for SLS check
            (double epb0, double epb2, double epb1red, double epbt0, double epbt2, double epbt1red) = ExtMaterial.GetConcreteStrain(hmClass);

            // Rebar parameter
            int n1top = arrrebar[0];
            int d1top = arrrebar[1];
            int n1bot = arrrebar[2];
            int d1bot = arrrebar[3];

            int Ln2top = arrLrebar[0];
            int Ld2top = arrLrebar[1];
            int Ln3top = arrLrebar[2];
            int Ld3top = arrLrebar[3];
            int Ln2bot = arrLrebar[4];
            int Ld2bot = arrLrebar[5];
            int Ln3bot = arrLrebar[6];
            int Ld3bot = arrLrebar[7];
            int Lds = arrLrebar[8];

            int Mn2top = arrMrebar[0];
            int Md2top = arrMrebar[1];
            int Mn3top = arrMrebar[2];
            int Md3top = arrMrebar[3];
            int Mn2bot = arrMrebar[4];
            int Md2bot = arrMrebar[5];
            int Mn3bot = arrMrebar[6];
            int Md3bot = arrMrebar[7];
            int Mds = arrMrebar[8];

            int[] dstir = { arrLrebar[8], arrMrebar[8] };
            int[] nstir = { arrLrebar[9], arrMrebar[9] };
            int[] swstir = { arrLrebar[10], arrMrebar[10] };

            // Mesh section and rebar coordinate
            (List<int[]> listLrebarTop, List<int[]> listLrebarBot) =
                ElementPosition.Rebar(b, h, acv, tw, Lds, n1top, d1top, Ln2top, Ld2top, Ln3top, Ld3top, n1bot, d1bot, Ln2bot, Ld2bot, Ln3bot, Ld3bot);

            (List<int[]> listMrebarTop, List<int[]> listMrebarBot) =
                ElementPosition.Rebar(b, h, acv, tw, Mds, n1top, d1top, Mn2top, Md2top, Mn3top, Md3top, n1bot, d1bot, Mn2bot, Md2bot, Mn3bot, Md3bot);

            List<double[]> listconc =
                ElementPosition.Concrete(b, h, hs, bs, Tsect, Tsectrevert);

            int tolerance = 5;

            // ULS output
            double[,] naDepth = new double[2, 2];
            double[,] totalP = new double[2, 2];
            double[,] totalM = new double[2, 2];
            double[] sideAs = new double[2];

            // SLS output
            double[,] Mcrc = new double[2, 2];
            double[,] equivDia = new double[2, 2];

            double[,] tensDepthS = new double[2, 2];
            double[,] tensAreaConcS = new double[2, 2];
            double[,] tensAreaRebarS = new double[2, 2];
            double[,] disTensToCompS = new double[2, 2];

            double[,] tensDepthL = new double[2, 2];
            double[,] tensAreaConcL = new double[2, 2];
            double[,] tensAreaRebarL = new double[2, 2];
            double[,] disTensToCompL = new double[2, 2];

            // ULS bending calc
            (naDepth[0, 0], totalP[0, 0], totalM[0, 0]) = ULSCheck.BeamCapacity(b, h, tolerance, listconc, listLrebarTop, listLrebarBot, true, compressbar, Rb, Eb, Rs, Rsc, Es);
            (naDepth[0, 1], totalP[0, 1], totalM[0, 1]) = ULSCheck.BeamCapacity(b, h, tolerance, listconc, listLrebarTop, listLrebarBot, false, compressbar, Rb, Eb, Rs, Rsc, Es);
            (naDepth[1, 0], totalP[1, 0], totalM[1, 0]) = ULSCheck.BeamCapacity(b, h, tolerance, listconc, listMrebarTop, listMrebarBot, true, compressbar, Rb, Eb, Rs, Rsc, Es);
            (naDepth[1, 1], totalP[1, 1], totalM[1, 1]) = ULSCheck.BeamCapacity(b, h, tolerance, listconc, listMrebarTop, listMrebarBot, false, compressbar, Rb, Eb, Rs, Rsc, Es);
            sideAs[0] = SubExtensions.SideAsForTorsional(listLrebarTop, listLrebarBot);
            sideAs[1] = SubExtensions.SideAsForTorsional(listMrebarTop, listMrebarBot);

            // SLS Mcrc calc
            Mcrc[0, 0] = SLSCheck.BeamMcrc(b, h, tolerance, listconc, listLrebarTop, listLrebarBot, true, compressbar, Rbn, Rbtn, Eb, Rs, Rsc, Es);
            Mcrc[0, 1] = SLSCheck.BeamMcrc(b, h, tolerance, listconc, listLrebarTop, listLrebarBot, false, compressbar, Rbn, Rbtn, Eb, Rs, Rsc, Es);
            Mcrc[1, 0] = SLSCheck.BeamMcrc(b, h, tolerance, listconc, listMrebarTop, listMrebarBot, true, compressbar, Rbn, Rbtn, Eb, Rs, Rsc, Es);
            Mcrc[1, 1] = SLSCheck.BeamMcrc(b, h, tolerance, listconc, listMrebarTop, listMrebarBot, false, compressbar, Rbn, Rbtn, Eb, Rs, Rsc, Es);

            // SLS crack width parameter for short-term loading
            equivDia[0, 0] = SubExtensions.EquivalentDiaRebar(listLrebarTop, listLrebarBot, true, compressbar);
            equivDia[0, 1] = SubExtensions.EquivalentDiaRebar(listLrebarTop, listLrebarBot, false, compressbar);
            equivDia[1, 0] = SubExtensions.EquivalentDiaRebar(listMrebarTop, listMrebarBot, true, compressbar);
            equivDia[1, 1] = SubExtensions.EquivalentDiaRebar(listMrebarTop, listMrebarBot, false, compressbar);

            (tensDepthS[0, 0], tensAreaConcS[0, 0], tensAreaRebarS[0, 0], disTensToCompS[0, 0]) =
                SLSCheck.CrackWidthParameter(b, h, tolerance, listconc, listLrebarTop, listLrebarBot, true, compressbar, 0.0015, 0.00008, 0.00015, Rbn, Rbtn, Eb, Rs, Rsc, Es);
            (tensDepthS[0, 1], tensAreaConcS[0, 1], tensAreaRebarS[0, 1], disTensToCompS[0, 1]) =
                SLSCheck.CrackWidthParameter(b, h, tolerance, listconc, listLrebarTop, listLrebarBot, false, compressbar, 0.0015, 0.00008, 0.00015, Rbn, Rbtn, Eb, Rs, Rsc, Es);
            (tensDepthS[1, 0], tensAreaConcS[1, 0], tensAreaRebarS[1, 0], disTensToCompS[1, 0]) =
                SLSCheck.CrackWidthParameter(b, h, tolerance, listconc, listMrebarTop, listMrebarBot, true, compressbar, 0.0015, 0.00008, 0.00015, Rbn, Rbtn, Eb, Rs, Rsc, Es);
            (tensDepthS[1, 1], tensAreaConcS[1, 1], tensAreaRebarS[1, 1], disTensToCompS[1, 1]) =
                SLSCheck.CrackWidthParameter(b, h, tolerance, listconc, listMrebarTop, listMrebarBot, false, compressbar, 0.0015, 0.00008, 0.00015, Rbn, Rbtn, Eb, Rs, Rsc, Es);

            // SLS crack width parameter for long-term loading
            (tensDepthL[0, 0], tensAreaConcL[0, 0], tensAreaRebarL[0, 0], disTensToCompL[0, 0]) =
                SLSCheck.CrackWidthParameter(b, h, tolerance, listconc, listLrebarTop, listLrebarBot, true, compressbar, epb1red, epbt1red, epbt2, Rbn, Rbtn, Eb, Rs, Rsc, Es);
            (tensDepthL[0, 1], tensAreaConcL[0, 1], tensAreaRebarL[0, 1], disTensToCompL[0, 1]) =
                SLSCheck.CrackWidthParameter(b, h, tolerance, listconc, listLrebarTop, listLrebarBot, false, compressbar, epb1red, epbt1red, epbt2, Rbn, Rbtn, Eb, Rs, Rsc, Es);
            (tensDepthL[1, 0], tensAreaConcL[1, 0], tensAreaRebarL[1, 0], disTensToCompL[1, 0]) =
                SLSCheck.CrackWidthParameter(b, h, tolerance, listconc, listMrebarTop, listMrebarBot, true, compressbar, epb1red, epbt1red, epbt2, Rbn, Rbtn, Eb, Rs, Rsc, Es);
            (tensDepthL[1, 1], tensAreaConcL[1, 1], tensAreaRebarL[1, 1], disTensToCompL[1, 1]) =
                SLSCheck.CrackWidthParameter(b, h, tolerance, listconc, listMrebarTop, listMrebarBot, false, compressbar, epb1red, epbt1red, epbt2, Rbn, Rbtn, Eb, Rs, Rsc, Es);

            bool breakOut = false;

            for (int i = 0; i < 2; i++)
            {
                if (naDepth[i, 0] == 0 || naDepth[i, 1] == 0)
                {
                    breakOut = true;
                    break;

                }
            }

            List<(string[], List<double[]>)> listResult = new List<(string[], List<double[]>)>();

            if (breakOut == true)
            {
                MessageBox.Show("- Equilibrium force cannot be established, may be too much or less rebar"
                + Environment.NewLine + "- Bending capacity [Mu] will set at 1");
                return listResult;
            }

            foreach ((string[], List<int[]>) item in listbeamforce)
            {
                List<double[]> listTemp = new List<double[]>();
                // Note for i varible => 0 - Support, 1 - Mid
                for (int i = 0; i < item.Item2.Count; i++)
                {
                    double[] temp = new double[52];
                    int Mtop = item.Item2[i][0];
                    int Mbot = item.Item2[i][1];
                    int MtopS = item.Item2[i][2];
                    int MbotS = item.Item2[i][3];
                    int MtopL = Convert.ToInt32(0.8 * MtopS);
                    int MbotL = Convert.ToInt32(0.8 * MbotS);
                    int Q = Math.Abs(item.Item2[i][4]);
                    int T = Math.Abs(item.Item2[i][5]);

                    double bendingRatioTop = Math.Round(Mtop / Math.Min(totalM[i, 0], -1), 2);
                    double bendingRatioBot = Math.Round(Mbot / Math.Max(totalM[i, 1], 1), 2);
                    (double Qb, double Qs, double Qn, double shearRatio) = ULSCheck.ShearCheck(b, h, Q, dstir[i], nstir[i], swstir[i], Rb, Rbt, Rsw);
                    (double Tn, double torRatio) = ULSCheck.TorsionalCheck(b, h, T, dstir[i], nstir[i], swstir[i], sideAs[i], Rb, Rs, Rsw);
                    
                    // ULS
                    temp[0] = Mtop; // Mtop
                    temp[1] = Mbot; // Mbot
                    temp[2] = MtopS; // MtopSLS
                    temp[3] = MbotS; // MbotSLS
                    temp[4] = MtopL; // Qshear
                    temp[5] = MbotL; // Ttorsion
                    temp[6] = naDepth[i, 0]; // Neutral axis depth top
                    temp[7] = naDepth[i, 1]; // Neutral axis depth bot
                    temp[8] = totalM[i, 0]; // Bending capacity top
                    temp[9] = totalM[i, 1]; // Bending capacity bot
                    temp[10] = Qn; // Section shear capacity
                    temp[11] = Tn; // Section torsinal capacity

                    // SLS
                    temp[12] = Mcrc[i, 0]; // Crack moment top
                    temp[13] = Mcrc[i, 1]; // Crack moment bot
                    temp[14] = equivDia[i, 0];// Equivalent rebar top
                    temp[15] = equivDia[i, 1];// Equivalent rebar bot
                    temp[16] = tensDepthS[i, 0]; // Tension depth top (short-term)
                    temp[17] = tensDepthS[i, 1]; // Tension depth bot (short-term)
                    temp[18] = tensAreaConcS[i, 0];// Tension concrete area top (short-term)
                    temp[19] = tensAreaConcS[i, 1];// Tension concrete area bot (short-term)
                    temp[20] = tensAreaRebarS[i, 0];// Tension rebar area top (short-term)
                    temp[21] = tensAreaRebarS[i, 1];// Tension rebar area bot (short-term)
                    temp[22] = disTensToCompS[i, 0];// Distance from tension rebar to compression centroid top (short-term)
                    temp[23] = disTensToCompS[i, 1];// Distance from tension rebar to compression centroid bot (short-term)
                    temp[24] = tensDepthL[i, 0]; // Tension depth top (long-term)
                    temp[25] = tensDepthL[i, 1]; // Tension depth bot (long-term)
                    temp[26] = tensAreaConcL[i, 0];// Tension concrete area top (long-term)
                    temp[27] = tensAreaConcL[i, 1];// Tension concrete area bot (long-term)
                    temp[28] = tensAreaRebarL[i, 0];// Tension rebar area top (long-term)
                    temp[29] = tensAreaRebarL[i, 1];// Tension rebar area bot (long-term)
                    temp[30] = disTensToCompL[i, 0];// Distance from tension rebar to compression centroid top (long-term)
                    temp[31] = disTensToCompL[i, 1];// Distance from tension rebar to compression centroid bot (long-term)

                    (double LcrcTopS, double psiTopS, double sigrTopS) = SLSCheck.SigTensionCrack(Mcrc[i, 0], MtopS, Rsn, tensAreaConcS[i, 0], tensAreaRebarS[i, 0], equivDia[i, 0], disTensToCompS[i, 0]);
                    (double LcrcBotS, double psiBotS, double sigrBotS) = SLSCheck.SigTensionCrack(Mcrc[i, 1], MbotS, Rsn, tensAreaConcS[i, 1], tensAreaRebarS[i, 1], equivDia[i, 1], disTensToCompS[i, 1]);
                    (double LcrcTopL, double psiTopL, double sigrTopL) = SLSCheck.SigTensionCrack(Mcrc[i, 0], MtopL, Rsn, tensAreaConcL[i, 0], tensAreaRebarL[i, 0], equivDia[i, 0], disTensToCompL[i, 0]);
                    (double LcrcBotL, double psiBotL, double sigrBotL) = SLSCheck.SigTensionCrack(Mcrc[i, 1], MbotL, Rsn, tensAreaConcL[i, 1], tensAreaRebarL[i, 1], equivDia[i, 1], disTensToCompL[i, 1]);

                    // Calc crack width
                    (double acrcTopS, double acrcTopL) = SLSCheck.CrackWidth(LcrcTopS, psiTopS, sigrTopS, LcrcTopL, psiTopL, sigrTopL, Es);
                    (double acrcBotS, double acrcBotL) = SLSCheck.CrackWidth(LcrcBotS, psiBotS, sigrBotS, LcrcBotL, psiBotL, sigrBotL, Es);

                    temp[32] = sigrTopS;
                    temp[33] = LcrcTopS;
                    temp[34] = acrcTopS;
                    temp[35] = sigrBotS;
                    temp[36] = LcrcBotS;
                    temp[37] = acrcBotS;

                    temp[38] = sigrTopL;
                    temp[39] = LcrcTopL;
                    temp[40] = acrcTopL;
                    temp[41] = sigrBotL;
                    temp[42] = LcrcBotL;
                    temp[43] = acrcBotL;

                    double acrcTopSRatio = Math.Round(acrcTopS / acrcSlim, 2);
                    double acrcBotSRatio = Math.Round(acrcBotS / acrcSlim, 2);

                    double acrcTopLRatio = Math.Round(acrcTopL / acrcLlim, 2);
                    double acrcBotLRatio = Math.Round(acrcBotL / acrcLlim, 2);

                    // Ratio check
                    temp[44] = bendingRatioTop;
                    temp[45] = bendingRatioBot;
                    temp[46] = shearRatio;
                    temp[47] = torRatio;
                    temp[48] = acrcTopSRatio;
                    temp[49] = acrcBotSRatio;
                    temp[50] = acrcTopLRatio;
                    temp[51] = acrcBotLRatio;

                    listTemp.Add(temp);
                }
                listResult.Add((item.Item1, listTemp));
            }

            return listResult;
        } 
    }
}