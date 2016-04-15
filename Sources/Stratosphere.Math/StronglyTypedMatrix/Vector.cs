namespace Stratosphere.Math.StronglyTypedMatrix
{
    public class Vector<D> : Matrix<D, One>
    {
        public Vector(Matrix inner) : base(inner)
        {
        }

        public Vector(params double[] values) : base(Matrix.Vector(values))
        {
        }

        public static Vector<D> operator -(Vector<D> a, Vector<D> b) => a.Inner.Substract(b.Inner).As<D>();
    }
}