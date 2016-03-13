using System;
using System.Collections.Generic;

namespace Stratosphere.Math.Optimization
{
    public class SimpleSteepestDescentMethod
    {
        public double Epsilon { get; } = 0.00001;
        public int MaxIterations { get; }
        public bool TrackProgress { get; }

        private readonly List<Matrix> _history = new List<Matrix>();
        public IReadOnlyList<Matrix> History => _history;

        public SimpleSteepestDescentMethod(int maxIterations = 1000, bool trackProgres = false)
        {
            MaxIterations = maxIterations;
            TrackProgress = trackProgres;
        }

        public static Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix initial, double alpha, int maxIterations)
        {
            return new SimpleSteepestDescentMethod(maxIterations).Find(f, df, initial, alpha);
        }

        public Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix initial, double alpha)
        {
            var x = initial;
            var fx = f(x);
            for (int i = 0; i < MaxIterations; ++i)
            {
                var x2 = x - df(x) * alpha;
                if (TrackProgress)
                {
                    x2 = x2.Evaluate();
                    _history.Add(x2);
                }

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