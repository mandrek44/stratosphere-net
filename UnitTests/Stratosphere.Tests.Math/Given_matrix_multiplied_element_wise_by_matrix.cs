using NUnit.Framework;

namespace Stratosphere.Tests.Math
{
    public class Given_matrix_multiplied_element_wise_by_matrix : Given_transformed_matrix
    {
        [SetUp]
        public void Given_test_matrix()
        {
            _matrix = TestMatrix.Data.MultiplyEach("2 2 2;2 2 2");
            _expectedMatrix = "0 2 8;2 6 10";
        }
    }
}