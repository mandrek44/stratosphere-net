using System.Linq;
using NUnit.Framework;
using Stratosphere.Math;

namespace Stratosphere.Tests.Math
{
    public abstract class Given_transformed_matrix
    {
        protected Matrix _matrix;
        protected int[] _expectedSize;
        protected Matrix _expectedMatrix;

        public void Should_be_transformed_ok()
        {
            Assert.AreEqual(_expectedMatrix, _matrix);
        }

        [Test]
        public void Size_should_be_ok()
        {
            CollectionAssert.AreEqual(_expectedMatrix.Size, _matrix.Size);
        }

        [Test]
        public void Transposed_size_should_be_ok()
        {
            CollectionAssert.AreEqual(_expectedMatrix.Size.Reverse(), _matrix.T.Size);
        }

        [Test]
        public void Width_should_be_ok()
        {
            Assert.AreEqual(_expectedMatrix.Width, _matrix.Width);
        }

        [Test]
        public void Height_should_be_ok()
        {
            Assert.AreEqual(_expectedMatrix.Height, _matrix.Height);
        }
    }
}