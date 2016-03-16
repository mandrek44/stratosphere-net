using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using Stratosphere.Math;
using Stratosphere.Math.Optimization;

namespace Stratosphere.MachineLearning.Studio
{
    public partial class FormDataSet : Form
    {
        public FormDataSet()
        {
            InitializeComponent();
            Size = new Size(800, 600);

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
            var a = 100 * (x[1] - (x[0] * x[0]));
            var b = 1 - x[0];

            return a * a + b * b;
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

            var map = new HeatMapSeries { X0 = -2.0, X1 = 2.0, Y0 = -1, Y1 = 3, Data = new double[40, 40], Interpolate = false, RenderMethod = HeatMapRenderMethod.Rectangles };

            double step = 0.05;
            int xi1 = 0;
            int xi2 = 0;

            map.Data = new double[(int)((map.X1 - map.X0) / step) + 1, (int)((map.Y1 - map.Y0) / step) + 1];
            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.HueDistinct(1500), HighColor = OxyColors.Gray, LowColor = OxyColors.Black });


            for (double x1 = map.X0; x1 < map.X1; x1 += step)
            {
                xi2 = 0;
                for (double x2 = map.Y0; x2 < map.Y1; x2 += step)
                {
                    map.Data[xi1, xi2++] = System.Math.Log(BananaFunction(Matrix.Vector(x1, x2)));
                }

                xi1++;
            }

            model.Series.Add(map);


            PlotSteepestDescent(model);
            PlotSteepestDescentWithBacktracking(model);

            return model;

        }

        private static void PlotSteepestDescentWithBacktracking(PlotModel model)
        {
            var findMethod = new BacktrackingSteepestDescentMethod(trackProgres: true, maxIterations: 1500);
            findMethod.Find(BananaFunction, BananaFunctionDerivatives, "-2;0");

            var historyPoints = new ScatterSeries() { MarkerType = MarkerType.Cross };
            foreach (var historyX in findMethod.Tracker.History)
            {
                historyPoints.Points.Add(new ScatterPoint(historyX[0], historyX[1], value: -30));
            }

            model.Series.Add(historyPoints);
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

        private static PlotModel RegressionTests()
        {
            var planetsData = ColumnMajorMatrix.Parse(File.ReadAllText(@"DataSets\swapi_planets_filtered.txt"));
            var model = new PlotModel { Title = "Star Wars Planets (diameter vs period)" };

            var diameters = planetsData.GetColumn(0) * 0.001;
            var periods = planetsData.GetColumn(1) * 0.1;
            var y = periods.Evaluate();

            model.Scatter(diameters, y);

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
                .Evaluate();

            var theta = BacktrackingSteepestDescentMethod.Find(
                _theta => ComputeCost(X, y, _theta),
                _theta => Gradient(X, y, _theta),
                Matrix.Ones(X.Width, 1),
                1,
                3000);

            var line = model.Function(diameters, y, x => theta[0] + theta[1] * x + theta[2] * x * x );

            line.Title = ComputeCost(X, y, theta).ToString("0.0000");
            line.Color = OxyPalettes.Hot(3).Colors[0];
        }

        private static void LinearRegression(Matrix diameters, Matrix y, PlotModel model)
        {
            var X = Matrix.Ones(diameters.Height, 1).Concat(diameters).Evaluate();
            var theta = BacktrackingSteepestDescentMethod.Find(
                _theta => ComputeCost(X, y, _theta),
                _theta => Gradient(X, y, _theta),
                Matrix.Ones(X.Width, 1),
                1,
                2000);

            var line = model.Function(diameters, y, x => theta[0] + theta[1] * x);

            line.Title = ComputeCost(X, y, theta).ToString("0.0000");
            line.Color = OxyPalettes.Hot(3).Colors[1];
        }

        private static double ComputeCost(Matrix X, Matrix y, Matrix theta)
        {
            var m = (double)y.Height;

            return (X * theta - y).Map(v => v * v).Sum() / (2 * m);
        }

        private static Matrix Gradient(Matrix X, Matrix y, Matrix theta)
        {
            var tC = (X * theta - y);
            var a = (Matrix.Ones(theta.Height, 1) * tC.Transpose()).Transpose().MultiplyEach(X);
            var b = a.Sum(0).Transpose();

            return b;
        }
    }
}
