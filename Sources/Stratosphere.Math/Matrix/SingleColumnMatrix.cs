using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratosphere.Math
{
    public class SingleColumnMatrix : Matrix
    {
        private readonly Matrix _matrix;
        private readonly int _column;

        public SingleColumnMatrix(Matrix matrix, int column) : base(new int[] { matrix.Height, 1 })
        {
            _matrix = matrix;
            _column = column;
        }

        public override double Get(int index)
        {
            return _matrix.Get(_column * Height + index);
        }

        public override double GetByCoordinates(int row, int column)
        {
            return _matrix.GetByCoordinates(row, _column);
        }
    }
}