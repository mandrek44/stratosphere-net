using System.Collections.Generic;

namespace Stratosphere.Math.Optimization
{
    public interface IIterationsTracker
    {
        IReadOnlyList<Matrix> History { get; }
        void Track(Matrix x);
    }
}