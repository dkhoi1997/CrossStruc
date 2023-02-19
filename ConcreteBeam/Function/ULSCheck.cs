using CrossStruc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CrossStruc.ConcreteBeam.Function
{
    public static class ULSCheck
    {
        public static (double, double, double) BeamCapacity(double b, double h, int tolerance, List<double[]> listConc, List<int[]> listRebarT, List<int[]> listRebarB,
            bool revertScan, bool compressBar, double Rb, double Eb, double Rs, double Rsc, double Es) // ULS beam calc function
        {
            // Sign note, for bottom bending (+), for top bending (-))
            // NA will scan from top -> bot or revert, default is top -> bot

            List<int[]> listRebar = listRebarB;

            // Initial parameter
            int k = 1;

            if (revertScan == true)
            {
                k = -1;
                listRebar = listRebarT;
            }

            if (compressBar == true)
            {
                listRebar = listRebarB.Concat(listRebarT).ToList();
            }

            int nstep = Convert.ToInt32(2 * h + 1);
            int naresult = 0;
            int Presult = 0;
            int Mresult = 0;

            double epcb2 = 0.0035;

            double c; double nadepth; double kj; double kjj;
            double dci; double epci; double sigci; double Pci; double Mci;
            double dsi; double epsi; double sigsi; double Psi; double Msi;
            double cmin = -k * h / 2;
            double cmax = k * h / 2;

            double yM = k * (h / 2 + 100);

            double sumPc; double sumPs; double sumMc; double sumMs;

            // NA equation for beam 0x + 1y + c = 0

            for (int i = 0; i < nstep; i++)
            {
                sumPc = 0;
                sumPs = 0;
                sumMc = 0;
                sumMs = 0;
                c = cmin + 2 * Math.Abs(cmin) / (nstep - 1) * i * k;
                nadepth = Math.Abs(k * h / 2 + c);
                if ((Math.Round(nadepth, 2) == 0) || (Math.Round(c - cmax, 2) == 0))
                {
                    nadepth = 1 / int.MaxValue;
                }
                foreach (double[] item in listConc)  // Calc for concrete
                {
                    dci = Math.Abs(item[1] + c);
                    epci = dci * epcb2 / nadepth;
                    kj = yM + c;
                    kjj = item[1] + c;
                    if (kj * kjj <= 0)
                    {
                        epci = -epci;
                    }
                    sigci = StressStrainCurve.Concrete(epci, 0.0015, 0.00008, Rb, 0, Eb);
                    Pci = sigci * item[2] / 1000;
                    Mci = Pci * item[1] / 1000;
                    sumPc = sumPc + Pci;
                    sumMc = sumMc + Mci;
                }
                foreach (int[] item in listRebar) // Calc for rebar
                {
                    dsi = Math.Abs(item[1] + c);
                    epsi = dsi * epcb2 / nadepth;
                    kj = yM + c;
                    kjj = item[1] + c;
                    if (kj * kjj <= 0)
                    {
                        epsi = -epsi;
                    }
                    double dre = Math.PI * Math.Pow(item[2], 2) / 4;
                    sigsi = StressStrainCurve.Rebar(epsi, Rs, Rsc, Es);
                    Psi = sigsi * dre / 1000;
                    Msi = Psi * item[1] / 1000;
                    sumPs = sumPs + Psi;
                    sumMs = sumMs + Msi;
                }
                if (Math.Abs(sumPc + sumPs) < tolerance)
                {
                    naresult = Convert.ToInt32(nadepth);
                    Presult = Convert.ToInt32(sumPc + sumPs);
                    Mresult = Convert.ToInt32(sumMc + sumMs);
                    break;
                }
            }
            return (naresult, Presult, Mresult);
        }

        public static (double, double, double, double) ShearCheck(double b, double h, int Q, int dstir, int ns, int sw,
            double Rb, double Rbt, double Rsw) // Shear check for beam
        {
            double att; double DC;
            double Qn; double Qb; double Qsw; double Qs; double c;

            // Unit converted
            // b, h - (mm)
            // Q - (kN)
            // dstir, nstir, sw, acv - (mm)
            // Rb, Rbt, Rsw - (MPa)

            att = 0.15 * h;
            if (0.3 * Rb * b * (h - att) / 1000 < Q)
            {
                c = 0;
                Qb = 0;
                Qs = 0;
                DC = ushort.MaxValue;
                return (c, Qb, Qs, DC);
            }
            else
            {
                Qsw = Rsw * ns * Math.PI * Math.Pow(dstir, 2) / 4 / sw;
                c = Math.Sqrt(1.5 * Rbt * b * Math.Pow(h - att, 2) / (0.75 * Qsw));
                if (c < h - att)
                {
                    c = h - att;
                }
                if (c > 2 * (h - att))
                {
                    c = 2 * (h - att);
                }

                Qb = 1.5 * Rbt * b * Math.Pow(h - att, 2) / c;
                if (Qb < 0.5 * Rbt * b * (h - att))
                {
                    Qb = 0.5 * Rbt * b * (h - att);
                }
                if (Qb > 2.5 * Rbt * b * (h - att))
                {
                    Qb = 2.5 * Rbt * b * (h - att);
                }
                c = Math.Round(c, 0);
                Qb = Convert.ToInt32(Qb / 1000);
                Qs = Convert.ToInt32(0.75 * Qsw * c / 1000);
                Qn = Convert.ToInt32(Qb + Qs);
                DC = Math.Round((double)(Math.Abs(Q) / Qn), 2);
                return (c, Qb, Qs, DC);
            }
        }

        public static  (double, double, double) TorsionCheck(double b, double h, int T, int dstir, int ns, int sw, double sideAs,
            double Rb, double Rs, double Rsw) // Torsion check for beam
        {
            double DC;
            double Tn; double Qsw; double deltaZ; double torCap;

            // Unit converted
            // b, h - (mm)
            // Q - (kN)
            // dmain, dstir, nstir, sw, acv - (mm)
            // Rb, Rs, Rsw - (MPa)
            // side As - (mm2)

            if (0.1 * Rb * Math.Pow(b, 2) * h / 1000000 < T)
            {
                torCap = 0;
                Tn = 0;
                DC = ushort.MaxValue;
                return (torCap, Tn, DC);
            }
            else
            {
                Qsw = Rsw * ns * Math.PI * Math.Pow(dstir, 2) / 4 / sw;
                deltaZ = h / (2 * b + h);
                torCap = Rs * sideAs;
                if (torCap >= 2 * Qsw * h)
                {
                    Tn = 1.8 * Qsw * b * h * Math.Sqrt(2 * deltaZ);
                }
                else if ((torCap < 2 * Qsw * h) & (2 * Qsw * h < 3 * torCap))
                {
                    Tn = 1.8 * b * h * Math.Sqrt(torCap * Qsw / (2 * b + h));
                }
                else
                {
                    Tn = 1.8 * torCap * b * Math.Sqrt(1.5 * deltaZ);
                }
                Tn = Math.Round(Tn / 1000000, 2);
                DC = Math.Round(Math.Abs(T) / Tn, 2);
                torCap = Convert.ToInt32(torCap / 1000);
                return (torCap, Tn, DC);
            }
        }
    }
}
