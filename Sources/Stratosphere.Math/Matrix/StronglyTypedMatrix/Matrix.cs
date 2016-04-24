namespace Stratosphere.Math.StronglyTypedMatrix
{
    public class Matrix<D1, D2>
    {
        public Matrix Inner { get; }

        public Matrix(Matrix inner)
        {
            Inner = inner;
        }

        public static implicit operator Matrix(Matrix<D1, D2> a) => a.Inner;

        public static Matrix<D1, m> operator *(Matrix<D1, D2> a, Matrix<D2, m> b) => a.Inner.Multiply(b.Inner).As<D1, m>();

        public static Matrix<D1, n> operator *(Matrix<D1, D2> a, Matrix<D2, n> b) => a.Inner.Multiply(b.Inner).As<D1, n>();

        public static Matrix<D1, p> operator *(Matrix<D1, D2> a, Matrix<D2, p> b) => a.Inner.Multiply(b.Inner).As<D1, p>();

        public static Matrix<D1, k> operator *(Matrix<D1, D2> a, Matrix<D2, k> b) => a.Inner.Multiply(b.Inner).As<D1, k>();

        public static Matrix<D1, One> operator *(Matrix<D1, D2> a, Matrix<D2, One> b) => a.Inner.Multiply(b.Inner).As<D1, One>();

        public static Matrix<D1, D2> operator -(Matrix<D1, D2> a, Matrix<D1, D2> b) => a.Inner.Substract(b.Inner).As<D1, D2>();

        public static Matrix<D1, D2> operator -(double a, Matrix<D1, D2> b) => (a - b.Inner).As<D1, D2>();

        public static Matrix<D1, D2> operator +(Matrix<D1, D2> a, Matrix<D1, D2> b) => a.Inner.Add(b.Inner).As<D1, D2>();

        public static Matrix<D1, D2> operator /(Matrix<D1, D2> a, Matrix<One, One> scalar) => a.Inner.Map(v => v / scalar.Inner).As<D1, D2>();

        public static Matrix<D1, D2> operator *(Matrix<D1, D2> a, Matrix<One, One> scalar) => a.Inner.Map(v => v * scalar.Inner).As<D1, D2>();

        public static Matrix<D1, D2> operator *(Matrix<D1, D2> a, double scalar) => a.Inner.Map(v => v * scalar).As<D1, D2>();

        public Matrix<D2, D1> T => Inner.Transpose().As<D2, D1>();

        public Matrix<D1, D2> MultiplyEach(Matrix<D1, D2> matrix) => Inner.MultiplyEach(matrix).As<D1, D2>();

        public Matrix<One, D2> SumColumns() => Inner.Sum(0).As<One, D2>();

        public Matrix<D1, One> SumRows() => Inner.Sum(1).As<D1, One>();

        public double Sum() => Inner.Sum();

        public int Height => Inner.Height;
        public int Width => Inner.Width;
        public int[] Size => Inner.Size;

        public Matrix<D1, One> GetColumn(int i) => Inner.GetColumn(i).As<D1, One>();
    }
}