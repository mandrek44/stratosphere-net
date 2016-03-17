﻿using System;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using Stratosphere.Math;

namespace Stratosphere.MachineLearning.Studio
{
    public static class PlotModelExtensions
    {
        public static ScatterSeries Scatter(this PlotModel plot, Matrix x, Matrix y, MarkerType marker = MarkerType.Cross)
        {
            var scatterSeries = new ScatterSeries() { MarkerType = marker };
            for (int row = 0; row < x.Height; ++row)
                scatterSeries.Points.Add(new ScatterPoint(x[row, 0], y[row, 0], 5, 1));


            plot.Series.Add(scatterSeries);

            return scatterSeries;
        }

        public static ScatterSeries Point(this PlotModel plot, double x, double y, MarkerType marker = MarkerType.Cross)
        {
            var scatterSeries = new ScatterSeries() { MarkerType = marker };

            scatterSeries.Points.Add(new ScatterPoint(x, y, 5, 1));

            plot.Series.Add(scatterSeries);

            return scatterSeries;
        }

        public static LineSeries Function(this PlotModel plot, Matrix X, Matrix y, Func<double, double> f)
        {
            double maxX = X.Max();
            double minX = X.Min();
            double maxY = y.Max();

            double step = (maxX - minX) / 100;


            var lineSeries = new LineSeries();


            for (double x = minX, fx = f(x); x < maxX && fx <= maxY; x += step, fx = f(x))
            {
                lineSeries.Points.Add(new DataPoint(x, f(x)));
            }


            lineSeries.Color = OxyPalettes.Hot(3).Colors.First();

            plot.Series.Add(lineSeries);

            return lineSeries;
        }

        public static void Line(this PlotModel plot, Matrix X, Matrix y, Matrix lineCoefficients)
        {
            Function(plot, X, y, x => lineCoefficients[0] + x * lineCoefficients[1]);
        }
    }
}