namespace Stratosphere.Math.StronglyTypedMatrix
{
    public class m { }
    public class n { }
    public class p { }
    public class One { }

    public class Example
    {
        public void Test()
        {
            Vector<n> x1 = new Vector<n>(1, 2);
            Vector<n> x2 = new Vector<n>(2, 3);
            Vector<n> dx1 = new Vector<n>(1, 3);
            Vector<n> dx2 = new Vector<n>(1, 3);

            Matrix<n, n> H = Matrix.Identity(2).As<n, n>();
            Matrix<n, One> q = dx2 - dx1;
            Matrix<n, One> s = x2 - x1;

            Matrix<n, n> dH = (s*s.T)/(s.T*q) - (H*q)*((H*q).T/(q.T*H*q));

            H = H + dH;
        }

        public void MultiplicationTest()
        {
            Matrix<One, n> A = null;
            Matrix<n, One> B = null;

            Matrix<One, One> AB = A*B;
        }

        public void DotProductTest()
        {
            Matrix<One, n> A = null;
            Matrix<n, One> B = null;

            Matrix<One, One> AB = A * B;
        }
    }
}