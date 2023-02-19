using System;

namespace CrossStruc.Extensions
{
    public static class StressStrainCurve
    {
        public static double Concrete(double strain, double epb1red, double epbt1red, double Rbn, double Rbtn, double Eb) // Stress-strain concrete
        {
            double Ebred = Rbn / epb1red;
            double Ebtred = Rbtn / epbt1red; // If Rbt = 0 -> Skip tension
            double sigma;

            if (strain <= 0) // Tension
            {
                strain = Math.Abs(strain);
                if ((0 <= strain) && (strain <= epbt1red))
                {
                    sigma = -strain * Ebtred;
                }
                else
                {
                    sigma = -Rbtn;
                }
            }
            else // Compression
            {
                if ((0 <= strain) && (strain <= epb1red))
                {
                    sigma = strain * Ebred;
                }
                else
                {
                    sigma = Rbn;
                }
            }
            return sigma;
        }

        public static double Rebar(double strain, double Rsn, double Rscn, double Es) // Stress-strain rebar
        {
            double eps0 = Rsn / Es;
            double epsc0 = Rscn / Es;
            double sigma;
            if (strain <= 0) // Tension
            {
                strain = Math.Abs(strain);
                if ((0 <= strain) && (strain <= eps0))
                {
                    sigma = -strain * Es;
                }
                else
                {
                    sigma = -Rsn;
                }
            }
            else // Compression
            {
                if ((0 <= strain) && (strain <= epsc0))
                {
                    sigma = strain * Es;
                }
                else
                {
                    sigma = Rscn;
                }
            }
            return sigma;
        }
    }
}
