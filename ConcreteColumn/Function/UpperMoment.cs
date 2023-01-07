using System;

namespace CrossStruc.ConcreteColumn.Function
{
    public class UpperMoment
    {
        public static (int, int) RecSect(int P, int Mx, int My, double Cx,
            double Cy, double Lx, double Ly, double Eb, double kx, double ky) // Design moment for rectangle section
        {
            int Mxup; int Myup;
            Eb *= 1000;
            Cx /= 1000;
            Cy /= 1000;
            Lx /= 1000;
            Ly /= 1000;


            if (P <= 0)
            {
                Mxup = Math.Max(Math.Abs(Mx), 1);
                Myup = Math.Max(Math.Abs(My), 1);
                return (Mxup, Myup);
            }
            else
            {
                double e1x = (double)Mx / P;
                double e1y = (double)My / P;
                double ea1x = Math.Max(Math.Max(Lx / 600, Cx / 30), 0.01);
                double ea1y = Math.Max(Math.Max(Ly / 600, Cy / 30), 0.01);
                double e0x = Math.Max(e1x, ea1x);
                double e0y = Math.Max(e1y, ea1y);
                double Ibx = Cy * Math.Pow(Cx, 3) / 12;
                double Iby = Cx * Math.Pow(Cy, 3) / 12;
                double Ncrx = 2.5 * Eb * Ibx / Math.Pow(Lx * kx, 2);
                double Ncry = 2.5 * Eb * Iby / Math.Pow(Ly * ky, 2);

                if (Ncrx < P) Mxup = ushort.MaxValue;
                else
                {
                    double etax = 1 / (1 - P / Ncrx);
                    Mxup = Math.Max((int)(P * e0x * etax), 1);
                }
                if (Ncry < P) Myup = ushort.MaxValue;
                else
                {
                    double etay = 1 / (1 - P / Ncry);
                    Myup = Math.Max((int)(P * e0y * etay), 1);
                }
                return (Mxup, Myup);
            }
        }

        public static (int, int) CirSect(int P, int Mx, int My, double D,
            double Lx, double Ly, double Eb, double kx, double ky) // Design moment for circle section
        {
            int Mxup; int Myup;
            Eb *= 1000;
            D /= 1000;
            Lx /= 1000;
            Ly /= 1000;

            if (P <= 0)
            {
                Mxup = Math.Max(Math.Abs(Mx), 1);
                Myup = Math.Max(Math.Abs(My), 1);
                return (Mxup, Myup);
            }
            else
            {
                //Độ lệch tâm
                double e1x = (double)Mx / P;
                double e1y = (double)My / P;
                double ea1x = Math.Max(Math.Max(Lx / 600, D / 30), 0.01);
                double ea1y = Math.Max(Math.Max(Ly / 600, D / 30), 0.01);
                double e0x = Math.Max(e1x, ea1x);
                double e0y = Math.Max(e1y, ea1y);
                double Ib = Math.PI / 4 * Math.Pow(D / 2, 4);
                double Ncrx = 2.5 * Eb * Ib / Math.Pow(Lx * kx, 2);
                double Ncry = 2.5 * Eb * Ib / Math.Pow(Ly * ky, 2);
                if (Ncrx < P) Mxup = ushort.MaxValue;
                else
                {
                    double etax = 1 / (1 - P / Ncrx);
                    Mxup = Math.Max((int)(P * e0x * etax), 1);
                }
                if (Ncry < P) Myup = ushort.MaxValue;
                else
                {
                    double etay = 1 / (1 - P / Ncry);
                    Myup = Math.Max((int)(P * e0y * etay), 1);
                }
                return (Mxup, Myup);
            }
        }
    }
}
