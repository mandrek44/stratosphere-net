using NUnit.Framework;
using Stratosphere.Math;

namespace Stratosphere.Tests.Math
{
    public static class MatrixAssert
    {
        public static void AreEqual(Matrix expected, Matrix actual)
        {
            Assert.AreEqual(expected, actual);
        }
    }
}