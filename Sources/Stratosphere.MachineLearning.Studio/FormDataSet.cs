using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.WindowsForms;
using Stratosphere.Math.Matrix;

namespace Stratosphere.MachineLearning.Studio
{
    public partial class FormDataSet : Form
    {
        public FormDataSet()
        {
            InitializeComponent();
            Size = new Size(800, 600);

            var planetsData = ColumnMajorMatrix.Parse(File.ReadAllText(@"DataSets\swapi_planets_filtered.txt"));
            var model = new PlotModel { Title = "Star Wars Planets (diameter vs period)" };

            var diameters = planetsData.GetColumn(0)*0.001;
            var periods = planetsData.GetColumn(1)*0.1;
            
            var X = Matrix.Ones(diameters.Height, 1).Concat(diameters); // Add a column of ones to x
            var y = periods;

            var theta = Math.Optimization.BacktrackingSteepestDescentMethod.Find(
                _theta => ComputeCost(X, y, _theta),
                _theta => Gradient(X, y, _theta), 
                Matrix.Zeros(X.Width, 1), 
                1, 
                1000);

            model.Scatter(diameters, y);
            model.Function(diameters, y, x => theta[0] + theta[1]*x);
            model.Title += " " + ComputeCost(X, y, theta).ToString("##.00") + " " + costCallCount;

            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Hot(3) });

            var plotView = new PlotView();
            plotView.Dock = DockStyle.Fill;
            plotView.Location = new Point(0, 0);
            plotView.Margin = new Padding(0);
            plotView.Name = "plotView";
            plotView.Model = model;

            Controls.Add(plotView);
        }

        static int costCallCount =0;

        private static double ComputeCost(Matrix X, Matrix y, Matrix theta)
        {
            costCallCount++;
            var m = (double)y.Height;

            return (X*theta - y).Map(v => v * v).Sum() / (2 * m);
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
