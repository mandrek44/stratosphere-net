# Plotting data with OxyPlot - Part 2

In [previous post](<http://marcindrobik.pl/Post/PlottingdatawithOxyPlot>) we looked at using [OxyPlot](<http://oxyplot.org/>) for drawing simple functions and datasets. 

Today let's see how we can plot a function of 2 arguments.

## Heat Map

As far as I know, OxyPlot doesn't have any support for plotting 3D objects, so we have to find different way for visualizing functions with 2 arguments.

One neat way for doing it is to plot a heat map of values. It works like in cartography - low values are deep blue. High values are red. Everything in between goes from blue, through green and orange to red. Just like [following map of Poland](<https://commons.wikimedia.org/wiki/File:Regiony_Kondrackiego-hipsometria.png>):

<img src="https://upload.wikimedia.org/wikipedia/commons/3/38/Regiony_Kondrackiego-hipsometria.png" style="width:98%">

## HeatMapSeries
OxyPlot has a data series called [`HeatMapSeries`](<http://docs.oxyplot.org/en/latest/models/series/HeatMapSeries.html>) that let us do exactly that.

First, let's define some context:

    public static HeatMapSeries HeatMap(this PlotModel model, double minX0, double maxX0, double minX1, double maxX1, Func<Matrix, double> f)
    {
    
We want to plot a function f (that accepts Matrix and computes single value) in given range of values. 

First initialize the OxyPlot series:

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
    
Each element of series will be an individual pixel. Let's define the resolution at which we'll evaluation our function, and allocate data for series accordingly:

    var x0Resolution = 200;
    var step = (maxX0 - minX0)/x0Resolution;
    var x1Resolution = (int) ((map.Y1 - map.Y0)/step);
    
    map.Data = new double[x0Resolution, x1Resolution];
    
With preallocated series data, now it's time to fill it in by iterating through each "pixel" on our map and evaluating function value:

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

Finally, let's define how the function value will be translated to color. For this, OxyPlot uses a notion of axises:

    model.Axes.Add(new LinearColorAxis
    {
        Position = AxisPosition.Right,
        Palette = OxyPalettes.HueDistinct(1500)
    });

## "Banana" function

Let's try our method on following function (logarithm of [Rosenbrock function](<https://en.wikipedia.org/wiki/Rosenbrock_function>)):

![](https://mandrostorage.blob.core.windows.net/blogfiles/LogOfRosenbrock.jpg)

First, let's write down the Rosenbrock function itself:

    double BananaFunction(Matrix x)
    {
        var a = x[1] - (x[0] * x[0]);
        var b = 1 - x[0];
    
        return 100 * a * a + b * b;
    }

... and display the Heatmap (using the PlotModel from previous post):


    model.HeatMap(-2.0, 2.0, -1, 3, x => Log(BananaFunction(x)));
    
And here's your Banana:

![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-03-27_21-54-51.png)

(Notice a clearly visible minimum at point `[1, 1]`)