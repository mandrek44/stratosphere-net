using System;

namespace Stratosphere.Math.Optimization
{
    public interface IOptimizationMethod
    {
        IIterationsTracker Tracker { get; }
        Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix initial);
    }
}