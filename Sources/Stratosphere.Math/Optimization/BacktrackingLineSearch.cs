using System;

namespace Stratosphere.Math.Optimization
{
    public class BacktrackingLineSearch
    {
        private const double K = 0.125;
        private const double C = 0.01;

        /// <summary>
        /// Finds inexact local minimum at direction p.
        /// </summary>
        /// <param name="f">Function to minimize.</param>
        /// <param name="p">Direction vector along which to minimize the function f.</param>
        /// <param name="x_start">Starting point.</param>
        /// <param name="dfx_start">Gradient value at point x_start.</param>
        /// <returns>Value of x for which f(x) is close to local minimum.</returns>
        public static Matrix.Matrix Find(Func<Matrix.Matrix, double> f, Matrix.Matrix p, Matrix.Matrix x_start, Matrix.Matrix dfx_start)
        {
            var x0 = x_start;
            var fx0 = f(x0);

            double alpha = 1;
            int i = 0;

            while (!Armijo(f, x0, fx0, dfx_start, p, alpha) && i < 32)
            {
                alpha = K * alpha;
                i++;
            }

            return x0 + alpha * p;
        }

        private static bool Armijo(Func<Matrix.Matrix, double> f, Matrix.Matrix x0, double fx0, Matrix.Matrix dfx0, Matrix.Matrix p, double alpha)
        {
            return f(x0 + alpha * p) <= fx0 + C * alpha * (dfx0.T * p);
        }
    }
}