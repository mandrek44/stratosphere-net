using System;
using System.Collections.Generic;

namespace Stratosphere.Math.Optimization
{
    public interface IIterationsTracker
    {
        IReadOnlyList<Matrix> History { get; }
        void Track(Matrix x);
    }

    public class IterationsTracker : IIterationsTracker
    {
        private readonly List<Matrix> _history = new List<Matrix>();
        public IReadOnlyList<Matrix> History => _history;

        public void Track(Matrix x)
        {
            _history.Add(x);
        }
    }

    public class EmptyIterationsTracker : IIterationsTracker
    {
        public IReadOnlyList<Matrix> History { get; } = new List<Matrix>();
        public void Track(Matrix x)
        {
        }
    }

    public class SimpleSteepestDescentMethod
    {
        public double Epsilon { get; } = 0.000001;
        public int MaxIterations { get; }

        private readonly IIterationsTracker _tracker = new EmptyIterationsTracker();
        public IReadOnlyList<Matrix> History => _tracker.History;

        public SimpleSteepestDescentMethod(int maxIterations = 1000, bool trackProgres = false)
        {
            MaxIterations = maxIterations;

            if (trackProgres)
                _tracker = new IterationsTracker();
        }

        public static Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix x0, double alpha, int maxIterations)
        {
            return new SimpleSteepestDescentMethod(maxIterations).Find(f, df, x0, alpha);
        }

        public Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix initial, double alpha)
        {
            _tracker.Track(initial);

            var x = initial;
            var fx = f(x);
            for (int i = 0; i < MaxIterations; ++i)
            {
                var x2 = (x - df(x) * alpha).Evaluate();

                _tracker.Track(x2);

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