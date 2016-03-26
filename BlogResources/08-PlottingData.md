# Plotting data with OxyPlot

Startosphere.NET has a simple WinForms application for visualizing some of the algorithms. To create data charts I used [OxyPlot](<http://oxyplot.org/>) library - it's open source and cross platform.

With OxyPlot, you start with create an instance of `PlotModel`:

    var model = new PlotModel { Title = "f(x) = x^2" };
    
## LineSeries

Model can have multiple data series. To draw a line chart we'll use `LineSeries` class:

    var lineSeries = new LineSeries ();

We can then add data:

    for (double x = -2, fx = x*x; x <= 2; x += 0.01, fx = x* x)
    {
        lineSeries.Points.Add(new DataPoint(x, fx));
    }

When finished, register the series in the plot

    plot.Series.Add(lineSeries);

And finally, display the plot - this platform specific. Following is the WinForms code:

    var plotView = new PlotView ();
    plotView.Dock = DockStyle.Fill;
    plotView.Location = new Point(0, 0);
    plotView.Margin = new Padding(0);
    plotView.Name = "plotView";
    plotView.Model = model;
    
    Controls.Add (plotView);

When run, this gives a nice chart:

![LineSeries](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-03-25_15-10-59.png)


## Scatter Series:

Scatter series let's you define a set o points to visualize. This very handy to visualize simple data sets:

    public static ScatterSeries Scatter(this PlotModel plot, Matrix x, Matrix y, MarkerType marker = MarkerType.Cross)
    {
        var scatterSeries = new ScatterSeries() { MarkerType = marker, MarkerStrokeThickness = 3};
        for (int row = 0; row < x.Height; ++row)
            scatterSeries.Points.Add(new ScatterPoint(x[row, 0], y[row, 0], 5, 1));
    
        plot.Series.Add(scatterSeries);
    
        return scatterSeries;
    }
    
    
 
![ScatterSeries](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-03-25_15-14-09.png)

## Multiple Series

Since you can add as many series as you want, it's easy to combine them into single chart. For Linear Regression this can be used to display the data set and corresponding linear regression model:

    model.Scatter(diameters, y);
    
    var theta = LinearRegression(diameters, y);
    model.Function(diameters, x => theta[0] + theta[1] * x); // Creates LineSeries

![Combined Series](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-03-20_11-17-38.png)
