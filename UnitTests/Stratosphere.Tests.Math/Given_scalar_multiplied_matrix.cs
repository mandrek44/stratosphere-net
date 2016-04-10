using NUnit.Framework;

namespace Stratosphere.Tests.Math
{
    public class Given_scalar_multiplied_matrix : Given_transformed_matrix
    {
        [SetUp]
        public void Given_test_matrix()
        {
            _matrix = TestMatrix.Data*2;
            _expectedMatrix = "0 4 8;2 6 10";
        }
    }
}