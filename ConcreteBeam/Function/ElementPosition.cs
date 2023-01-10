using System;
using System.Collections.Generic;
using ExtOther = CrossStruc.Extensions.Other;

namespace CrossStruc.ConcreteBeam.Function
{
    public class ElementPosition
    {

        public static List<double[]> Concrete(double b, double h, double tf, double bs, bool secT, bool secTrevert) // Mesh concrete column section (Rec, T, Inverted-T)
        {
            List<double[]> listconc = new List<double[]>();
            Dictionary<string, double[]> dicconc = new Dictionary<string, double[]>();

            // Rectangle section
            int di = Math.Min(ExtOther.DeterMesh(b), ExtOther.DeterMesh(h));
            int du = di * di;
            for (int i = 0; i < h / di; i++)
            {
                for (int j = 0; j < b / di; j++)
                {
                    double[] pos = new double[3];
                    pos[0] = b / 2 - di / 2 - di * j;
                    pos[1] = h / 2 - di / 2 - di * i;
                    pos[2] = du;
                    string combine = Convert.ToString(pos[0]) + Convert.ToString(pos[1]);
                    if (dicconc.ContainsKey(combine) == false)
                    {
                        dicconc.Add(combine, pos);
                    }
                }
            }

            // T-shaped section
            if (secT == true)
            {
                int k = 1;
                double bf = b + 2 * bs;
                if (secTrevert == true)
                {
                    k = -1;
                }
                for (int i = 0; i < tf / di; i++)
                {
                    for (int j = 0; j < bf / di; j++)
                    {
                        double[] pos = new double[3];
                        pos[0] = bf / 2 - di / 2 - di * j;
                        pos[1] = (h / 2 - di / 2 - di * i) * k;
                        pos[2] = du;
                        string combine = Convert.ToString(pos[0]) + Convert.ToString(pos[1]);
                        if (dicconc.ContainsKey(combine) == false)
                        {
                            dicconc.Add(combine, pos);
                        }
                    }
                }
            }

            // Transfer dict to list
            foreach (KeyValuePair<string, double[]> item in dicconc)
            {
                listconc.Add(item.Value);
            }

            return listconc;

        }

        public static (List<int[]>, List<int[]>) Rebar(double b, double h, double acv, double tw, int dstir,
            int n1top, int d1top, int n2top, int d2top, int n3top, int d3top,
            int n1bot, int d1bot, int n2bot, int d2bot, int n3bot, int d3bot) // Get rebar coordinate
        {

            double delta; double baseX; double baseY; int dmain; int k;
            // Top layer rebar
            List<int[]> listTop = new List<int[]>();
            int sumnTop = n1top + n2top + n3top;
            for (int i = 0; i < sumnTop; i++)
            {
                if (i < n1top) // 1st
                {
                    delta = (b - 2 * (acv + dstir + d1top / 2)) / (Math.Max(n1top, 2) - 1);
                    baseX = -b / 2 + acv + dstir + d1top / 2;
                    baseY = h / 2 - acv - dstir - d1top / 2;
                    dmain = d1top;
                    k = i;
                }
                else if ((i > n1top - 1) && (i < n1top + n2top)) // 2nd
                {
                    delta = (b - 2 * (acv + dstir + d2top / 2)) / (Math.Max(n2top, 2) - 1);
                    baseX = -b / 2 + acv + dstir + d2top / 2;
                    baseY = h / 2 - acv - dstir - d1top - tw - d2top / 2;
                    dmain = d2top;
                    k = i - n1top;
                }
                else // 3rd
                {
                    delta = (b - 2 * (acv + dstir + d3top / 2)) / (Math.Max(n3top, 2) - 1);
                    baseX = -b / 2 + acv + dstir + d3top / 2;
                    baseY = h / 2 - acv - dstir - d1top - tw - d2top - tw - d3top / 2;
                    dmain = d3top;
                    k = i - (n1top + n2top);
                }
                int[] pos = new int[3];
                pos[0] = Convert.ToInt32(baseX + delta * k);
                pos[1] = Convert.ToInt32(baseY);
                pos[2] = Convert.ToInt32(dmain);
                listTop.Add(pos);
            }

            // Bot layer rebar
            List<int[]> listBot = new List<int[]>();
            int sumnBot = n1bot + n2bot + n3bot;
            for (int i = 0; i < sumnBot; i++)
            {
                if (i < n1bot) // 1st
                {
                    delta = (b - 2 * (acv + dstir + d1bot / 2)) / (Math.Max(n1bot, 2) - 1);
                    baseX = -b / 2 + acv + dstir + d1bot / 2;
                    baseY = -h / 2 + acv + dstir + d1bot / 2;
                    dmain = d1bot;
                    k = i;
                }
                else if ((i > n1bot - 1) && (i < n1bot + n2bot)) // 2nd
                {
                    delta = (b - 2 * (acv + dstir + d2bot / 2)) / (Math.Max(n2bot, 2) - 1);
                    baseX = -b / 2 + acv + dstir + d2bot / 2;
                    baseY = -h / 2 + acv + dstir + d1bot + tw + d2bot / 2;
                    dmain = d2bot;
                    k = i - n1bot;
                }
                else // 3rd
                {
                    delta = (b - 2 * (acv + dstir + d3bot / 2)) / (Math.Max(n3bot, 2) - 1);
                    baseX = -b / 2 + acv + dstir + d3bot / 2;
                    baseY = -h / 2 + acv + dstir + d1bot + tw + d2bot + tw + d3bot / 2;
                    dmain = d3bot;
                    k = i - (n1bot + n2bot);
                }
                int[] pos = new int[3];
                pos[0] = Convert.ToInt32(baseX + delta * k);
                pos[1] = Convert.ToInt32(baseY);
                pos[2] = Convert.ToInt32(dmain);
                listBot.Add(pos);
            }

            return (listTop, listBot);
        }

    }
}
