using System;

namespace Stratosphere.Math.Optimization
{
    public interface LineSearchAlgorithm
    {
        Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix p, Matrix x_start, Matrix dfx_start);
    }
}