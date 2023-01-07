using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;

namespace CrossStruc.ConcreteColumn.Function
{
    public class ChartIDColumn
    {
        public ChartIDColumn(string mode, double[,] outputPMxy, double[,] outputMxMy, int Muxy, int P, int Mx_up, int My_up, double Cx, double Cy) // ID cut chart
        {
            PlotIDCut = new PlotModel()
            {
                PlotAreaBorderColor = OxyColors.Transparent,
            };

            var IDSurfaceSeries = new AreaSeries()
            {
                StrokeThickness = 3,
                LineJoin = LineJoin.Round,
                Fill = OxyColor.FromRgb(255, 204, 153),
                DataFieldX2 = "X",
                ConstantY2 = 0
            };

            var casePoints = new ScatterSeries()
            {
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColor.FromRgb(51, 102, 255),
                MarkerSize = 5,
            };

            int maxX; int minX;
            int maxY; int minY;

            // Chart PMxy
            if (mode == "P-Mxy")
            {
                for (int i = 0; i <= outputPMxy.GetUpperBound(0); i++)
                {
                    double x = Math.Round(outputPMxy[i, 1], 0);
                    double y = Math.Round(outputPMxy[i, 0], 0);
                    IDSurfaceSeries.Points.Add(new DataPoint(x, y));
                }
                casePoints.Points.Add(new ScatterPoint(Muxy, P));
                minX = 0;
                maxX = Convert.ToInt32(1.1 * Math.Max(Muxy, SubExtensions.FindMaxMin(outputPMxy).Item3));
                minY = Convert.ToInt32(1.1 * Math.Min(P, SubExtensions.FindMaxMin(outputPMxy).Item2));
                maxY = Convert.ToInt32(1.2 * Math.Max(P, SubExtensions.FindMaxMin(outputPMxy).Item1));
            }
            else
            {
                for (int i = 0; i <= outputMxMy.GetUpperBound(0); i++)
                {
                    double x = Math.Round(outputMxMy[i, 0], 0);
                    double y = Math.Round(outputMxMy[i, 1], 0);
                    IDSurfaceSeries.Points.Add(new DataPoint(x, y));
                }
                casePoints.Points.Add(new ScatterPoint(Mx_up, My_up));

                double ratio_X; double ratio_Y;
                if (Cy / Cx > 4) ratio_X = 2;
                else if ((Cy / Cx <= 4) && (Cy / Cx > 2)) ratio_X = 1.7;
                else if ((Cy / Cx <= 2) && (Cy / Cx > 1)) ratio_X = 1.3;
                else ratio_X = 1;
                if (Cx / Cy > 4) ratio_Y = 2;
                else if ((Cx / Cy <= 4) && (Cx / Cy > 2)) ratio_Y = 1.7;
                else if ((Cx / Cy <= 2) && (Cx / Cy > 1)) ratio_Y = 1.3;
                else ratio_Y = 1;
                minX = 0;
                maxX = Convert.ToInt32(1.1 * Math.Max(Math.Max(Mx_up, SubExtensions.FindMaxMin(outputMxMy).Item1), 20) * ratio_X);
                minY = 0;
                maxY = Convert.ToInt32(1.1 * Math.Max(Math.Max(My_up, SubExtensions.FindMaxMin(outputMxMy).Item3), 20) * ratio_Y);
            }

            int stepX = Math.Max((maxX - minX) / 4 / 10 * 10, 10);
            int stepY = Math.Max((maxY - minY) / 4 / 10 * 10, 10);

            PlotIDCut.Axes.Add(new LinearAxis()
            {
                IsAxisVisible = true,
                Position = AxisPosition.Bottom,
                AxislineStyle = LineStyle.Solid,
                AxislineThickness = 1,
                MinorTickSize = 0,
                MajorTickSize = 2,
                MajorGridlineStyle = LineStyle.LongDash,
                MajorGridlineThickness = 1,
                MajorStep = stepX,
                Minimum = minX,
                Maximum = maxX
            }); ;

            PlotIDCut.Axes.Add(new LinearAxis()
            {
                IsAxisVisible = true,
                Position = AxisPosition.Left,
                AxislineStyle = LineStyle.Solid,
                AxislineThickness = 1,
                Angle = -90,
                MinorTickSize = 0,
                MajorTickSize = 2,
                MajorGridlineThickness = 1,
                MajorGridlineStyle = LineStyle.LongDash,
                MajorStep = stepY,
                Minimum = minY,
                Maximum = maxY
            });


            PlotIDCut.Series.Add(IDSurfaceSeries);
            PlotIDCut.Series.Add(casePoints);
        }

        public PlotModel PlotIDCut { get; private set; }

    }
}
