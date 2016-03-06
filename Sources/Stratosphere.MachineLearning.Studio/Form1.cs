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
            var model = new PlotModel { Title = "Star Wars Planets" };

            var dX = planetsData.GetColumn(0)*0.01;
            var y = planetsData.GetColumn(1);

            model.Scatter(dX, y);
            

            model.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Hot(3) });

            var plot1 = new PlotView();
            plot1.Dock = DockStyle.Fill;
            plot1.Location = new Point(0, 0);
            plot1.Margin = new Padding(0);
            plot1.Name = "plot1";
            plot1.Model = model;

            Controls.Add(plot1);

        }
    }
}
