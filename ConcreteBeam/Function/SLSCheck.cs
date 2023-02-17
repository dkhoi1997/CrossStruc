using CrossStruc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrossStruc.ConcreteBeam.Function
{
    public class SLSCheck
    {
        public static double BeamMcrc(double b, double h, int tolerance, List<double[]> listConc, List<int[]> listRebarT, List<int[]> listRebarB,
            bool revertScan, bool compressBar, double Rbn, double Rbtn, double Eb,
            double Rs, double Rsc, double Es) // Cracked moment of beam
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
            int Mcrc = 0;

            double epcbt2 = 0.00015;
            double epb1red = 0.0015;
            double epbt1red = 0.00008;

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
                    epci = dci * epcbt2 / (h - nadepth);
                    kj = yM + c;
                    kjj = item[1] + c;
                    if (kj * kjj <= 0)
                    {
                        epci = -epci;
                    }
                    sigci = StressStrainCurve.Concrete(epci, epb1red, epbt1red, Rbn, Rbtn, Eb);
                    Pci = sigci * item[2] / 1000;
                    Mci = Pci * item[1] / 1000;
                    sumPc = sumPc + Pci;
                    sumMc = sumMc + Mci;
                }
                foreach (int[] item in listRebar) // Calc for rebar
                {
                    dsi = Math.Abs(item[1] + c);
                    epsi = dsi * epcbt2 / (h - nadepth);
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
                    Mcrc = Convert.ToInt32(sumMc + sumMs);
                    break;
                }
            }
            return Mcrc;
        }

        public static (double, double, double, double) CrackWidthParameter(double b, double h, int tolerance, List<double[]> listConc, List<int[]> listRebarT, List<int[]> listRebarB,
            bool revertScan, bool compressBar, double epb1red, double epbt1red, double epbt2, double Rbn, double Rbtn, double Eb,
            double Rs, double Rsc, double Es) // Calc parameter for crack width
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
            int tensDepth = 0;
            double tensAreaConc = 0; 
            double tensAreaRebar = 0;
            double disTensToComp = 0;

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
                    epci = dci * epbt2 / (h - nadepth);
                    kj = yM + c;
                    kjj = item[1] + c;
                    if (kj * kjj <= 0)
                    {
                        epci = -epci;
                    }
                    sigci = StressStrainCurve.Concrete(epci, epb1red, epbt1red, Rbn, Rbtn, Eb);
                    Pci = sigci * item[2] / 1000;
                    Mci = Pci * item[1] / 1000;
                    sumPc = sumPc + Pci;
                    sumMc = sumMc + Mci;
                }
                foreach (int[] item in listRebar) // Calc for rebar
                {
                    dsi = Math.Abs(item[1] + c);
                    epsi = dsi * epbt2 / (h - nadepth);
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
                    double sumSquareCompressionForce = 0;
                    double sumCompressionForce = 0;

                    // Distance of tensile concrete
                    tensDepth = Convert.ToInt32(h - nadepth);

                    // Area of tensile concrete & total concrete compression force
                    foreach (double[] item in listConc)
                    {
                        dci = Math.Abs(item[1] + c);
                        epci = dci * epbt2 / (h - nadepth);
                        kj = yM + c;
                        kjj = item[1] + c;
                        if (kj * kjj <= 0) // Tension side
                        {
                            tensAreaConc = tensAreaConc + item[2];
                        }
                        else // Compression force of concrete
                        {
                            sigci = StressStrainCurve.Concrete(epci, epb1red, epbt1red, Rbn, Rbtn, Eb);
                            Pci = sigci * item[2] / 1000;
                            sumSquareCompressionForce = sumSquareCompressionForce + Pci * item[1];
                            sumCompressionForce = sumCompressionForce + Pci;
                        }
                    }

                    // Area of tensile rebar & total rebar compression force
                    double sumAsSquareRebar = 0;
                    double sumAsRebar = 0;
                    foreach (int[] item in listRebar)
                    {
                        dsi = Math.Abs(item[1] + c);
                        epsi = dsi * epbt2 / (h - nadepth);
                        kj = yM + c;
                        kjj = item[1] + c;
                        if (kj * kjj <= 0) // Tension side
                        {
                            double dre = Math.PI * Math.Pow(item[2], 2) / 4;
                            sumAsSquareRebar = sumAsSquareRebar + dre * item[1];
                            sumAsRebar = sumAsRebar + dre;
                            tensAreaRebar = tensAreaRebar + dre;
                        }
                        else // Compression force of rebar
                        {
                            double dre = Math.PI * Math.Pow(item[2], 2) / 4;
                            sigsi = StressStrainCurve.Rebar(epsi, Rs, Rsc, Es);
                            Psi = sigsi * item[2] / 1000;
                            sumSquareCompressionForce = sumSquareCompressionForce + Psi * item[1];
                            sumCompressionForce = sumCompressionForce + Psi;
                        }
                    }

                    // Distance from maximum tensile rebar to centroid of compression zone
                    disTensToComp = Math.Sqrt(Math.Pow(sumSquareCompressionForce / sumCompressionForce - sumAsSquareRebar / sumAsRebar, 2));

                    break;
                }
            }
            return (tensDepth, tensAreaConc, tensAreaRebar, disTensToComp);
        }

        public static (double, double, double) SigTensionCrack (double Mcrc, double Mterm, double Rsn,
            double tensAreaConc, double tensAreaRebar, double equivDia, double disTensToComp) // Calc base crack length and tension stress crack;
        {
            double Lcrc = 0;
            double sig = 0;
            double psis = 0;

            Mterm = Math.Abs(Mterm);
            Mcrc = Math.Abs(Mcrc);

            if (Mterm > Mcrc) // Consider crack
            {
                Lcrc = 0.5 * tensAreaConc / tensAreaRebar * equivDia;
                if (Lcrc > Math.Min(40 * equivDia, 400))
                {
                    Lcrc = Math.Min(40 * equivDia, 400);
                }
                if (Lcrc < Math.Max(10 * equivDia, 100))
                {
                    Lcrc = Math.Max(10 * equivDia, 100);
                }

                psis = 1 - Mcrc / Mterm;
                sig = Math.Round(Math.Min(Mterm * 1000000 / (disTensToComp * tensAreaRebar), Rsn), 0);

            }

            return (Lcrc, psis, sig);
        }

        public static (double, double) CrackWidth(double LcrcS, double psisS, double sigS, double LcrcL, double psisL, double sigL, double Es) // Calc crack width
        {
            double psi1S = 1;
            double psi1L = 1.4;
            double psi2 = 0.5;
            double psi3 = 1;
            double acrc1 = psi1L * psi2 * psi3 * psisL * sigL / Es * LcrcL;
            double acrc2 = psi1S * psi2 * psi3 * psisS * sigS / Es * LcrcS;
            double acrc3 = psi1S * psi2 * psi3 * psisL * sigL / Es * LcrcL;
            double acrcS = Math.Round(acrc1 + acrc2 - acrc3, 2);
            double acrcL = Math.Round(acrc1, 2);

            return (acrcS, acrcL);
        }
    }
}
