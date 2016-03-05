using System;

namespace Stratosphere.Math.Optimization
{
    public class SimpleSteepestDescentMethod
    {
        private static double _epsilon = 0.00001;

        public static Matrix.Matrix Find(Func<Matrix.Matrix, double> f, Func<Matrix.Matrix, Matrix.Matrix> df, Matrix.Matrix initial, double alpha, int maxIterations)
        {
            var x = initial;
            var fx = f(x);
            for (int i = 0; i < maxIterations; ++i)
            {
                var x2 = x - df(x)*alpha;

                var fx2 = f(x2);
                if (fx - fx2 < _epsilon)
                    return x2;

                x = x2;
                fx = fx2;
            }

            return x;
        }
    }
}