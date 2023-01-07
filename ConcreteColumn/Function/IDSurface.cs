using CrossStruc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossStruc.ConcreteColumn.Function
{
    public class IDSurface
    {
        public static (double[,], double[,]) RecSect(List<double[]> concreteElement, List<int[]> rebarElement, double Cx, double Cy,
            double Rb, double Eb, double Rs, double Rsc, double Es, int nver, int nhoz) // ID Surface for rectangle section
        {
            Cx = Math.Round(Cx / 10, 0) * 10;
            Cy = Math.Round(Cy / 10, 0) * 10;

            int k; int i; int j; int u;
            double xM = Cx / 2 + 100;
            double yM = Cy / 2 + 100;
            double epcb2 = 0.0035;
            int a0 = 0; int b0 = 1;
            int col; int row;
            double rotateAngle; double a; double b; double cmin; double cmax; double c;
            double nadepth; double kj; double kjj;
            double dci; double epci; double sigci; double Pci; double Mcxi; double Mcyi;
            double dsi; double epsi; double sigsi; double Psi; double Msxi; double Msyi;
            double sumPc; double sumMcx; double sumMcy; double sumPs; double sumMsx; double sumMsy;
            double[,] vervalue = new double[nhoz, 4 * nver];
            // Rotate neutral axis in I/4 section
            col = 0;
            for (u = 0; u < nver; u++)
            {
                row = 0;
                rotateAngle = 0.5 * Math.PI / (nver - 1) * u;
                a = a0 * Math.Cos(rotateAngle) + b0 * Math.Sin(rotateAngle);
                b = b0 * Math.Cos(rotateAngle) - a0 * Math.Sin(rotateAngle);
                cmin = -a * Cx / 2 - b * Cy / 2 + a0 * Cx / 2 + b0 * Cy / 2 - Cy / 2;
                cmax = -cmin;
                for (i = 0; i < nhoz; i++)
                {
                    sumPc = 0;
                    sumMcx = 0;
                    sumMcy = 0;
                    sumPs = 0;
                    sumMsx = 0;
                    sumMsy = 0;
                    if (i == nhoz - 1) // Final loop
                    {
                        sumPc = Cx * Cy * Rb / 1000;
                        sumPs = Math.PI * Math.Pow(rebarElement[0][2], 2) / 4 * rebarElement.Count * Rsc / 1000;
                    }
                    else
                    {
                        c = cmin + 2 * Math.Abs(cmin) / (nhoz - 1) * i;
                        nadepth = Math.Abs(a * Cx / 2 + b * Cy / 2 + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                        if ((Math.Round(nadepth, 2) == 0) || (Math.Round(c - cmax, 2) == 0))
                        {
                            nadepth = 1 / int.MaxValue;
                        }
                        foreach (double[] concrete in concreteElement) // Calc for concrete
                        {
                            kj = a * xM + b * yM + c;
                            kjj = a * concrete[0] + b * concrete[1] + c;
                            dci = Math.Abs(a * concrete[0] + b * concrete[1] + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                            epci = dci * epcb2 / nadepth;
                            if (kj * kjj <= 0)
                            {
                                epci = -epci;
                            }
                            sigci = StressStrainCurve.Concrete(epci, 0.0015, 0.00008, Rb, 0, Eb); // Not consider tension of concrete
                            Pci = sigci * concrete[2] / 1000;
                            Mcxi = Pci * concrete[0] / 1000;
                            Mcyi = Pci * concrete[1] / 1000;
                            sumPc = sumPc + Pci;
                            sumMcx = sumMcx + Mcxi;
                            sumMcy = sumMcy + Mcyi;
                        }
                        for (j = 0; j < rebarElement.Count; j++) // Calc for rebar
                        {
                            dsi = Math.Abs(a * rebarElement[j][0] + b * rebarElement[j][1] + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                            epsi = dsi * epcb2 / nadepth;
                            kj = a * xM + b * yM + c;
                            kjj = a * rebarElement[j][0] + b * rebarElement[j][1] + c;
                            if (kj * kjj <= 0)
                            {
                                epsi = -epsi;
                            }
                            double dre = Math.PI * Math.Pow(rebarElement[j][2], 2) / 4;
                            sigsi = StressStrainCurve.Rebar(epsi, Rs, Rsc, Es);
                            Psi = sigsi * dre / 1000;
                            Msxi = Psi * rebarElement[j][0] / 1000;
                            Msyi = Psi * rebarElement[j][1] / 1000;
                            sumPs = sumPs + Psi;
                            sumMsx = sumMsx + Msxi;
                            sumMsy = sumMsy + Msyi;
                        }
                    }
                    vervalue[row, col] = Math.Round(sumMcx + sumMsx, 2);
                    vervalue[row, col + 1] = Math.Round(sumMcy + sumMsy, 2);
                    vervalue[row, col + 2] = Math.Round(sumPc + sumPs, 2);
                    vervalue[row, col + 3] = Math.Round(rotateAngle * (180 / Math.PI), 2);
                    row = row + 1;
                }
                col = col + 4;
            }
            // Calc Pz(i)
            double Pzmin = vervalue[0, 2];
            double Pzmax = vervalue[nhoz - 1, 2];
            double[] Pzdelta = new double[nhoz];
            double ki;
            for (i = 0; i < nhoz; i++)
            {
                Pzdelta[i] = Pzmin + (Pzmax - Pzmin) / (nhoz - 1) * i;
            }
            // Find intersect between Pz(i) and vertical curve
            double[,] hozvalue = new double[nver, 4 * nhoz];
            col = 0;
            for (i = 0; i < nhoz; i++)
            {
                row = 0;
                for (k = 0; k < 4 * nver; k += 4)
                {
                    for (j = 0; j < nhoz - 1; j++)
                    {
                        ki = Math.Round((Pzdelta[i] - vervalue[j, k + 2]) / (vervalue[j + 1, k + 2] - vervalue[j, k + 2]), 2);
                        if ((0 <= ki) && (ki <= 1))
                        {
                            hozvalue[row, col] = Math.Round(ki * (vervalue[j + 1, k] - vervalue[j, k]) + vervalue[j, k], 2);
                            hozvalue[row, col + 1] = Math.Round(ki * (vervalue[j + 1, k + 1] - vervalue[j, k + 1]) + vervalue[j, k + 1], 2);
                            hozvalue[row, col + 2] = Math.Round(Pzdelta[i], 2);
                            hozvalue[row, col + 3] = ki;
                            row = row + 1;
                            break;
                        }
                    }
                }
                col = col + 4;
            }
            return (vervalue, hozvalue);
        }

        public static (double[,], double[,]) CirSect(List<double[]> concreteElement, List<int[]> rebarElement, double D,
            double Rb, double Eb, double Rs, double Rsc, double Es, int nver, int nhoz) // ID Surface for circle section
        {
            D = Math.Round(D / 10, 0) * 10;
            int k; int i; int j; int u;
            double xM = D / 2 + 100;
            double yM = D / 2 + 100;
            double xH; double yH;
            double epcb2 = 0.0035;
            int a0 = 0; int b0 = 1;
            int col; int row;
            double rotateAngle; double a; double b; double cmin; double cmax; double c;
            double nadepth; double kj; double kjj;
            double dci; double epci; double sigci; double Pci; double Mcxi; double Mcyi;
            double dsi; double epsi; double sigsi; double Psi; double Msxi; double Msyi;
            double sumPc; double sumMcx; double sumMcy; double sumPs; double sumMsx; double sumMsy;
            double[,] vervalue = new double[nhoz, 4 * nver];
            // Rotate neutral axis in I/4 section
            col = 0;
            for (u = 0; u < nver; u++)
            {
                row = 0;
                rotateAngle = 0.5 * Math.PI / (nver - 1) * u;
                a = a0 * Math.Cos(rotateAngle) + b0 * Math.Sin(rotateAngle);
                b = b0 * Math.Cos(rotateAngle) - a0 * Math.Sin(rotateAngle);
                yH = Math.Sqrt(Math.Pow(D / 2, 2) * Math.Pow(b, 2) / (Math.Pow(a, 2) + Math.Pow(b, 2)));
                xH = Math.Sqrt(Math.Pow(D / 2, 2) - Math.Pow(yH, 2));
                cmin = -a * xH - b * yH + a0 * xH + b0 * yH - yH;
                cmax = -cmin;
                for (i = 0; i < nhoz; i++)
                {
                    sumPc = 0;
                    sumMcx = 0;
                    sumMcy = 0;
                    sumPs = 0;
                    sumMsx = 0;
                    sumMsy = 0;
                    if (i == nhoz - 1) // Final loop
                    {
                        sumPc = Math.PI * Math.Pow(D, 2) / 4 * Rb / 1000;
                        sumPs = Math.PI * Math.Pow(rebarElement[0][2], 2) / 4 * rebarElement.Count * Rsc / 1000;
                    }
                    else
                    {
                        c = cmin + 2 * Math.Abs(cmin) / (nhoz - 1) * i;
                        nadepth = Math.Abs(a * xH + b * yH + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                        if ((Math.Round(nadepth, 2) == 0) || (Math.Round(c - cmax, 2) == 0))
                        {
                            nadepth = 1 / int.MaxValue;
                        }
                        foreach (double[] concrete in concreteElement) // Calc for concrete
                        {
                            kj = a * xM + b * yM + c;
                            kjj = a * concrete[0] + b * concrete[1] + c;
                            dci = Math.Abs(a * concrete[0] + b * concrete[1] + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                            epci = dci * epcb2 / nadepth;
                            if (kj * kjj <= 0)
                            {
                                epci = -epci;
                            }
                            sigci = StressStrainCurve.Concrete(epci, 0.0015, 0.00008, Rb, 0, Eb); // Not consider tension of concrete
                            Pci = sigci * concrete[02] / 1000;
                            Mcxi = Pci * concrete[0] / 1000;
                            Mcyi = Pci * concrete[1] / 1000;
                            sumPc = sumPc + Pci;
                            sumMcx = sumMcx + Mcxi;
                            sumMcy = sumMcy + Mcyi;
                        }
                        for (j = 0; j < rebarElement.Count; j++) // Calc for rebar
                        {
                            dsi = Math.Abs(a * rebarElement[j][0] + b * rebarElement[j][1] + c) / Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
                            epsi = dsi * epcb2 / nadepth;
                            kj = a * xM + b * yM + c;
                            kjj = a * rebarElement[j][0] + b * rebarElement[j][1] + c;
                            if (kj * kjj <= 0)
                            {
                                epsi = -epsi;
                            }
                            double dre = Math.PI * Math.Pow(rebarElement[j][2], 2) / 4;
                            sigsi = StressStrainCurve.Rebar(epsi, Rs, Rsc, Es);
                            Psi = sigsi * dre / 1000;
                            Msxi = Psi * rebarElement[j][0] / 1000;
                            Msyi = Psi * rebarElement[j][1] / 1000;
                            sumPs = sumPs + Psi;
                            sumMsx = sumMsx + Msxi;
                            sumMsy = sumMsy + Msyi;
                        }
                    }

                    vervalue[row, col] = Math.Round(sumMcx + sumMsx, 2);
                    vervalue[row, col + 1] = Math.Round(sumMcy + sumMsy, 2);
                    vervalue[row, col + 2] = Math.Round(sumPc + sumPs, 2);
                    vervalue[row, col + 3] = Math.Round(rotateAngle * (180 / Math.PI), 2);
                    row = row + 1;
                }
                col = col + 4;
            }

            // Calc Pz(i)
            double Pzmin = vervalue[0, 2];
            double Pzmax = vervalue[nhoz - 1, 2];
            double[] Pzdelta = new double[nhoz];
            double ki;
            for (i = 0; i < nhoz; i++)
            {
                Pzdelta[i] = Pzmin + (Pzmax - Pzmin) / (nhoz - 1) * i;
            }
            // Find intersect between Pz(i) and vertical curve
            double[,] hozvalue = new double[nver, 4 * nhoz];
            col = 0;
            for (i = 0; i < nhoz; i++)
            {
                row = 0;
                for (k = 0; k < 4 * nver; k += 4)
                {
                    for (j = 0; j < nhoz - 1; j++)
                    {
                        ki = Math.Round((Pzdelta[i] - vervalue[j, k + 2]) / (vervalue[j + 1, k + 2] - vervalue[j, k + 2]), 2);
                        if ((0 <= ki) && (ki <= 1))
                        {
                            hozvalue[row, col] = Math.Round(ki * (vervalue[j + 1, k] - vervalue[j, k]) + vervalue[j, k], 2);
                            hozvalue[row, col + 1] = Math.Round(ki * (vervalue[j + 1, k + 1] - vervalue[j, k + 1]) + vervalue[j, k + 1], 2);
                            hozvalue[row, col + 2] = Math.Round(Pzdelta[i], 2);
                            hozvalue[row, col + 3] = ki;
                            row = row + 1;
                            break;
                        }
                    }
                }
                col = col + 4;
            }
            return (vervalue, hozvalue);
        }

    }
}
