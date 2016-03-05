using NUnit.Framework;
using Stratosphere.Math;
using Stratosphere.Math.Matrix;
using Stratosphere.Math.Optimization;
using static System.Math;

namespace Stratosphere.Tests.Math
{
    public class Given_simple_optimization_problem
    {
        [Test]
        public void Find_minimum_with_simple_steepest_descent_method()
        {
            var x_min = SimpleSteepestDescentMethod.Find(x => Pow(x[0], 2), x => 2*x, ColumnMajorMatrix.Parse("-1"), 0.5, 100);

            Assert.AreEqual(0d, x_min[0], 0.001);
        }

        [Test]
        public void Find_minimum_with_backgracking_method()
        {
            var x_min = BacktrackingSteepestDescentMethod.Find(x => Pow(x[0], 2), x => 2 * x, ColumnMajorMatrix.Parse("-1"), 0.5, 100);

            Assert.AreEqual(0d, x_min[0], 0.001);
        }
    }
}