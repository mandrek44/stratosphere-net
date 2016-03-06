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
    }
}