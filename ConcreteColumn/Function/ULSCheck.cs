using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossStruc.ConcreteColumn.Function
{
    public class ULSCheck
    {
        public static (int, int, double, double[,], double[,]) InteractionDiagramCheck(int P, int Mxup, int Myup, int nver, int nhoz,
            double[,] vervalue, double[,] hozvalue) // Flexual check use interaction diagram
        {
            int k; int i; int j; double outMx; double outMy;
            double deno; double ki;
            // P-Mxy result
            double[,] outputPMxy = new double[nhoz, 2];
            k = 0;
            for (i = 0; i < 4 * nhoz; i += 4)
            {
                for (j = 0; j < nver - 1; j++)
                {
                    deno = Myup * (hozvalue[j + 1, i] - hozvalue[j, i]) - Mxup * (hozvalue[j + 1, i + 1] - hozvalue[j, i + 1]);
                    if (deno == 0)
                    {
                        ki = 0;
                        goto nextstep;
                    }
                    ki = Math.Round((Mxup * hozvalue[j, i + 1] - Myup * hozvalue[j, i]) / deno, 2);
                nextstep:
                    if ((0 <= ki) && (ki <= 1))
                    {
                        outputPMxy[k, 0] = hozvalue[j, i + 2];
                        outMx = hozvalue[j, i] + (hozvalue[j + 1, i] - hozvalue[j, i]) * ki;
                        outMy = hozvalue[j, i + 1] + (hozvalue[j + 1, i + 1] - hozvalue[j, i + 1]) * ki;
                        outputPMxy[k, 1] = Math.Sqrt(Math.Pow(outMx, 2) + Math.Pow(outMy, 2));
                        k = k + 1;
                        break;
                    }
                }
            }
            // Mx-My result
            double[,] outputMxMy = new double[nver, 2];
            k = 0;
            for (i = 0; i < 4 * nver; i += 4)
            {
                for (j = 0; j < nhoz - 1; j++)
                {
                    ki = Math.Round((P - vervalue[j, i + 2]) / (vervalue[j + 1, i + 2] - vervalue[j, i + 2]), 5);
                    if ((0 <= ki) && (ki <= 1))
                    {
                        outputMxMy[k, 0] = Math.Round(ki * (vervalue[j + 1, i] - vervalue[j, i]) + vervalue[j, i], 2);
                        outputMxMy[k, 1] = Math.Round(ki * (vervalue[j + 1, i + 1] - vervalue[j, i + 1]) + vervalue[j, i + 1], 2);
                        k = k + 1;
                        break;
                    }
                }
            }
            // Final result
            double Mxy = Math.Sqrt(Math.Pow(Mxup, 2) + Math.Pow(Myup, 2));
            int Mnxy = 0; int Pnxy = 0; double RC; double RD; double DC = 0;
            for (i = 0; i < nhoz - 1; i++)
            {
                ki = (Mxy * outputPMxy[i, 0] - P * outputPMxy[i, 1]) / (P * (outputPMxy[i + 1, 1] - outputPMxy[i, 1]) - Mxy * (outputPMxy[i + 1, 0] - outputPMxy[i, 0]));
                if ((0 <= ki) && (ki <= 1))
                {
                    Mnxy = Convert.ToInt32(outputPMxy[i, 1] + ki * (outputPMxy[i + 1, 1] - outputPMxy[i, 1]));
                    Pnxy = Convert.ToInt32(outputPMxy[i, 0] + ki * (outputPMxy[i + 1, 0] - outputPMxy[i, 0]));
                    RC = Math.Sqrt(Math.Pow(Mnxy, 2) + Math.Pow(Pnxy, 2));
                    RD = Math.Sqrt(Math.Pow(Mxy, 2) + Math.Pow(P, 2));
                    DC = Math.Round(RD / RC, 2);
                    break;
                }
            }
            return (Pnxy, Mnxy, DC, outputPMxy, outputMxMy);
        }

        public static (double, double, double, double, double, double, double, double, double) ShearCheck(string shape, double Cx, double Cy, int P, int Qx, int Qy, int dmain, int dstir,
            int nsx, int nsy, int sw, double acv, double Rb, double Rbt, double Rsw) // Shear force check
        {
            double att; double DC; 
            double sigma; double phin;
            double cx; double cy;
            double Qbx; double Qsx;
            double Qby; double Qsy;

            // Unit converted
            // Cx, Cy - (mm)
            // Qx, Qy - (kN)
            // dmain, dstir, nstir, sw, acv - (mm)
            // Rb, Rbt, Rsw - (MPa)

            if (shape == "Cir")
            {
                double equalArea = Math.PI * Math.Pow(Convert.ToDouble(Cx), 2) / 4;
                Cx = Math.Sqrt(equalArea);
                Cy = Math.Sqrt(equalArea);
            }

            att = (acv + dstir + dmain / 2);
            sigma = Math.Round(P * 1000 / (1.3 * Cx * Cy), 2); // (Average stress force whole section included rebar - from Russia book)
            double sigma_abs = Math.Abs(sigma);
            if (P > 0) // Compression stress
            {
                if (sigma_abs <= 0.25 * Rb)
                {
                    phin = 1 + sigma_abs / Rb;
                }
                else if ((0.25 * Rb < sigma_abs) && (sigma_abs <= 0.75 * Rb))
                {
                    phin = 1.25;
                }
                else if ((0.75 * Rb < sigma_abs) && (sigma_abs <= Rb))
                {
                    phin = 5 * (1 - sigma_abs / Rb);
                }
                else
                {
                    goto endstep;
                }
            }
            else // Tension stress
            {
                if (sigma_abs <= Rbt)
                {
                    phin = 1 - sigma_abs / (2 * Rbt);
                }
                else
                {
                    goto endstep;
                }
            }
            if ((0.3 * phin * Rb * Cy * (Cx - att) / 1000 < Qx) || (0.3 * phin * Rb * Cx * (Cy - att) / 1000 < Qy))
            {
                goto endstep;
            }
            goto nextstep;
        endstep:
            {
                phin = 0;
                cx = 0;
                cy = 0;
                Qbx = 0;
                Qsx = 0;
                Qby = 0;
                Qsy = 0;
                DC = ushort.MaxValue;
                return (sigma, phin, cx, cy, Qbx, Qsx, Qby, Qsy, DC);
            }
        nextstep:
            {
                double Qswx; double Qswy;
                Qswx = Rsw * nsx * Math.PI * Math.Pow(dstir, 2) / 4 / sw;
                Qswy = Rsw * nsy * Math.PI * Math.Pow(dstir, 2) / 4 / sw;
                cx = Math.Sqrt(phin * 1.5 * Rbt * Cy * Math.Pow(Cx - att, 2) / (0.75 * Qswx));
                cy = Math.Sqrt(phin * 1.5 * Rbt * Cx * Math.Pow(Cy - att, 2) / (0.75 * Qswy));
                if (cx < Cx - att)
                {
                    cx = Cx - att;
                }
                if (cx > 2 * (Cx - att))
                {
                    cx = 2 * (Cx - att);
                }
                if (cy < Cy - att)
                {
                    cy = Cy - att;
                }
                if (cy > 2 * (Cy - att))
                {
                    cy = 2 * (Cy - att);
                }
                Qbx = phin * 1.5 * Rbt * Cy * Math.Pow(Cx - att, 2) / cx;
                Qby = phin * 1.5 * Rbt * Cx * Math.Pow(Cy - att, 2) / cy;

                if (Qbx < 0.5 * Rbt * Cy * (Cx - att))
                {
                    Qbx = 0.5 * Rbt * Cy * (Cx - att);
                }
                if (Qbx > 2.5 * Rbt * Cy * (Cx - att))
                {
                    Qbx = 2.5 * Rbt * Cy * (Cx - att);
                }
                if (Qby < 0.5 * Rbt * Cx * (Cy - att))
                {
                    Qby = 0.5 * Rbt * Cx * (Cy - att);
                }
                if (Qby > 2.5 * Rbt * Cx * (Cy - att))
                {
                    Qby = 2.5 * Rbt * Cx * (Cy - att);
                }
                Qsx = 0.75 * Qswx * cx;
                Qsy = 0.75 * Qswy * cy;

                Qbx = Math.Round(Qbx / 1000, 0);
                Qby = Math.Round(Qby / 1000, 0);
                Qsx = Math.Round(Qsx / 1000, 0);
                Qsy = Math.Round(Qsy / 1000, 0);

                double Qnx = Qbx + Qsx;
                double Qny = Qby + Qsy;

                DC = Math.Round(Math.Pow(Math.Abs(Qx) / Qnx,2) + Math.Pow(Math.Abs(Qy) / Qny,2), 2); // Ellipse function interaction (since TCVN not provide, use JSCE for reference)
                phin = Math.Round(phin, 2);
                return (sigma, phin, cx, cy, Qbx, Qsx, Qby, Qsy, DC);
            }
        }

        public static (double, double) AxialCompressionCheck(string shape, double Cx, double Cy, int P, double Rb) // Axial compression check
        {
            double ved = 0;
            double fck = 1.53 * Rb - 1.67;
            double fcd = Math.Round(fck / 1.2, 2);

            if (shape == "Cir")
            {
                double equalArea = Math.PI * Math.Pow(Convert.ToDouble(Cx), 2) / 4;
                Cx = Math.Sqrt(equalArea);
                Cy = Math.Sqrt(equalArea);
            }

            if (P > 0) ved = Math.Round(P * 1000 / (Cx * Cy * fcd), 2);

            return (fcd, ved);
        }
    }
}
