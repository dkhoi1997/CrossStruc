using System;

namespace CrossStruc.Extensions
{
    public static class Material
    {
        public static (double, double, double) GetConcrete(string grade) // Get concrete parameter (characteristic)
        {
            double Rbn = 0;
            double Rbtn = 0;
            double Eb = 0;
            switch (grade)
            {
                case "B15":
                    Rbn = 11;
                    Rbtn = 1.1;
                    Eb = 23000;
                    break;
                case "B20":
                    Rbn = 15;
                    Rbtn = 1.35;
                    Eb = 27000;
                    break;
                case "B22.5":
                    Rbn = 16.5;
                    Rbtn = 1.4;
                    Eb = 28500;
                    break;
                case "B25":
                    Rbn = 18.5;
                    Rbtn = 1.55;
                    Eb = 30000;
                    break;
                case "B30":
                    Rbn = 22;
                    Rbtn = 1.75;
                    Eb = 32500;
                    break;
                case "B35":
                    Rbn = 25.5;
                    Rbtn = 1.95;
                    Eb = 34500;
                    break;
                case "B40":
                    Rbn = 29;
                    Rbtn = 2.1;
                    Eb = 36000;
                    break;
                case "B45":
                    Rbn = 32;
                    Rbtn = 2.25;
                    Eb = 37500;
                    break;
                case "B50":
                    Rbn = 36;
                    Rbtn = 2.45;
                    Eb = 39000;
                    break;
                case "B55":
                    Rbn = 39.5;
                    Rbtn = 2.6;
                    Eb = 39500;
                    break;
                case "B60":
                    Rbn = 43;
                    Rbtn = 2.75;
                    Eb = 40000;
                    break;
            }
            return (Rbn, Rbtn, Eb);
        }

        public static (double, double, double, double, double, double) GetConcreteStrain(string type) // Concrete strain for SLS calc
        {
            double epb0 = 0;
            double epb2 = 0;
            double epb1red = 0;
            double epbt0 = 0;
            double epbt2 = 0;
            double epbt1red = 0;
            switch (type)
            {
                case "I": // Larger than 75%
                    epb0 = 0.0030;
                    epb2 = 0.0042;
                    epb1red = 0.0024;
                    epbt0 = 0.00021;
                    epbt2 = 0.00027;
                    epbt1red = 0.00019;
                    break;
                case "II": // From 40 - 75 %
                    epb0 = 0.0034;
                    epb2 = 0.0048;
                    epb1red = 0.0028;
                    epbt0 = 0.00024;
                    epbt2 = 0.00031;
                    epbt1red = 0.00022;
                    break;
                case "III": // Below 40%
                    epb0 = 0.0040;
                    epb2 = 0.0056;
                    epb1red = 0.0034;
                    epbt0 = 0.00028;
                    epbt2 = 0.00036;
                    epbt1red = 0.00026;
                    break;
            }
            return (epb0, epb2, epb1red, epbt0, epbt2, epbt1red);
        }

        public static (double, double, double, double) GetRebar(string grade) // Get rebar parameter (characteristic)
        {
            double Rsn = 0;
            double Rscn = 0;
            double Rswn = 0;
            double Es = 0;
            switch (grade)
            {
                case "CB240-T":
                    Rsn = 240;
                    Rscn = 240;
                    Rswn = Math.Min(240 * 0.81, 345);
                    Es = 200000;
                    break;
                case "CB300-T":
                    Rsn = 300;
                    Rscn = 300;
                    Rswn = Math.Min(300 * 0.81, 345);
                    Es = 200000;
                    break;
                case "CB300-V":
                    Rsn = 300;
                    Rscn = 300;
                    Rswn = Math.Min(300 * 0.81, 345);
                    Es = 200000;
                    break;
                case "CB400-V":
                    Rsn = 400;
                    Rscn = 400;
                    Rswn = Math.Min(400 * 0.81, 345);
                    Es = 200000;
                    break;
                case "CB500-V":
                    Rsn = 500;
                    Rscn = 500;
                    Rswn = Math.Min(500 * 0.81, 345); ;
                    Es = 200000;
                    break;
            }
            return (Rsn, Rscn, Rswn, Es);
        }
    }
}
