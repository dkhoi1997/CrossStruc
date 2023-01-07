using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossStruc.ConcreteColumn.Function
{
    public class ElementPosition
    {
        public static List<double[]> Concrete(string secShape, double Cx, double Cy, int di) // Mesh concrete column section
        {
            List<double[]> listConc = new List<double[]>();

            if (secShape == "Rec")
            {
                for (int i = 0; i < Convert.ToInt32(Cy / di); i++)
                {
                    for (int j = 0; j < Convert.ToInt32(Cx / di); j++)
                    {
                        double[] point = new double[3];
                        point[0] = Cx / 2 - di / 2 - di * j;
                        point[1] = Cy / 2 - di / 2 - di * i;
                        point[2] = di * di;
                        listConc.Add(point);
                    }
                }
            }
            else
            {
                double D = Cx;
                for (int i = 0; i < Convert.ToInt32(D / di); i++)
                {
                    for (int j = 0; j < Convert.ToInt32(D / di); j++)
                    {
                        double[] point = new double[3];
                        point[0] = D / 2 - di / 2 - di * j;
                        point[1] = D / 2 - di / 2 - di * i;
                        point[2] = di * di;
                        if (Math.Sqrt(Math.Pow(point[0], 2) + Math.Pow(point[1], 2)) <= D / 2) listConc.Add(point);
                    }
                }
            }
            return listConc;
        }

        public static List<int[]> Rebar(string secShape, string mLayer, int tw, double Cx, double Cy,
                int nx, int ny, double offset, int dmain, int dstir) // Get rebar coordinate
        {
            List<int[]> listRebar = new List<int[]>();
            Dictionary<string, int[]> dictRebar = new Dictionary<string, int[]>();
            if (secShape == "Rec")
            {

                double CxCen = Cx - 2 * offset - 2 * dstir - dmain;
                double CyCen = Cy - 2 * offset - 2 * dstir - dmain;
                if (nx == 1) nx = 2;
                if (ny == 1) ny = 2;

                int setX = 2;
                int switchX;
                int gapver;
                int gapX;

                // X-layer
                for (int i = 0; i < nx; i++)
                {
                    if ((mLayer == "X") || (mLayer == "XY"))
                    {
                        setX = 4;
                    }
                    for (int j = 1; j <= setX; j++)
                    {
                        // 1st layer
                        int[] pt = new int[2];
                        if (j <= 2)
                        {
                            gapver = 0;
                            gapX = 0;
                            if (j == 1)
                            {
                                switchX = 1;
                            }
                            else
                            {
                                switchX = -1;
                            }
                        }
                        // 2nd layer
                        else
                        {
                            if (j == 3)
                            {
                                switchX = 1;
                                gapver = -tw;
                            }
                            else
                            {
                                switchX = -1;
                                gapver = tw;
                            }
                            if (mLayer == "XY")
                            {
                                if (i == 0)
                                {
                                    gapX = tw;
                                }
                                else if (i == nx - 1)
                                {
                                    gapX = -tw;
                                }
                                else
                                {
                                    gapX = 0;
                                }
                            }
                            else
                            {
                                gapX = 0;
                            }
                        }
                        pt[0] = Convert.ToInt32(-CxCen / 2 + CxCen / (nx - 1) * i + gapX);
                        pt[1] = Convert.ToInt32(switchX * CyCen / 2 + gapver);
                        string combineX = Convert.ToString(pt[0]) + Convert.ToString(pt[1]);
                        if (dictRebar.ContainsKey(combineX) == false)
                        {
                            dictRebar.Add(combineX, pt);
                        }
                    }
                }

                int setY = 2;
                int switchY;
                int gaphoz;
                int gapY;

                // Y-Layer
                for (int i = 0; i < ny; i++)
                {
                    if ((mLayer == "Y") || (mLayer == "XY"))
                    {
                        setY = 4;
                    }
                    for (int j = 1; j <= setY; j++)
                    {
                        // 1st layer
                        int[] pt = new int[2];
                        if (j <= 2)
                        {
                            gaphoz = 0;
                            gapY = 0;
                            if (j == 1)
                            {
                                switchY = 1;
                            }
                            else
                            {
                                switchY = -1;
                            }
                        }
                        // 2nd Layer
                        else
                        {
                            if (j == 3)
                            {
                                switchY = 1;
                                gaphoz = -tw;
                            }
                            else
                            {
                                switchY = -1;
                                gaphoz = tw;
                            }
                            if (mLayer == "XY")
                            {
                                if (i == 0)
                                {
                                    gapY = tw;
                                }
                                else if (i == ny - 1)
                                {
                                    gapY = -tw;
                                }
                                else
                                {
                                    gapY = 0;
                                }
                            }
                            else
                            {
                                gapY = 0;
                            }
                        }
                        pt[0] = Convert.ToInt32(switchY * CxCen / 2 + gaphoz);
                        pt[1] = Convert.ToInt32(-CyCen / 2 + CyCen / (ny - 1) * i + gapY);
                        string combineY = Convert.ToString(pt[0]) + Convert.ToString(pt[1]);
                        if (dictRebar.ContainsKey(combineY) == false)
                        {
                            dictRebar.Add(combineY, pt);
                        }
                    }
                }
            }
            else
            {
                double D = Cx;
                int sumn = nx;

                double Dcen = D - 2 * offset - 2 * dstir - dmain;

                byte setCir = 1;
                int gap;

                if (mLayer == "XY")
                {
                    setCir = 2;
                }

                for (int i = 0; i < sumn; i++)
                {
                    for (int j = 1; j <= setCir; j++)
                    {
                        int[] pt = new int[2];
                        if (j == 1)
                        {
                            gap = 0;
                        }
                        else
                        {
                            gap = tw;
                        }
                        pt[0] = Convert.ToInt32(Math.Sin(2 * Math.PI / sumn * i) * ((Dcen - gap) / 2));
                        pt[1] = Convert.ToInt32(Math.Cos(2 * Math.PI / sumn * i) * ((Dcen - gap) / 2));
                        string combineCir = Convert.ToString(pt[0]) + Convert.ToString(pt[1]);
                        if (dictRebar.ContainsKey(combineCir) == false)
                        {
                            dictRebar.Add(combineCir, pt);
                        }
                    }
                }
            }
            foreach (KeyValuePair<string, int[]> item in dictRebar)
            {
                int[] temp = new int[3];
                temp[0] = item.Value[0];
                temp[1] = item.Value[1];
                temp[2] = dmain;
                listRebar.Add(temp);
            }
            return listRebar;
        }
    }
}
