using System;
using System.Collections.Generic;
using ExtOther = CrossStruc.Extensions.Other;
using ExtMaterial = CrossStruc.Extensions.Material;

namespace CrossStruc.ConcreteColumn.Function
{
    public class Solve
    {
        public static List<(string[], List<(double[], double[,], double[,])>)> GetResultColumn(List<(string[], List<int[]>)> listCol,
            string concGrade, string lRebarGrade, string sRebarGrade,
            string shape, string mLayer, double Cx, double Cy, double Lx, double Ly, double kx, double ky, double acv,
            int nx, int ny, int dmain, int dstir, int tw, int sw, int nsx, int nsy, string combACR, bool localAxis) // Solve function for all input data
        {
            // Material
            (double Rbn, double Rbtn, double Eb) = ExtMaterial.GetConcrete(concGrade);
            (double Rsn, double Rscn, double foo, double Es) = ExtMaterial.GetRebar(lRebarGrade);
            double Rswn = ExtMaterial.GetRebar(sRebarGrade).Item3;

            // Design strength
            double Rb = Math.Round(Rbn / 1.3, 2);
            double Rbt = Math.Round(Rbtn / 1.5, 2);
            double Rs = Math.Round(Rsn / 1.15, 0);
            double Rsc = Math.Round(Rscn / 1.15, 0);
            double Rsw = Math.Round(Rswn / 1.15, 0);

            // Input parameter
            int nver = 20;
            int nhoz = 20;
            double[,] vervalue;
            double[,] hozvalue;
            int di = Math.Min(ExtOther.DeterMesh(Cx), ExtOther.DeterMesh(Cy));

            // Create ID surface
            List<int[]> listRebar = ElementPosition.Rebar(shape, mLayer, tw, Cx, Cy, nx, ny, acv, dmain, dstir);
            List<double[]> listConcrete = ElementPosition.Concrete(shape, Cx, Cy, di);

            if (shape == "Rec")
            {
                (vervalue, hozvalue) = IDSurface.RecSect(listConcrete, listRebar, Cx, Cy, Rb, Eb, Rs, Rsc, Es, nver, nhoz);
            }
            else
            {
                (vervalue, hozvalue) = IDSurface.CirSect(listConcrete, listRebar, Cx, Rb, Eb, Rs, Rsc, Es, nver, nhoz);
            }

            // Calc all
            List<(string[], List<(double[], double[,], double[,])>)> listResult = new List<(string[], List<(double[], double[,], double[,])>)>();
            foreach ((string[], List<int[]>) temp in listCol)
            {
                List<int[]> cd = temp.Item2;
                List<(double[], double[,], double[,])> listTemp = new List<(double[], double[,], double[,])>();
                for (int i = 0; i < cd.Count; i++)
                {
                    int comb = cd[i][0];
                    int P = cd[i][1];
                    int Qx; int Qy; int Mx; int My;
                    if (localAxis == true)
                    {
                        Qx = Math.Abs(cd[i][2]);
                        Qy = Math.Abs(cd[i][3]);
                        Mx = Math.Abs(cd[i][6]);
                        My = Math.Abs(cd[i][5]);
                    }
                    else
                    {
                        Qx = Math.Abs(cd[i][3]);
                        Qy = Math.Abs(cd[i][2]);
                        Mx = Math.Abs(cd[i][5]);
                        My = Math.Abs(cd[i][6]);
                    }

                    int Mxup; int Myup;
                    double e0x; double e0y;
                    double etax; double etay;
                    double Ncrx; double Ncry;
                    if (shape == "Rec")
                    {
                        (e0x, e0y, Ncrx, Ncry, etax, etay, Mxup, Myup) = UpperMoment.RecSect(P, Mx, My, Cx, Cy, Lx, Ly, Eb, kx, ky);
                    }
                    else
                    {
                        (e0x, e0y, Ncrx, Ncry, etax, etay, Mxup, Myup) = UpperMoment.CirSect(P, Mx, My, Cx, Lx, Ly, Eb, kx, ky);
                    }
                    // Flexural
                    int Muxy = Convert.ToInt32(Math.Sqrt(Math.Pow(Mxup, 2) + Math.Pow(Myup, 2)));
                    (int Pnxy, int Mnxy, double DC, double[,] vercheck, double[,] hozcheck) =
                        ULSCheck.InteractionDiagramCheck(P, Mxup, Myup, nver, nhoz, vervalue, hozvalue);

                    // Shear
                    (double sigma, double phin, double cx, double cy, double Qbx, double Qsx, double Qby, double Qsy, double DCs) =
                        ULSCheck.ShearCheck(shape, Cx, Cy, P, Qx, Qy, dmain, dstir, nsx, nsy, sw, acv, Rb, Rbt, Rsw);

                    // ACR
                    double ved = 0;
                    double fcd = 0;
                    if (string.IsNullOrEmpty(combACR) == false)
                    {
                        List<int> listComb = ExtOther.ExtractCombRobot(combACR);
                        for (int k = 0; k < listComb.Count; k++)
                        {
                            if (Convert.ToString(cd[i][0]) == Convert.ToString(listComb[k]))
                            {
                                (fcd, ved) = ULSCheck.AxialCompressionCheck(shape, Cx, Cy, P, Rb);
                                break;
                            }
                        }
                    }

                    // Paste result into array
                    double[] calData = new double[29];
                    calData[0] = comb;
                    calData[1] = P;
                    calData[2] = Qx;
                    calData[3] = Qy;
                    calData[4] = Mx;
                    calData[5] = My;
                    calData[6] = e0x;
                    calData[7] = e0y;
                    calData[8] = Ncrx;
                    calData[9] = Ncry;
                    calData[10] = etax;
                    calData[11] = etay;
                    calData[12] = Mxup;
                    calData[13] = Myup;
                    calData[14] = Muxy;
                    calData[15] = Pnxy;
                    calData[16] = Mnxy;
                    calData[17] = DC;
                    calData[18] = sigma;
                    calData[19] = phin;
                    calData[20] = cx;
                    calData[21] = cy;
                    calData[22] = Qbx;
                    calData[23] = Qsx;
                    calData[24] = Qby;
                    calData[25] = Qsy;
                    calData[26] = DCs;
                    calData[27] = fcd;
                    calData[28] = ved;

                    listTemp.Add((calData, vercheck, hozcheck));
                }
                listResult.Add((temp.Item1, listTemp));
            }
            return listResult;
        }
    }
}
