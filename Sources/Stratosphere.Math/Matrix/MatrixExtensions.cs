using Stratosphere.Math.StronglyTypedMatrix;

namespace Stratosphere.Math
{
    public static class MatrixExtensions
    {
        public static Matrix<D, One> Prepend<D>(this Matrix<D, One> z2, params double[] values)
        {
            return z2.Inner.Prepend(1).As<D>();
        }

        public static Matrix<D1,D2> Prepend<D1, D2>(this Matrix<D1, D2> matrix, double value)
        {
            return matrix.Inner.Prepend(1).As<D1, D2>();
        }

        public static Matrix Prepend(this Matrix matrix, double value)
        {
            return (value * Matrix.Ones(matrix.Width, 1)).Concat(matrix.T).T.Evaluate();
        }
    }
}