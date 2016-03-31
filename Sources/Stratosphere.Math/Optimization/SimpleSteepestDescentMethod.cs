using System;
using System.Collections.Generic;

namespace Stratosphere.Math.Optimization
{
    public class SimpleSteepestDescentMethod : IOptimizationMethod
    {
        public double Epsilon { get; } = 0.000001;
        public int MaxIterations { get; }
        public double Alpha { get; set; }

        public IIterationsTracker Tracker { get; } = new EmptyIterationsTracker();

        public SimpleSteepestDescentMethod(int maxIterations = 1000, bool trackProgres = false, double alpha = 0.001)
        {
            MaxIterations = maxIterations;
            Alpha = alpha;

            if (trackProgres)
                Tracker = new IterationsTracker();
        }

        public static Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix x0, double alpha, int maxIterations)
        {
            return new SimpleSteepestDescentMethod(maxIterations: maxIterations, alpha: alpha).Find(f, df, x0);
        }

        public Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix initial)
        {
            Tracker.Track(initial);

            var x = initial;
            var fx = f(x);
            for (int i = 0; i < MaxIterations; ++i)
            {
                var x2 = (x - df(x) * Alpha).Evaluate();

                Tracker.Track(x2);

                var fx2 = f(x2);
                if (fx - fx2 < Epsilon)
                    return x2;

                x = x2;
                fx = fx2;
            }

            return x;
        }
    }
}