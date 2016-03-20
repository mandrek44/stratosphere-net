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
            var scatterSeries = new ScatterSeries() { MarkerType = marker, MarkerStrokeThickness = 3};
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
    
        private static LineSeries Function(PlotModel plot, double minX, double maxX, Func<double, double> f)
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

        public static void Line(this PlotModel plot, Matrix X, Matrix y, Matrix lineCoefficients)
        {
            Function(plot, X, x => lineCoefficients[0] + x * lineCoefficients[1]);
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

            var resolution = 100;
            var step = (maxX0 - minX0)/resolution;

            
            map.Data = new double[100, (int) ((map.Y1 - map.Y0)/step)];
            model.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.Right,
                Palette = OxyPalettes.HueDistinct(1500),
                HighColor = OxyColors.Gray,
                LowColor = OxyColors.Black
            });

            int xi1 = 0;
            int xi2 = 0;

            for (double x1_ = map.X0; x1_ < map.X1; x1_ += step)
            {
                xi2 = 0;
                for (double x2 = map.Y0; x2 < map.Y1; x2 += step)
                {
                    map.Data[xi1, xi2++] = f(Matrix.Vector(x1_, x2));
                }

                xi1++;
            }

            model.Series.Add(map);

            return map;
        }
    }
}