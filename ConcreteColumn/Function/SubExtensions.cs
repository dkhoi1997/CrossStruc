using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CrossStruc.ConcreteColumn.Function
{
    public static class SubExtensions
    {
        public static double RebarPercent(string shape, double Cx, double Cy, int sumn, int dmain) // Cacl rebar percent of section
        {
            double sect;
            if (shape == "Rec")
            {
                sect = Cx * Cy;
            }
            else
            {
                sect = Math.PI * Math.Pow(Cx, 2) / 4;
            }
            double rebarA = sumn * Math.PI * Math.Pow(dmain, 2) / 4;
            double percent = Math.Round(100 * rebarA / sect, 2);

            return percent;
        }

        public static (double, double, double, double) FindMaxMin(double[,] input) // Get min max of ID Surface array value
        {
            // Mặc định X-0, Y-1
            double maxX = input[0, 0];
            double minX = input[0, 0];
            double maxY = input[0, 1];
            double minY = input[0, 1];
            for (int i = 1; i <= input.GetUpperBound(0); i++)
            {
                if (maxX < input[i, 0]) maxX = input[i, 0];
                if (minX > input[i, 0]) minX = input[i, 0];
                if (maxY < input[i, 1]) maxY = input[i, 1];
                if (minY > input[i, 1]) minY = input[i, 1];
            }
            return (maxX, minX, maxY, minY);

        }
    }
}
