using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossStruc.Extensions
{
    public static class Other
    {
        public static List<int> ExtractCombRobot(string comb) // Extract combo Robot from string input
        {
            List<int> listcomb = new List<int>();
            string[] splitcomb = comb.Split(' ');
            for (int i = 0; i <= splitcomb.GetUpperBound(0); i++)
            {
                if (splitcomb[i].Contains("to"))
                {
                    string[] splitsub = splitcomb[i].Split(new[] { "to" }, StringSplitOptions.None);
                    for (int j = Convert.ToInt32(splitsub[0]); j <= Convert.ToInt32(splitsub[1]); j++)
                    {
                        listcomb.Add(j);
                    }
                }
                else
                {
                    listcomb.Add(Convert.ToInt32(splitcomb[i]));
                }
            }
            return listcomb;
        }

        public static int DeterMesh(double size) // Optimize section mesh size
        {
            if (Convert.ToInt32(size) % 25 == 0)
            {
                return 25;
            }
            else
            {
                return 10;
            }
        }

        public static string ZeroIfEmpty(this string s)
        {
            return string.IsNullOrEmpty(s) ? "0" : s;
        }

        public static double ConvertRebar(int dmain) // Optimize rebar size for drawing chart
        {
            double dmainconvert = Convert.ToInt32(0.6 * dmain + 3.08);
            return dmainconvert;
        }

    }
}
