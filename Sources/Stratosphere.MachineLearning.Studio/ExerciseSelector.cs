using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stratosphere.MachineLearning.Studio
{
    public partial class ExerciseSelector : Form
    {
        public ExerciseSelector()
        {
            InitializeComponent();
        }

        private void buttonXSquared_Click(object sender, EventArgs e)
        {
            var form = new FormDataSet();
            form.Display(FormDataSet.XSquared());

            form.ShowDialog();
        }

        private void buttonRegression_Click(object sender, EventArgs e)
        {
            var form = new FormDataSet();
            form.Display(FormDataSet.RegressionTests());

            form.ShowDialog();
        }

        private void buttonBanana_Click(object sender, EventArgs e)
        {
            var form = new FormDataSet();
            form.Display(FormDataSet.PlotBananaFunction());

            form.ShowDialog();
        }
    }
}
