using System;

namespace Stratosphere.Math.Optimization
{
    public class BacktrackingSteepestDescentMethod
    {
        private static double _epsilon = 0.00001;

        public static Math.Matrix Find(Func<Math.Matrix, double> f, Func<Math.Matrix, Math.Matrix> df, Math.Matrix initial, double alpha, int maxIterations)
        {
            var x = initial;
            var fx = f(x);
            for (int i = 0; i < maxIterations; ++i)
            {
                var dfx = df(x);
                if (dfx.Length < _epsilon)
                    return x;

                var p = -dfx/dfx.Length;

                var x2 = BacktrackingLineSearch.Find(f, p, x, dfx).Evaluate();
                
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