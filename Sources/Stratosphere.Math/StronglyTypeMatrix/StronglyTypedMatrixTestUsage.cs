namespace Stratosphere.Math.StronglyTypeMatrix
{
    public class StronglyTypedMatrixTestUsage
    {
        public void Test()
        {
            Matrix<M, M> H = Matrix.Identity(2).As<M, M>();
            Vector<M> x1 = new Vector<M>(1, 2);
            Vector<M> x2 = new Vector<M>(2, 3);
            Vector<M> dx1 = new Vector<M>(1, 3);
            Vector<M> dx2 = new Vector<M>(1, 3);

            Vector<M> q = dx2 - dx1;
            Vector<M> s = x2 - x1;

            H = H + (s * s.T) / (s.T * q) - (H * q) * ((H * q).T / (q.T * H * q));
        }
    }
}