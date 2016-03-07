using System;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using Stratosphere.Math.Matrix;

namespace Stratosphere.MachineLearning.Studio
{
    public static class PlotModelExtensions
    {
        public static void Scatter(this PlotModel plot, Matrix x, Matrix y, MarkerType marker = MarkerType.Cross)
        {
            var scatterSeries = new ScatterSeries() { MarkerType = marker };
            for (int row = 0; row < x.Height; ++row)
                scatterSeries.Points.Add(new ScatterPoint(x[row, 0], y[row, 1], 5, 1));

            
            plot.Series.Add(scatterSeries);
        }

        public static void Function(this PlotModel plot, Matrix X, Matrix y, Func<double, double> f)
        {
            double maxX = X.Max();
            double minX = X.Min();
            double maxY = y.Max();

            double step = (maxX - minX)/100;


            var lineSeries = new LineSeries();


            for (double x = minX, fx = f(x); x < maxX && fx < maxY; x += step, fx = f(x))
            {
                lineSeries.Points.Add(new DataPoint(x, f(x)));
            }


            lineSeries.Color = OxyPalettes.Hot(3).Colors.First();
            lineSeries.Title = "Regression";

            plot.Series.Add(lineSeries);
        }

        public static void Line(this PlotModel plot, Matrix X, Matrix y, Matrix lineCoefficients)
        {
            Function(plot, X, y, x => lineCoefficients[0] + x * lineCoefficients[1]);
        }
    }
}