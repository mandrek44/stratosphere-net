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
}