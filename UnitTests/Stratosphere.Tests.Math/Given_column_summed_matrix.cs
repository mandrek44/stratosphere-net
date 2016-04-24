using NUnit.Framework;

namespace Stratosphere.Tests.Math
{
    public class Given_column_summed_matrix : Given_transformed_matrix
    {
        [SetUp]
        public void When_transforming()
        {
            _matrix = TestMatrix.Data.Sum(0);
            _expectedMatrix = "1 4 9";
        }
    }
}