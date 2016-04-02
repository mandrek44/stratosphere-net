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
        }

        public void Display(PlotModel model)
        {
            var plotView = new PlotView();
            plotView.Dock = DockStyle.Fill;
            plotView.Location = new Point(0, 0);
            plotView.Margin = new Padding(0);
            plotView.Name = "plotView";
            plotView.Model = model;

            Controls.Add(plotView);
        }

        public static PlotModel PlotBananaFunction()
        {
            var model = new PlotModel();

            model.HeatMap(-2.0, 2.0, -1, 3, x => Log(BananaFunction(x)));

            //PlotOptimizationSteps(model, new SimpleSteepestDescentMethod(trackProgres: true, maxIterations: 1500));
            //PlotOptimizationSteps(model, new BacktrackingSteepestDescentMethod(trackProgres: true, maxIterations: 1500));
            PlotOptimizationSteps(model, new QuasiNewtonMethod(trackProgres: true, maxIterations: 1500));

            return model;
        }

        public static PlotModel PlotBananaFunction(IOptimizationMethod method)
        {
            var model = new PlotModel();

            model.HeatMap(-2.0, 2.0, -1, 3, x => Log(BananaFunction(x)));
            PlotOptimizationSteps(model, method);

            return model;
        }

        public static PlotModel XSquared()
        {
            var model = new PlotModel { Title = "f(x) = x^2", LegendFontSize = 20.5, LegendPosition = LegendPosition.TopCenter };

            model.Function(-2, 2, v => v * v);

            return model;
        }

        public static PlotModel RegressionTests()
        {
            var planetsData = ColumnMajorMatrix.Parse(File.ReadAllText(@"DataSets\swapi_planets_filtered.txt"));
            var model = new PlotModel { Title = "Star Wars Planets (diameter vs period)" };

            var diameters = planetsData.GetColumn(0) * 0.001;
            var periods = planetsData.GetColumn(1) * 0.1;
            var y = periods.Evaluate();

            model.Scatter(diameters, y);

            var theta = LinearRegression(diameters, y);

            var line = model.Polynomial(diameters, theta);

            line.Color = OxyPalettes.Hot(3).Colors[0];

            PolynomialRegression(diameters, y, model);
            LinearRegression(diameters, y, model);

            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Hot(3) });
            return model;
        }

        private static double BananaFunction(Matrix x)
        {
            var a = x[1] - (x[0] * x[0]);
            var b = 1 - x[0];

            return 100 * a * a + b * b;
        }

        public static Matrix BananaFunctionDerivatives(Matrix x)
        {
            // http://www.wolframalpha.com/input/?i=d%2Fdx(+100*((y+-+x%5E2)%5E2)+%2B+(1-x)%5E2)
            var dx = 2 * (200 * x[0] * x[0] * x[0] - 200 * x[0] * x[1] + x[0] - 1);

            // http://www.wolframalpha.com/input/?i=d%2Fdy(+100*((y+-+x%5E2)%5E2)+%2B+(1-x)%5E2)
            var dy = 200 * (x[1] - x[0] * x[0]);

            return Matrix.Vector(dx, dy);
        }

        public static Matrix BananaFunctionSecondDerivatives(Matrix x)
        {
            // http://www.wolframalpha.com/input/?i=d%2Fdx(+100*((y+-+x%5E2)%5E2)+%2B+(1-x)%5E2)
            var dxx = 1200*x[0]*x[0] - 400*x[1] + 2;
            var dxy = -400*x[0];

            var dyx = -400*x[0];
            var dyy = 200;
            
            return Matrix.Vector(dxx, dyx).Concat(Matrix.Vector(dxy, dyy));
        }

        private static void PlotOptimizationSteps(PlotModel model, IOptimizationMethod findMethod)
        {
            findMethod.Find(BananaFunction, BananaFunctionDerivatives, "-2;0");

            var historyLine = new LineSeries() { MarkerType = MarkerType.Cross };
            var historyPoints = new ScatterSeries() { MarkerType = MarkerType.Cross, MarkerStrokeThickness = 5 };
            double lastF = double.MaxValue;
            var lastHistoryEntry = findMethod.Tracker.History.Last();
            foreach (var historyX in findMethod.Tracker.History)
            {
                var f = BananaFunction(historyX);
                if (historyX != lastHistoryEntry)
                    historyPoints.Points.Add(new ScatterPoint(historyX[0], historyX[1], value: f < lastF + 0.001 ? -30 : -50));
                else
                    historyPoints.Points.Add(new ScatterPoint(historyX[0], historyX[1], value: -40));
                historyLine.Points.Add(new DataPoint(historyX[0], historyX[1]));

                lastF = f;
            }

            model.Series.Add(historyPoints);
            model.Series.Add(historyLine);
        }

        private static void PolynomialRegression(Matrix diameters, Matrix y, PlotModel model)
        {
            var X = Matrix.Ones(diameters.Height, 1)
                .Concat(diameters)
                .Concat(diameters.Map(x => x * x))
                .Concat(diameters.Map(x => x * x * x))
                .Concat(diameters.Map(x => x * x * x * x))
                .Evaluate();

            var theta = QuasiNewtonMethod.Find(
                _theta => ComputeCost(X, y, _theta),
                _theta => Gradient(X, y, _theta),
                Matrix.Ones(X.Width, 1),
                1,
                1000);

            var line = model.Polynomial(diameters, theta);

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
