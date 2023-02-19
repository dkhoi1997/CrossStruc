using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossStruc.ConcreteBeam.Function
{
    public class SubExtensions
    {
        public static List<(string[], List<int[]>)> FilterBeamForce(List<(string[], List<int[]>)> listBeamForce, 
            List<int> ULScomb, List<int> SLScomb) // Filter beam force
        {
            List<(string[], List<int[]>)> listFilter = new List<(string[], List<int[]>)>();

            foreach ((string[], List<int[]>) item in listBeamForce)
            {
                // Initial parameter
                int[,] force = new int[2, 6]; // In order to Mtop, Mbot, Q, T
                int pos = 0;

                for (int i = 0; i < item.Item2.Count; i++)
                {
                    int comb = item.Item2[i][0]; // Comb
                    int Qcurrent = item.Item2[i][3];// Q
                    int Tcurrent = item.Item2[i][4]; // T
                    int Mcurrent = item.Item2[i][5]; // M

                    // ULS
                    for (int j = 0; j < ULScomb.Count; j++)
                    {
                        if (comb == ULScomb[j])
                        {
                            if (pos < 8 || pos > 15) // Support position
                            {
                                if (force[0, 0] > Mcurrent) force[0, 0] = Mcurrent;
                                if (force[0, 1] < Mcurrent) force[0, 1] = Mcurrent;
                                if (Math.Abs(force[0, 4]) < Math.Abs(Qcurrent)) force[0, 4] = Qcurrent;
                                if (Math.Abs(force[0, 5]) < Math.Abs(Tcurrent)) force[0, 5] = Tcurrent;
                            }
                            else // Mid position
                            {
                                if (force[1, 0] > Mcurrent) force[1, 0] = Mcurrent;
                                if (force[1, 1] < Mcurrent) force[1, 1] = Mcurrent;
                                if (Math.Abs(force[1, 4]) < Math.Abs(Qcurrent)) force[1, 4] = Qcurrent;
                                if (Math.Abs(force[1, 5]) < Math.Abs(Tcurrent)) force[1, 5] = Tcurrent;
                            }
                        }
                    }

                    // SLS
                    for (int j = 0; j < SLScomb.Count; j++)
                    {
                        if (comb == SLScomb[j])
                        {
                            if (pos < 8 || pos > 15)
                            {
                                if (force[0, 2] > Mcurrent) force[0, 2] = Mcurrent;
                                if (force[0, 3] < Mcurrent) force[0, 3] = Mcurrent;
                            }
                            else
                            {
                                if (force[1, 2] > Mcurrent) force[1, 2] = Mcurrent;
                                if (force[1, 3] < Mcurrent) force[1, 3] = Mcurrent;
                            }
                        }
                    }

                    if (pos < 23) pos++;
                    else pos = 0;
                }

                // Refill data into list
                List<int[]> temp = new List<int[]>();
                for (int i = 0; i < 2; i++)
                {
                    int[] forceSub = new int[6];
                    for (int j = 0; j < 6; j++)
                    {
                        forceSub[j] = force[i, j];

                    }
                    temp.Add(forceSub);
                }
                listFilter.Add((item.Item1, temp));
            }

            return listFilter;
        }

        public static List<(string[], List<int[]>)> FilterEnveBeamForce(List<(string[], List<int[]>)> listBeamForce) // Filter envelop beam force
        {
            List<(string[], List<int[]>)> listFilter = new List<(string[], List<int[]>)>();
            List<int[]> temp = new List<int[]>();
            int[] enveSup = new int[6];
            int[] enveMid = new int[6];

            foreach ((string[], List<int[]>) item in listBeamForce)
            {
                if (enveSup[0] > item.Item2[0][0]) enveSup[0] = item.Item2[0][0]; // Mtop sup
                if (enveSup[1] < item.Item2[0][1]) enveSup[1] = item.Item2[0][1]; // Mbot sup

                if (enveSup[2] > item.Item2[0][2]) enveSup[2] = item.Item2[0][2]; // MtopSLS sup
                if (enveSup[3] < item.Item2[0][3]) enveSup[3] = item.Item2[0][3]; // MbotSLS sup

                if (enveSup[4] > item.Item2[0][4]) enveSup[4] = item.Item2[0][4]; // Q sup
                if (enveSup[5] < item.Item2[0][5]) enveSup[5] = item.Item2[0][5]; // T sup

                if (enveMid[0] > item.Item2[1][0]) enveMid[0] = item.Item2[1][0]; // Mtop mid
                if (enveMid[1] < item.Item2[1][1]) enveMid[1] = item.Item2[1][1]; // Mbot mid

                if (enveMid[2] > item.Item2[1][2]) enveMid[2] = item.Item2[1][2]; // MtopSLS mid
                if (enveMid[3] < item.Item2[1][3]) enveMid[3] = item.Item2[1][3]; // MbotSLS mid

                if (enveMid[4] > item.Item2[1][4]) enveMid[4] = item.Item2[1][4]; // Q mid
                if (enveMid[5] < item.Item2[1][5]) enveMid[5] = item.Item2[1][5]; // T mid
            }

            temp.Add(enveSup);
            temp.Add(enveMid);

            string[] arrNull = new string[6];
            arrNull[0] = "Enve";

            listFilter.Add((arrNull, temp));

            return listFilter;
        }

        public static (double, double, double) RebarProperty(int b, int h, double acv, double tw,
            int n1, int d1, int n2, int d2, int n3, int d3, int dstir) // Calc area and rebar percentage
        {
            double sect = b * h;
            double As1 = n1 * Math.PI * Math.Pow(d1, 2) / 4;
            double As2 = n2 * Math.PI * Math.Pow(d2, 2) / 4;
            double As3 = n3 * Math.PI * Math.Pow(d3, 2) / 4;
            double totalAs = Math.Round(As1 + As2 + As3, 0);
            double percent = Math.Round(100 * totalAs / sect, 2);
            double a1 = acv + dstir + d1 / 2;
            double a2 = acv + dstir + d1 + tw + d2 / 2;
            double a3 = acv + dstir + d1 + tw + d2 + tw + d3 / 2;
            double att = Math.Round((As1 * a1 + As2 * a2 + As3 * a3) / totalAs, 2);
            return (totalAs, percent, att);
        }

        public static double EquivalentDiaRebar(List<int[]> listRebarT, List<int[]> listRebarB, bool revertScan, bool compressBar) // Equivalent rebar diameter of layer
        {
            List<int[]> listRebar = listRebarB;

            // Initial parameter
            double sumSquareRebar = 0;
            double sumRebar = 0;

            if (revertScan == true)
            {
                listRebar = listRebarT;
            }

            if (compressBar == true)
            {
                listRebar = listRebarB.Concat(listRebarT).ToList();
            }
            foreach (int[] item in listRebar)
            {
                sumSquareRebar = sumSquareRebar + Math.Pow(item[2], 2);
                sumRebar = sumRebar + item[2];
            }

            // Equivalent diamater rebar
            double equivDia = sumSquareRebar / sumRebar;

            return equivDia;
        }

        public static double SideAsForTorsional(int n1top, int d1top, int n2top, int d2top, int n3top, int d3top, 
            int n1bot, int d1bot, int n2bot, int d2bot, int n3bot, int d3bot)
        {
            if (n1top > 0) n1top = 1;
            if (n2top > 0) n2top = 1;
            if (n3top > 0) n3top = 1;

            if (n1bot > 0) n1bot = 1;
            if (n2bot > 0) n2bot = 1;
            if (n3bot > 0) n3bot = 1;

            double sideAsTop = Math.PI / 4 * (n1top * Math.Pow(d1top, 2) + n2top * Math.Pow(d2top, 2) + n3top * Math.Pow(d3top, 2));
            double sideAsBot = Math.PI / 4 * (n1bot * Math.Pow(d1bot, 2) + n2bot * Math.Pow(d2bot, 2) + n3bot * Math.Pow(d3bot, 2));
            double sideAs = sideAsTop + sideAsBot;

            return sideAs;

        }

    }
}
