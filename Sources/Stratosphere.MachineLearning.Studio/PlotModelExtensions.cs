using System;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Stratosphere.Math;

namespace Stratosphere.MachineLearning.Studio
{
    public static class PlotModelExtensions
    {
        public static ScatterSeries Scatter(this PlotModel plot, Matrix x, Matrix y, MarkerType marker = MarkerType.Cross)
        {
            var scatterSeries = new ScatterSeries() { MarkerType = marker, MarkerStrokeThickness = 3 };
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

        public static LineSeries Function(this PlotModel plot, Matrix dx, Func<double, double> f) => Function(plot, dx.Min(), dx.Max(), f);

        public static LineSeries Function(this PlotModel plot, double minX, double maxX, Func<double, double> f)
        {
            double step = (maxX - minX) / 100;
            var lineSeries = new LineSeries();

            for (double x = minX, fx = f(x); x <= maxX; x += step, fx = f(x))
            {
                lineSeries.Points.Add(new DataPoint(x, f(x)));
            }

            lineSeries.Color = OxyPalettes.Hot(3).Colors.First();

            plot.Series.Add(lineSeries);

            return lineSeries;
        }

        public static LineSeries Polynomial(this PlotModel plot, Matrix X, Matrix coefficients)
        {
            return Function(plot, X, x =>
            {
                double polynomialValue = 0;
                double xPower = 1;

                for (int i = 0; i < coefficients.Height; ++i)
                {
                    polynomialValue += coefficients[i] * xPower;
                    xPower *= x;
                }

                return polynomialValue;
            });
        }

        public static HeatMapSeries HeatMap(this PlotModel model, Matrix dx1, Matrix dx2, Func<Matrix, double> f) =>
            HeatMap(model, dx1.Min(), dx1.Max(), dx2.Min(), dx2.Max(), f);

        public static HeatMapSeries HeatMap(this PlotModel model, double minX0, double maxX0, double minX1, double maxX1, Func<Matrix, double> f)
        {
            var map = new HeatMapSeries
            {
                X0 = minX0,
                X1 = maxX0,
                Y0 = minX1,
                Y1 = maxX1,
                Interpolate = false,
                RenderMethod = HeatMapRenderMethod.Rectangles
            };

            model.Series.Add(map);

            var x0Resolution = 200;
            var step = (maxX0 - minX0) / x0Resolution;
            var x1Resolution = (int)((map.Y1 - map.Y0) / step);

            map.Data = new double[x0Resolution, x1Resolution];


            int x0Index = 0;

            for (double x0 = minX0; x0 < maxX0; x0 += step)
            {
                var x1Index = 0;
                for (double x1 = minX1; x1 < maxX1; x1 += step)
                {
                    map.Data[x0Index, x1Index++] = f(Matrix.Vector(x0, x1));
                }

                x0Index++;
            }

            model.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.HueDistinct(1500),

            });

            return map;
        }
    }
}