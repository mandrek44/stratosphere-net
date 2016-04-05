using Stratosphere.Math.Optimization;

namespace Stratosphere.Math.Regression
{
    public class LogisticRegression
    {
        public static Matrix Calculate(Matrix X, Matrix y)
        {
            X = Matrix.Ones(X.Height, 1)
                .Concat(X)
                .Evaluate();

            return QuasiNewtonMethod.Find(
                f: theta => _Cost(X, y, theta),
                df: theta => Gradient(X, y, theta),
                x0: Matrix.Zeros(X.Width, 1),
                maxIterations: 1000);
        }

        public static double Sigmoid(double x) => 1.0 / (1.0 + System.Math.Exp(-x));

        public static Matrix Sigmoid(Matrix x) => x.Map(Sigmoid);

        public static double Cost(Matrix X, Matrix y, Matrix theta)
        {
            return _Cost(Matrix.Ones(X.Height, 1).Concat(X), y, theta);
        }

        private static double _Cost(Matrix X, Matrix y, Matrix theta)
        {
            var m = X.Height;
            var h = Sigmoid(theta.T * X.T).T;
            
            return (
                -y.MultiplyEach(h.Map(v => System.Math.Log(NonZero(v)))) 
                -(1 - y).MultiplyEach((1 - h).Map(v => System.Math.Log(NonZero(v))))).Sum() / m;
        }

        public static Matrix Gradient(Matrix X, Matrix y, Matrix theta)
        {
            var m = X.Height;
            var h = Sigmoid(theta.T * X.T).T;
            var a = Matrix.Ones(theta.Height, 1);
            var b = (h - y).T;
            var tC = (a * b).T;
            var grad = tC.MultiplyEach(X).Sum(0) / m;

            return grad.T;
        }

        private static double NonZero(double v)
        {
            return v == 0 ? 0.00000001 : v;
        }
    }
}