using System.Linq;
using NUnit.Framework;
using Stratosphere.Math;

namespace Stratosphere.Tests.Math
{
    public static class MatrixAssert
    {
        public static void AreEqual(Matrix expected, Matrix actual)
        {
            Assert.True((expected - actual).EnumerateByColumns().All(v => System.Math.Abs(v) < 0.0001));
        }
    }
}