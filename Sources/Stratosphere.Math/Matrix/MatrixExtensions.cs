using Stratosphere.Math.StronglyTypedMatrix;

namespace Stratosphere.Math
{
    public static class MatrixExtensions
    {
        public static Matrix<D, One> Prepend<D>(this Matrix<D, One> z2, params double[] values)
        {
            return Matrix.Ones(1).Concat(z2.T.Inner).T.Evaluate().As<D, One>();
        }
    }
}