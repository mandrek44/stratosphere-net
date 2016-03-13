using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratosphere.Math
{
    public class TransposedMatrix : Matrix
    {
        private readonly Matrix _matrix;

        public TransposedMatrix(Matrix matrix)
        {
            _matrix = matrix;
            Size = matrix.Size.Reverse().ToArray();
        }

        public override int[] Size { get; }

        public override double Get(int index)
        {
            var row = index%Height;
            var col = index/Height;

            return GetByCoordinates(row, col);
        }

        public override double GetByCoordinates(int row, int column)
        {
            return _matrix.GetByCoordinates(column, row);
        }
    }
}