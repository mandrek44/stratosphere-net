using NUnit.Framework;

namespace Stratosphere.Tests.Math
{
    public class Given_transposed_matrix : Given_transformed_matrix
    {
        [SetUp]
        public void When_transforming()
        {
            _matrix = TestMatrix.Data.T;
            _expectedMatrix = "0 1;2 3;4 5";
        }
    }

    public class Given_row_removed_matrix : Given_transformed_matrix
    {
        [SetUp]
        public void When_transforming()
        {
            _matrix = TestMatrix.Data.RemoveFirstRow();
            _expectedMatrix = "1 3 5";
        }
    }
}