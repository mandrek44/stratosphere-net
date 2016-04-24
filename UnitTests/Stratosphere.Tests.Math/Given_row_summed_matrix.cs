using NUnit.Framework;

namespace Stratosphere.Tests.Math
{
    public class Given_row_summed_matrix : Given_transformed_matrix
    {
        [SetUp]
        public void When_transforming()
        {
            _matrix = TestMatrix.Data.Sum(1);
            _expectedMatrix = "6;9";
        }
    }
}