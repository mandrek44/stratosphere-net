using System;

namespace Stratosphere.Math.Optimization
{
    public class QuasiNewtonMethod : IOptimizationMethod
    {
        public double Epsilon { get; } = 0.00001;
        public int MaxIterations { get; }
        public LineSearchAlgorithm LineSearchAlgorithm { get; set; } = new BacktrackingLineSearch();

        public Matrix InitialH { get; set; }

        public IIterationsTracker Tracker { get; } = new EmptyIterationsTracker();

        public QuasiNewtonMethod(int maxIterations = 1000, bool trackProgres = false)
        {
            MaxIterations = maxIterations;

            if (trackProgres)
                Tracker = new IterationsTracker();
        }

        public static Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix x0, int maxIterations)
        {
            return new QuasiNewtonMethod(maxIterations).Find(f, df, x0);
        }

        public Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix initial)
        {
            var H = InitialH ?? Matrix.Identity(initial.Height);

            Tracker.Track(initial);

            var x1 = initial;
            for (int i = 0; i < MaxIterations; ++i)
            {
                var dfx = df(x1);
                var p = -H * dfx;

                var x2 = LineSearchAlgorithm.Find(f, df, p, x1, dfx).Evaluate();

                if ((x2 - x1).Length < Epsilon || System.Math.Abs(dfx.Length) < 0.01)
                    return x1;

                Tracker.Track(x1);

                var dfx1 = df(x1);
                var q = (df(x2) - dfx1).Evaluate();
                var s = (x2 - x1).Evaluate();

                //H = (H + (q * q.T) / (q.T * s) - (H * s * s.T * H.T) / (s.T * H * s));

                H = H + (s * s.T) / (s.T * q) - (H * q)*((H*q).T / (q.T * H * q));

                H = H.Evaluate();
                x1 = x2.Evaluate();
            }

            return x1;
        }
    }
}