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
using Stratosphere.Math.Formulas;
using Stratosphere.Math.Optimization;
using Stratosphere.Math.Regression;
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

        public static PlotModel PlotBananaFunction(IOptimizationMethod method)
        {
            var model = new PlotModel();

            model.HeatMap(-2.0, 2.0, -1, 3, x => Log(Rosenbrock.Function(x)));
            PlotOptimizationSteps(model, method);

            return model;
        }

        public static PlotModel XSquared()
        {
            var model = new PlotModel { Title = "f(x) = x^2", LegendFontSize = 20.5, LegendPosition = LegendPosition.TopCenter };

            model.Function(v => v * v, -2, 2);

            return model;
        }

        public static PlotModel Sigmoid()
        {
            var model = new PlotModel { Title = "S(t)", LegendFontSize = 20.5, LegendPosition = LegendPosition.TopCenter };
            var yAxis = new LinearAxis {PositionAtZeroCrossing = true};
            model.Axes.Add(yAxis);

            model.Function(LogisticRegression.Sigmoid, -5, 5);

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

            var theta = Math.Regression.LinearRegression.Calculate(diameters, y);

            var line = model.Polynomial(diameters, theta);

            line.Color = OxyPalettes.Hot(3).Colors[0];

            PolynomialRegression(diameters, y, model);
            LinearRegression(diameters, y, model);

            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Hot(3) });
            return model;
        }

        private static void PlotOptimizationSteps(PlotModel model, IOptimizationMethod findMethod)
        {
            findMethod.Find(Rosenbrock.Function, Rosenbrock.Derivative, "-2;0");

            var historyLine = new LineSeries() { MarkerType = MarkerType.Cross };
            var historyPoints = new ScatterSeries() { MarkerType = MarkerType.Cross, MarkerStrokeThickness = 5 };
            double lastF = double.MaxValue;
            var lastHistoryEntry = findMethod.Tracker.History.Last();
            foreach (var historyX in findMethod.Tracker.History)
            {
                var f = Rosenbrock.Function(historyX);
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
            var X = diameters
                .Concat(diameters.Map(x => x * x))
                .Concat(diameters.Map(x => x * x * x))
                .Concat(diameters.Map(x => x * x * x * x))
                .Evaluate();

            var theta = Math.Regression.LinearRegression.Calculate(X, y);

            var line = model.Polynomial(diameters, theta);
            line.Title = Math.Regression.LinearRegression.ComputeCost(X, y, theta).ToString("0.0000");
            line.Color = OxyPalettes.Hot(3).Colors[1];
        }

        private static void LinearRegression(Matrix diameters, Matrix y, PlotModel model)
        {
            var theta = Math.Regression.LinearRegression.Calculate(diameters, y);

            var line = model.Function(x => theta[0] + theta[1] * x, diameters);

            line.Title = Math.Regression.LinearRegression.ComputeCost(diameters, y, theta).ToString("0.0000");
            line.Color = OxyPalettes.Hot(3).Colors[0];
        }

        public static PlotModel PlotLogisticRegression()
        {

            var model = new PlotModel { Title = "Star Wars Planets (diameter vs period)" };

            var planetsData = ColumnMajorMatrix.Parse(File.ReadAllText(@"DataSets\swapi_planets_filtered.txt"));
            var data = (planetsData.GetColumn(0) * 0.001).Concat(planetsData.GetColumn(1) * 0.1).Concat(planetsData.GetColumn(5)).Evaluate();

            //var data = ColumnMajorMatrix.Parse(File.ReadAllText(@"DataSets\mlEx2.txt"));

            var set0 = data.FilterRows(row => Abs(row[0, 2]) < 0.00001).Evaluate();
            var set1 = data.FilterRows(row => Abs(row[0, 2] - 1) < 0.00001).Evaluate();

            model.Scatter(set0.GetColumn(0), set0.GetColumn(1), MarkerType.Diamond);
            model.Scatter(set1.GetColumn(0), set1.GetColumn(1), MarkerType.Circle);

            var y = data.GetColumn(2).Evaluate();
            var X = data.GetColumn(0).Concat(data.GetColumn(1));

            var theta = LogisticRegression.Calculate(X, y);

            var line = model.Function(x => -(theta[0] + theta[1] * x ) / theta[2],
                data.GetColumn(0), data.GetColumn(1));

            line.Title = LogisticRegression.Cost(X, y, theta).ToString("0.0000");
            line.Color = OxyPalettes.Hot(3).Colors[1];

            return model;
        }
    }
}
