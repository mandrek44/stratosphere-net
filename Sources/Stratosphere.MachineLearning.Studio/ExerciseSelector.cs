﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stratosphere.Math.Optimization;

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
            form.Display(FormDataSet.PlotBananaFunction(new QuasiNewtonMethod(trackProgres: true, maxIterations: 1500)));

            form.ShowDialog();
        }

        private void buttonNewton_Click(object sender, EventArgs e)
        {
            var form = new FormDataSet();
            form.Display(FormDataSet.PlotBananaFunction(new SimpleNewtonMethod
            {
                ddf = FormDataSet.BananaFunctionSecondDerivatives,
                Tracker = new IterationsTracker(),
                Alpha = 0.5
                
            }));

            form.ShowDialog();
        }

        private void buttonBacktrackGradient_Click(object sender, EventArgs e)
        {
            var form = new FormDataSet();
            form.Display(FormDataSet.PlotBananaFunction(new BacktrackingSteepestDescentMethod(trackProgres: true, maxIterations: 2000)));

            form.ShowDialog();
        }

        private void buttonNewtonBactrack_Click(object sender, EventArgs e)
        {
            var form = new FormDataSet();
            form.Display(FormDataSet.PlotBananaFunction(new NewtonMethodWithBacktracking
            {
                ddf = FormDataSet.BananaFunctionSecondDerivatives,
                Tracker = new IterationsTracker(),
            }));

            form.ShowDialog();
        }

        private void buttonLogistic_Click(object sender, EventArgs e)
        {
            var form = new FormDataSet();
            form.Display(FormDataSet.LogisticRegression());

            form.ShowDialog();
        }
    }
}
