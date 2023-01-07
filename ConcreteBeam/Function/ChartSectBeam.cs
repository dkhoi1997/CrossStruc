using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ext = CrossStruc.Extensions.Other;

namespace CrossStruc.ConcreteBeam.Function
{
    public class ChartSectBeam
    {
        public ChartSectBeam(int b, int h, bool Tsec, bool Tsecrevert, int hs, double acv, double tw,
            int n1top, int d1top, int n2top, int d2top, int n3top, int d3top,
            int n1bot, int d1bot, int n2bot, int d2bot, int n3bot, int d3bot, int dstir) // Beam section chart
        {
            PlotSect = new PlotModel();

            // Ratio scale rebar
            double sc;
            if (Math.Max(b, h) >= 1500)
            {
                acv = acv * 1.7;
                tw = tw * 1.7;
                sc = 0.12;
            }
            else if ((Math.Max(b, h) < 1500) & (Math.Max(b, h) >= 1200))
            {
                acv = acv * 1.5;
                tw = tw * 1.5;
                sc = 0.2;
            }
            else if ((Math.Max(b, h) < 1200) & (Math.Max(b, h) >= 900))
            {
                acv = acv * 1.3;
                tw = tw * 1.3;
                sc = 0.28;
            }
            else if ((Math.Max(b, h) < 900) & (Math.Max(b, h) >= 500))
            {
                acv = acv * 1.1;
                tw = tw * 1.1;
                sc = 0.36;
            }
            else
            {
                acv = acv * 0.9;
                tw = tw * 0.9;
                sc = 0.4;
            }

            // Scale axis
            double ratiox; double ratioy;
            if (b < 400)
            {
                ratiox = 1.2;
            }
            else if ((b >= 400) && (b < 600))
            {
                ratiox = 1.1;
            }
            else
            {
                ratiox = 1;
            }

            if (h < 400)
            {
                ratioy = 1.2;
            }
            else if ((h >= 400) && (h < 600))
            {
                ratioy = 1.1;
            }
            else
            {
                ratioy = 1;
            }

            AreaSeries sectSeries = new AreaSeries()
            {
                StrokeThickness = 0,
                Fill = OxyColor.FromRgb(205, 230, 255),
                DataFieldX2 = "X",
                ConstantY2 = 0

            };

            // Section point
            double flange = 0;
            if (Tsec == true)
            {
                flange = 1 * hs;
                if (Tsecrevert == true) // Inverted T-sect
                {

                    sectSeries.Points.Add(new DataPoint(b / 2, h / 2));
                    sectSeries.Points.Add(new DataPoint(b / 2, -h / 2 + hs));
                    sectSeries.Points.Add(new DataPoint(b / 2 + flange, -h / 2 + hs));
                    sectSeries.Points.Add(new DataPoint(b / 2 + flange, -h / 2));
                    sectSeries.Points.Add(new DataPoint(-b / 2 - flange, -h / 2));
                    sectSeries.Points.Add(new DataPoint(-b / 2 - flange, -h / 2 + hs));
                    sectSeries.Points.Add(new DataPoint(-b / 2, -h / 2 + hs));
                    sectSeries.Points.Add(new DataPoint(-b / 2, h / 2));
                    sectSeries.Points.Add(new DataPoint(b / 2, h / 2));
                }
                else // Normal T-sect
                {
                    sectSeries.Points.Add(new DataPoint(-b / 2, -h / 2));
                    sectSeries.Points.Add(new DataPoint(-b / 2, h / 2 - hs));
                    sectSeries.Points.Add(new DataPoint(-b / 2 - flange, h / 2 - hs));
                    sectSeries.Points.Add(new DataPoint(-b / 2 - flange, h / 2));
                    sectSeries.Points.Add(new DataPoint(b / 2 + flange, h / 2));
                    sectSeries.Points.Add(new DataPoint(b / 2 + flange, h / 2 - hs));
                    sectSeries.Points.Add(new DataPoint(b / 2, h / 2 - hs));
                    sectSeries.Points.Add(new DataPoint(b / 2, -h / 2));
                    sectSeries.Points.Add(new DataPoint(-b / 2, -h / 2));
                }
            }
            else
            {
                // Rectangle
                sectSeries.Points.Add(new DataPoint(-b / 2, -h / 2));
                sectSeries.Points.Add(new DataPoint(-b / 2, h / 2));
                sectSeries.Points.Add(new DataPoint(b / 2, h / 2));
                sectSeries.Points.Add(new DataPoint(b / 2, -h / 2));
                sectSeries.Points.Add(new DataPoint(-b / 2, -h / 2));
            }

            List<int[]> listrebarTop;
            List<int[]> listrebarBot;

            (listrebarTop, listrebarBot) = ElementPosition.Rebar(b, h, acv, tw, dstir, n1top, d1top, n2top, d2top, n3top, d3top, n1bot, d1bot, n2bot, d2bot, n3bot, d3bot);

            List<int[]> listrebar = listrebarTop.Concat(listrebarBot).ToList();

            // Rebar
            ScatterSeries rebarSeries = new ScatterSeries()
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColor.FromRgb(0, 180, 0)
            };
            foreach (int[] item in listrebar)
            {
                rebarSeries.Points.Add(new ScatterPoint(item[0], item[1], sc * Ext.ConvertRebar(item[2])));
            }

            PlotSect.PlotAreaBorderColor = OxyColors.Transparent;
            PlotSect.Axes.Add(new LinearAxis()
            {
                IsAxisVisible = false,
                Position = AxisPosition.Left,
                Minimum = Math.Min(-ratioy * h / 2, -ratiox * (b / 2 + flange)),
                Maximum = Math.Max(ratioy * h / 2, ratiox * (b / 2 + flange))
            });

            PlotSect.Axes.Add(new LinearAxis()
            {
                IsAxisVisible = false,
                Position = AxisPosition.Bottom,
                Minimum = Math.Min(-ratiox * (b / 2 + flange), -ratioy * h / 2),
                Maximum = Math.Max(ratiox * (b / 2 + flange), ratioy * h / 2)
            });

            PlotSect.Series.Add(sectSeries);
            PlotSect.Series.Add(rebarSeries);
        }

        public PlotModel PlotSect { get; private set; }
    }

}

