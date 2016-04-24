using NUnit.Framework;
using Stratosphere.Math;

namespace Stratosphere.Tests.Math
{
    public class Given_prepended_matrix : Given_transformed_matrix
    {
        [SetUp]
        public void When_transforming()
        {
            _matrix = TestMatrix.Data.Prepend(1);
            _expectedMatrix = "1 1 1;0 2 4;1 3 5";
        }
    }
}