using NUnit.Framework;
using Stratosphere.Math;

namespace Stratosphere.Tests.Math
{
    public class Given_square_matrix
    {
        private ColumnMajorMatrix _matrix;

        [SetUp]
        public void Given_test_matrix()
        {
            _matrix = "1 3;" +
                      "4 2";
        }

        [Test]
        public void Then_inverse_can_be_calculated()
        {
            Assert.AreEqual(@"
-0,2 0,3;
0,4 -0,1;
", _matrix.Inverse().ToString());
        }
    }
}