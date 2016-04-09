using NUnit.Framework;
using Stratosphere.Math;

namespace Stratosphere.Tests.Math
{
    public class Given_concated_matrix : Given_transformed_matrix
    {
        [SetUp]
        public void Given_test_matrix()
        {
            _matrix = Matrix.Vector(1, 4).Concat(Matrix.Vector(2, 5)).Concat(Matrix.Vector(3, 6));
            _expectedMatrix = "1 2 3;4 5 6";
        }
    }
}