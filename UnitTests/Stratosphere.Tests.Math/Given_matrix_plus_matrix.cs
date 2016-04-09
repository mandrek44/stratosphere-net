using NUnit.Framework;

namespace Stratosphere.Tests.Math
{
    public class Given_matrix_plus_matrix : Given_transformed_matrix
    {
        [SetUp]
        public void Given_test_matrix()
        {
            _matrix = TestMatrix.Data + "1 1 1;1 1 1";
            _expectedMatrix = "1 3 5;2 4 6";
        }
    }
}