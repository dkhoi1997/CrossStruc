using System;

namespace CrossStruc.ConcreteColumn.Function
{
    public class UpperMoment
    {
        public static (double, double, double, double, double, double, int, int) RecSect(int P, int Mx, int My, double Cx,
            double Cy, double Lx, double Ly, double Eb, double kx, double ky) // Design moment for rectangle section
        {
            int Mxup; int Myup;
            double e0x; double e0y;
            double etax; double etay;

            Eb *= 1000;
            Cx /= 1000;
            Cy /= 1000;
            Lx /= 1000;
            Ly /= 1000;

            double Ibx = Cy * Math.Pow(Cx, 3) / 12;
            double Iby = Cx * Math.Pow(Cy, 3) / 12;
            double Ncrx = Math.Round(2.5 * Eb * Ibx / Math.Pow(Lx * kx, 2), 0);
            double Ncry = Math.Round(2.5 * Eb * Iby / Math.Pow(Ly * ky, 2), 0);

            if (P <= 0)
            {
                e0x = 0;
                e0y = 0;
                etax = 0;
                etay = 0;
                Mxup = Math.Max(Math.Abs(Mx), 1);
                Myup = Math.Max(Math.Abs(My), 1);
                return (e0x, e0y, Ncrx, Ncry, etax, etay, Mxup, Myup);
            }
            else
            {
                double e1x = (double)Mx / P;
                double e1y = (double)My / P;
                double ea1x = Math.Max(Math.Max(Lx / 600, Cx / 30), 0.01);
                double ea1y = Math.Max(Math.Max(Ly / 600, Cy / 30), 0.01);
                e0x = Math.Round(Math.Max(e1x, ea1x),3);
                e0y = Math.Round(Math.Max(e1y, ea1y),3);

                if (Ncrx < P)
                {
                    etax = ushort.MaxValue;
                    Mxup = ushort.MaxValue;
                }
                else
                {
                    etax = 1 / (1 - P / Ncrx);
                    Mxup = Math.Max((int)(P * e0x * etax), 1);
                }
                if (Ncry < P)
                {
                    etay = ushort.MaxValue;
                    Myup = ushort.MaxValue;
                }
                else
                {
                    etay = 1 / (1 - P / Ncry);
                    Myup = Math.Max((int)(P * e0y * etay), 1);
                }

                e0x = e0x * 1000;
                e0y = e0y * 1000;
                etax = Math.Round(etax, 2);
                etay = Math.Round(etay, 2);

                return (e0x, e0y, Ncrx, Ncry, etax, etay, Mxup, Myup);
            }
        }

        public static (double, double, double, double, double, double, int, int) CirSect(int P, int Mx, int My, double D,
            double Lx, double Ly, double Eb, double kx, double ky) // Design moment for circle section
        {
            int Mxup; int Myup;
            double e0x; double e0y;
            double etax; double etay;

            Eb *= 1000;
            D /= 1000;
            Lx /= 1000;
            Ly /= 1000;

            double Ib = Math.PI / 4 * Math.Pow(D / 2, 4);
            double Ncrx = Math.Round(2.5 * Eb * Ib / Math.Pow(Lx * kx, 2), 0);
            double Ncry = Math.Round(2.5 * Eb * Ib / Math.Pow(Ly * ky, 2), 0);

            if (P <= 0)
            {
                e0x = 0;
                e0y = 0;
                etax = 0;
                etay = 0;
                Mxup = Math.Max(Math.Abs(Mx), 1);
                Myup = Math.Max(Math.Abs(My), 1);
                return (e0x, e0y, Ncrx, Ncry, etax, etay, Mxup, Myup);
            }
            else
            {
                double e1x = (double)Mx / P;
                double e1y = (double)My / P;
                double ea1x = Math.Max(Math.Max(Lx / 600, D / 30), 0.01);
                double ea1y = Math.Max(Math.Max(Ly / 600, D / 30), 0.01);
                e0x = Math.Round(Math.Max(e1x, ea1x), 3);
                e0y = Math.Round(Math.Max(e1y, ea1y), 3);

                if (Ncrx < P)
                {
                    etax = ushort.MaxValue;
                    Mxup = ushort.MaxValue;
                }
                else
                {
                    etax = 1 / (1 - P / Ncrx);
                    Mxup = Math.Max((int)(P * e0x * etax), 1);
                }
                if (Ncry < P)
                {
                    etay = ushort.MaxValue;
                    Myup = ushort.MaxValue;
                }
                else
                {
                    etay = 1 / (1 - P / Ncry);
                    Myup = Math.Max((int)(P * e0y * etay), 1);
                }

                e0x = e0x * 1000;
                e0y = e0y * 1000;
                etax = Math.Round(etax, 2);
                etay = Math.Round(etay, 2);

                return (e0x, e0y, Ncrx, Ncry, etax, etay, Mxup, Myup);
            }
        }
    }
}
