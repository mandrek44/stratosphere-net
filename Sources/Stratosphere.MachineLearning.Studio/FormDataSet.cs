using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using Stratosphere.Math;
using Stratosphere.Math.Optimization;
using static System.Math;

namespace Stratosphere.MachineLearning.Studio
{
    public partial class FormDataSet : Form
    {
        public FormDataSet()
        {
            InitializeComponent();
            Size = new Size(800, 600);

            //var model = XSquared();
            var model = RegressionTests();
            //var model = PlotBananaFunction();

            var plotView = new PlotView();
            plotView.Dock = DockStyle.Fill;
            plotView.Location = new Point(0, 0);
            plotView.Margin = new Padding(0);
            plotView.Name = "plotView";
            plotView.Model = model;

            Controls.Add(plotView);
        }

        private static double BananaFunction(Matrix x)
        {
            var a = x[1] - (x[0] * x[0]);
            var b = 1 - x[0];

            return 100 * a * a + b * b;
        }

        private static Matrix BananaFunctionDerivatives(Matrix x)
        {
            var dx = x[0] * (400 * x[0] * x[0] + 2) - 400 * x[0] * x[1] - 2;
            var dy = 200 * x[1] - 200 * x[0] * x[0];
            return Matrix.Vector(dx, dy);
        }

        private static PlotModel PlotBananaFunction()
        {
            var model = new PlotModel();

            model.HeatMap(-2.0, 2.0, -1, 3, x => Log(BananaFunction(x)));

            //PlotSteepestDescent(model);
            //PlotSteepestDescentWithBacktracking(model);
            PlotQuasiNewtonWithBacktracking(model);

            return model;
        }

        private static void PlotSteepestDescentWithBacktracking(PlotModel model)
        {
            var findMethod = new BacktrackingSteepestDescentMethod(trackProgres: true, maxIterations: 1500);
            findMethod.Find(BananaFunction, BananaFunctionDerivatives, "-2;0");

            var historyLine = new LineSeries() { MarkerType = MarkerType.Cross };
            var historyPoints = new ScatterSeries() { MarkerType = MarkerType.Cross, MarkerStrokeThickness = 5 };
            double lastF = double.MaxValue;
            foreach (var historyX in findMethod.Tracker.History)
            {
                var f = BananaFunction(historyX);
                historyPoints.Points.Add(new ScatterPoint(historyX[0], historyX[1], value: f < lastF + 0.001 ? -30 : -50));
                historyLine.Points.Add(new DataPoint(historyX[0], historyX[1]));

                lastF = f;
            }

            model.Series.Add(historyPoints);
            model.Series.Add(historyLine);
        }

        private static void PlotQuasiNewtonWithBacktracking(PlotModel model)
        {
            var findMethod = new QuasiNewtonMethod(trackProgres: true, maxIterations: 1500);
            findMethod.Find(BananaFunction, BananaFunctionDerivatives, "-2;0");

            var historyLine = new LineSeries() { MarkerType = MarkerType.Cross };
            var historyPoints = new ScatterSeries() { MarkerType = MarkerType.Cross, MarkerStrokeThickness = 5 };
            double lastF = double.MaxValue;
            foreach (var historyX in findMethod.Tracker.History)
            {
                var f = BananaFunction(historyX);
                historyPoints.Points.Add(new ScatterPoint(historyX[0], historyX[1], value: f < lastF + 0.001 ? -30 : -50));
                historyLine.Points.Add(new DataPoint(historyX[0], historyX[1]));

                lastF = f;
            }

            model.Series.Add(historyPoints);
            model.Series.Add(historyLine);
        }

        private static void PlotSteepestDescent(PlotModel model)
        {
            var findMethod = new SimpleSteepestDescentMethod(trackProgres: true, maxIterations: 1500);
            findMethod.Find(BananaFunction, BananaFunctionDerivatives, "-2;0", 0.001);

            var historyPoints = new ScatterSeries() { MarkerType = MarkerType.Cross };
            foreach (var historyX in findMethod.History)
            {
                historyPoints.Points.Add(new ScatterPoint(historyX[0], historyX[1], value: -40));
            }

            model.Series.Add(historyPoints);
        }

        private static PlotModel XSquared()
        {
            var model = new PlotModel { Title = "f(x) = x^2", LegendFontSize = 20.5, LegendPosition = LegendPosition.TopCenter };

            model.Function(-2, 2, v => v * v);

            //var X = Matrix.Vector(Enumerable.Range(0, 101).Select(i => -2d + i / 25d).ToArray());


            //Func<Matrix, double> f = x => x[0] * x[0];
            //Func<Matrix, Matrix> df = x => 2 * x;

            //var x0 = Matrix.Vector(-2);
            //var y = X.Map(x => f(Matrix.Vector(x))).Evaluate();
            //var dy = X.Map(x => df(Matrix.Vector(x))).Evaluate();

            //var findMethod = new SimpleSteepestDescentMethod(trackProgres: true);
            //var xmin = SimpleSteepestDescentMethod.Find(
            //    f: x => x[0] * x[0],
            //    df: x => 2 * x,
            //    x0: Matrix.Scalar(-2),
            //    alpha: 0.95,
            //    maxIterations: 1000);

            //var plot_f = model.Function(X, x => x * x);

            ////model.Function(X, dy, x => 0).Color = OxyPalettes.Hot(4).Colors[2];
            //var lineSeries = new LineSeries() { Color = OxyPalettes.Hot(3).Colors[1] };

            //for (int i = 0; i < findMethod.History.Count; ++i)
            //{
            //    var x = findMethod.History[i];
            //    lineSeries.Points.Add(new DataPoint(x, f(x)));
            //}

            //model.Series.Add(lineSeries);

            //model.Scatter(Matrix.Vector(findMethod.History.Select(x => x[0]).ToArray()), Matrix.Vector(findMethod.History.Select(xi => xi[0] * xi[0]).ToArray()));
            //model.Point(xmin, f(xmin), MarkerType.Circle);

            ////var plot_df = model.Function(X, dy, x => 2 * x);
            ////plot_df.Title = "df(x)";
            ////plot_df.Color = OxyPalettes.Hot(3).Colors[1];
            //plot_f.Title = "f(x)";
            //plot_f.Color = OxyPalettes.Hot(3).Colors[0];

            //model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Hot(3) });
            return model;
        }


        private static PlotModel RegressionTests()
        {
            var planetsData = ColumnMajorMatrix.Parse(File.ReadAllText(@"DataSets\swapi_planets_filtered.txt"));
            var model = new PlotModel { Title = "Star Wars Planets (diameter vs period)" };

            var diameters = planetsData.GetColumn(0) * 0.001;
            var periods = planetsData.GetColumn(1) * 0.1;
            var y = periods.Evaluate();

            model.Scatter(diameters, y);

            var theta = LinearRegression(diameters, y);

            var line = model.Function(diameters, x => theta[0] + theta[1] * x);

            //line.Title = ComputeCost(X, y, theta).ToString("0.0000");
            line.Color = OxyPalettes.Hot(3).Colors[0];

            PolynomialRegression(diameters, y, model);
            LinearRegression(diameters, y, model);

            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Hot(3) });
            return model;
        }

        private static void PolynomialRegression(Matrix diameters, Matrix y, PlotModel model)
        {
            var X = Matrix.Ones(diameters.Height, 1)
                .Concat(diameters)
                .Concat(diameters.Map(x => x * x))
                .Concat(diameters.Map(x => x * x * x))
                .Evaluate();

            var theta = QuasiNewtonMethod.Find(
                _theta => ComputeCost(X, y, _theta),
                _theta => Gradient(X, y, _theta),
                Matrix.Ones(X.Width, 1),
                1,
                1000);

            var line = model.Function(diameters, x => theta[0] + theta[1] * x + theta[2] * x * x + theta[3] * x *x * x);

            line.Title = ComputeCost(X, y, theta).ToString("0.0000");
            line.Color = OxyPalettes.Hot(3).Colors[1];
        }

        private static void LinearRegression(Matrix diameters, Matrix y, PlotModel model)
        {
            var theta = LinearRegression(diameters, y);

            var line = model.Function(diameters, x => theta[0] + theta[1] * x);

            line.Title = ComputeCost(Matrix.Ones(diameters.Height, 1).Concat(diameters).Evaluate(), y, theta).ToString("0.0000");
            line.Color = OxyPalettes.Hot(3).Colors[0];
        }

        private static Matrix LinearRegression(Matrix diameters, Matrix y)
        {
            var X = Matrix.Ones(diameters.Height, 1).Concat(diameters).Evaluate();
            return QuasiNewtonMethod.Find(
                f: theta => ComputeCost(X, y, theta),
                df: theta => Gradient(X, y, theta),
                x0: Matrix.Ones(X.Width, 1),
                alpha: 0.0001,
                maxIterations: 1000);
        }

        private static double ComputeCost(Matrix X, Matrix y, Matrix theta)
        {
            var m = y.Height;
            var error = X * theta - y;
            return error.Map(v => v * v).Sum() / (2.0 * m);
        }

        private static Matrix Gradient(Matrix X, Matrix y, Matrix theta)
        {
            var error = (X * theta - y);
            var a = (Matrix.Ones(theta.Height, 1) * error.Transpose()).Transpose().MultiplyEach(X);
            return a.Sum(0).Transpose();
        }
    }
}
