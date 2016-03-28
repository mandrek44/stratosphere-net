using System.Collections.Generic;

namespace Stratosphere.Math.Optimization
{
    public class EmptyIterationsTracker : IIterationsTracker
    {
        public IReadOnlyList<Matrix> History { get; } = new List<Matrix>();
        public void Track(Matrix x)
        {
        }
    }
}