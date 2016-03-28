using System.Collections.Generic;

namespace Stratosphere.Math.Optimization
{
    public class IterationsTracker : IIterationsTracker
    {
        private readonly List<Matrix> _history = new List<Matrix>();
        public IReadOnlyList<Matrix> History => _history;

        public void Track(Matrix x)
        {
            _history.Add(x);
        }
    }
}