using Stratosphere.Math.Optimization;

namespace Stratosphere.Math.Regression
{
    public class LinearRegression
    {
        public Matrix Initial { get; set; }

        public int MaxIterations { get; set; } = 1000;

        public Matrix X { get; set; }

        public Matrix y { get; set; }

        public LinearRegression(Matrix X, Matrix y)
        {
            this.X = X;
            this.y = y;
        }

        public static Matrix Calculate(Matrix X, Matrix y)
        {
            return new LinearRegression(X, y).Calculate();
        }

        public Matrix Calculate()
        {
            X = Matrix.Ones(X.Height, 1).Concat(X).Evaluate();
            return QuasiNewtonMethod.Find(
                f: theta => ComputeCost_(X, y, theta),
                df: theta => Gradient(X, y, theta),
                x0: Initial ?? Matrix.Ones(X.Width, 1),
                maxIterations: MaxIterations);
        }

        public static double ComputeCost(Matrix X, Matrix y, Matrix theta)
        {
            return ComputeCost_(Matrix.Ones(X.Height, 1).Concat(X), y, theta);
        }

        private static double ComputeCost_(Matrix X, Matrix y, Matrix theta)
        {
            var m = y.Height;
            var error = X * theta - y;
            return error.Map(v => v * v).Sum() / (2.0 * m);
        }

        private static Matrix Gradient(Matrix X, Matrix y, Matrix theta)
        {
            var error = (X * theta - y);
            var a = (Matrix.Ones(theta.Height, 1) * error.Transpose()).Transpose().MultiplyEach(X);
            return a.Sum(0).Transpose();
        }
    }
}