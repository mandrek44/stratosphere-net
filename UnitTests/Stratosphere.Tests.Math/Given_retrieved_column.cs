using NUnit.Framework;

namespace Stratosphere.Tests.Math
{
    public class Given_retrieved_column : Given_transformed_matrix
    {
        [SetUp]
        public void Given_test_matrix()
        {
            _matrix = TestMatrix.Data.GetColumn(1);
            _expectedMatrix = "2;3";
        }
    }
}