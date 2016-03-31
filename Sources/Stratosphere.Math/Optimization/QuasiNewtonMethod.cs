using System;

namespace Stratosphere.Math.Optimization
{
    public class SimpleNewtonMethod : IOptimizationMethod
    {
        public IIterationsTracker Tracker { get; set; } = new EmptyIterationsTracker();

        public double Epsilon { get; set; } = 0.00001;

        public int MaxIterations { get; set; } = 1000;

        public double Alpha { get; set; } = 1;

        public Func<Matrix, Matrix> ddf { get; set; }

        public Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Func<Matrix, Matrix> ddf, Matrix initial)
        {
            this.ddf = ddf;
            return Find(f, df, initial);
        }

        public Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix initial)
        {
            Tracker.Track(initial);

            var x = initial;
            for (int i = 0; i < MaxIterations; ++i)
            {
                var dfx = df(x);
                var x2 = x - Alpha * ddf(x).Inverse() * dfx;

                Tracker.Track(x2);

                if ((x2 - x).Length < Epsilon)
                    return x2;

                x = x2;
            }

            return x;
        }
    }

    public class NewtonMethodWithBacktracking : IOptimizationMethod
    {
        public IIterationsTracker Tracker { get; set; } = new EmptyIterationsTracker();

        public double Epsilon { get; set; } = 0.00001;

        public int MaxIterations { get; set; } = 1000;

        public Func<Matrix, Matrix> ddf { get; set; }

        public Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Func<Matrix, Matrix> ddf, Matrix initial)
        {
            this.ddf = ddf;
            return Find(f, df, initial);
        }

        public Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df , Matrix initial)
        {
            Tracker.Track(initial);

            var x = initial;
            for (int i = 0; i < MaxIterations; ++i)
            {
                var dfx = df(x);
                var p = -ddf(x).Inverse()*dfx;

                var x2 = BacktrackingLineSearch.Find(f, df, p, x, dfx).Evaluate();

                Tracker.Track(x2);

                if ((x2 - x).Length < Epsilon)
                    return x2;

                x = x2;
            }

            return x;
        }
    }

    public class QuasiNewtonMethod : IOptimizationMethod
    {
        public double Epsilon { get; } = 0.000001;
        public int MaxIterations { get; }

        public IIterationsTracker Tracker { get; } = new EmptyIterationsTracker();

        public QuasiNewtonMethod(int maxIterations = 1000, bool trackProgres = false)
        {
            MaxIterations = maxIterations;

            if (trackProgres)
                Tracker = new IterationsTracker();
        }

        public static Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix x0, double alpha, int maxIterations)
        {
            return new QuasiNewtonMethod(maxIterations).Find(f, df, x0);
        }

        public Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix initial)
        {
            var H = Matrix.Identity(initial.Height);

            Tracker.Track(initial);

            var x = initial;
            var fx = f(x);
            for (int i = 0; i < MaxIterations; ++i)
            {
                var dfx = df(x);
                if (dfx.Length < Epsilon)
                    return x;

                var p = -H*dfx;

                var x2 = BacktrackingLineSearch.Find(f, df, p, x, dfx).Evaluate();

                Tracker.Track(x);
                
                var q = df(x2) - dfx;
                var s = x2 - x;

                var H2 = H + (s*s.T)/(q.T*s) - (H*q*q.T*H.T)/(q.T*H*q);

                H = H2;
                x = x2;
            }

            return x;
        }
    }
}