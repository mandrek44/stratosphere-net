using System;

namespace Stratosphere.Math.Optimization
{
    public class BacktrackingSteepestDescentMethod
    {
        private static double _epsilon = 0.00001;
        public IIterationsTracker Tracker { get; } = new EmptyIterationsTracker();
        public int MaxIterations { get; }

        public BacktrackingSteepestDescentMethod(int maxIterations = 1000, bool trackProgres = false)
        {
            MaxIterations = maxIterations;

            if (trackProgres)
                Tracker = new IterationsTracker();
        }

        public Math.Matrix Find(Func<Math.Matrix, double> f, Func<Math.Matrix, Math.Matrix> df, Math.Matrix initial)
        {
            Tracker.Track(initial);

            var x = initial;
            var fx = f(x);
            for (int i = 0; i < MaxIterations; ++i)
            {
                var dfx = df(x);
                if (dfx.Length < _epsilon)
                    return x;

                var p = -dfx / dfx.Length;

                var x2 = BacktrackingLineSearch.Find(f, p, x, dfx).Evaluate();

                Tracker.Track(x2);

                x = x2;
                // = f(x2);
            }

            return x;
        }

        public static Math.Matrix Find(Func<Math.Matrix, double> f, Func<Math.Matrix, Math.Matrix> df, Math.Matrix initial, double alpha, int maxIterations)
        {
            return new BacktrackingSteepestDescentMethod(maxIterations).Find(f, df, initial);
        }
    }
}