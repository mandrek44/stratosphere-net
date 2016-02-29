using NUnit.Framework;
using Stratosphere.Math;

namespace Stratosphere.Tests.Math
{
    public class Given_some_matrix
    {
        private Matrix _matrix;

        [SetUp]
        public void Setup()
        {
            _matrix = new Matrix();
        }

        [Test]
        public void Just_do_nothing()
        {
            Assert.NotNull(_matrix);
        } 
    }
}