namespace Stratosphere.Math.StronglyTypeMatrix
{
    public class Matrix<D1, D2>
    {
        public Matrix Inner { get; }

        public Matrix(Matrix inner)
        {
            Inner = inner;
        }

        public static implicit operator Matrix(Matrix<D1, D2> a) => a.Inner;

        public static Matrix<D1, M> operator *(Matrix<D1, D2> a, Matrix<D2, M> b) => a.Inner.Multiply(b.Inner).As<D1, M>();

        public static Matrix<D1, One> operator *(Matrix<D1, D2> a, Matrix<D2, One> b) => a.Inner.Multiply(b.Inner).As<D1, One>();

        public static Matrix<D1, D2> operator -(Matrix<D1, D2> a, Matrix<D1, D2> b) => a.Inner.Substract(b.Inner).As<D1, D2>();

        public static Matrix<D1, D2> operator +(Matrix<D1, D2> a, Matrix<D1, D2> b) => a.Inner.Add(b.Inner).As<D1, D2>();

        public static Matrix<D1, D2> operator /(Matrix<D1, D2> a, Matrix<One, One> scalar) => a.Inner.Map(v => v / scalar.Inner).As<D1, D2>();

        public Matrix<D2, D1> T => Inner.Transpose().As<D2, D1>();
    }
}