using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using Ext = CrossStruc.Extensions.Other;

namespace CrossStruc.ConcreteColumn.Function
{
    public class ChartSectColumn
    {
        public ChartSectColumn(string secShape, string mLayer, int tw, double Cx, double Cy,
                            int nx, int ny, double acv, int dmain, int dstir) // Column section chart
        {
            PlotSect = new PlotModel();

            // Ratio scale rebar
            double sc;
            if (Math.Max(Cx, Cy) >= 1500)
            {
                acv = acv * 2;
                sc = 0.4;
            }
            else if ((Math.Max(Cx, Cy) >= 900) & (Math.Max(Cx, Cy) < 1500))
            {
                acv = acv * 1.5;
                sc = 0.46;
            }
            else if ((Math.Max(Cx, Cy) < 900) & (Math.Max(Cx, Cy) >= 500))
            {
                acv = acv * 1.25;
                sc = 0.52;
            }
            else
            {
                acv = acv * 1;
                sc = 0.58;
            }

            // Ratio scale section
            double ratioX; double ratioY;
            if (Cx < 300)
            {
                ratioX = 2;
            }
            else if ((Cx >= 300) && (Cx < 500))
            {
                ratioX = 1.2;
            }
            else if ((Cx >= 500) && (Cx < 900))
            {
                ratioX = 1.1;
            }
            else if ((Cx >= 900) && (Cx < 1500))
            {
                ratioX = 1.05;
            }
            else
            {
                ratioX = 1;
            }

            if (Cy < 300)
            {
                ratioY = 2;
            }
            else if ((Cy >= 300) && (Cy < 500))
            {
                ratioY = 1.2;
            }
            else if ((Cy >= 500) && (Cy < 900))
            {
                ratioY = 1.1;
            }
            else if ((Cy >= 900) && (Cy < 1500))
            {
                ratioY = 1.05;
            }
            else
            {
                ratioY = 1;
            }

            var sectSeries = new AreaSeries()
            {
                StrokeThickness = 0,
                Fill = OxyColor.FromRgb(205, 230, 255),
                DataFieldX2 = "X",
                ConstantY2 = 0
            };

            var rebarSeries = new ScatterSeries()
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColor.FromRgb(0, 180, 0),
                MarkerSize = sc * Ext.ConvertRebar(dmain),
            };

            if (secShape == "Rec")
            {
                // Section point
                sectSeries.Points.Add(new DataPoint(-Cx / 2, Cy / 2));
                sectSeries.Points.Add(new DataPoint(Cx / 2, Cy / 2));
                sectSeries.Points.Add(new DataPoint(Cx / 2, -Cy / 2));
                sectSeries.Points.Add(new DataPoint(-Cx / 2, -Cy / 2));
                sectSeries.Points.Add(new DataPoint(-Cx / 2, Cy / 2));
            }
            else
            {
                Cy = Cx; // Handle circle section
                for (int k = 0; k <= 50; k++)
                {
                    double x = Cx / 2 * Math.Cos(k * 2 * Math.PI / 50);
                    double y = Cx / 2 * Math.Sin(k * 2 * Math.PI / 50);
                    sectSeries.Points.Add(new DataPoint(x, y));
                }
            }

            List<int[]> rebarPosition = ElementPosition.Rebar(secShape, mLayer, tw, Cx, Cy, nx, ny, acv, dmain, dstir);
            foreach (int[] item in rebarPosition)
            {

                rebarSeries.Points.Add(new ScatterPoint(item[0], item[1]));
            }

            // Scale axis
            PlotSect.PlotAreaBorderColor = OxyColors.Transparent;
            PlotSect.Axes.Add(new LinearAxis()
            {
                IsAxisVisible = false,
                Position = AxisPosition.Left,
                Minimum = Math.Min(-ratioY * Cy / 2, -ratioX * Cx / 2),
                Maximum = Math.Max(ratioY * Cy / 2, ratioX * Cx / 2)
            });

            PlotSect.Axes.Add(new LinearAxis()
            {
                IsAxisVisible = false,
                Position = AxisPosition.Bottom,
                Minimum = Math.Min(-ratioX * Cx / 2, -ratioY * Cy / 2),
                Maximum = Math.Max(ratioX * Cx / 2, ratioY * Cy / 2)
            });
            PlotSect.Series.Add(sectSeries);
            PlotSect.Series.Add(rebarSeries);
        }
        public PlotModel PlotSect { get; private set; }

    }
}
