using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratosphere.Math.Matrix
{
    public class ColumnsConcatMatrix : Matrix
    {
        private readonly Matrix _a;
        private readonly Matrix _b;

        public ColumnsConcatMatrix(Matrix a, Matrix b)
        {
            if (a.Height != b.Height)
                throw new InvalidOperationException("Matrices must be of equal height");

            _a = a;
            _b = b;

            Size = new int[] {_a.Height, _a.Width + _b.Width};
        }

        public override int[] Size { get; }

        public override double Get(int index)
        {
            if (index < _a.Size.Product())
                return _a.Get(index);
            else
                return _b.Get(index - _a.Size.Product());
        }

        public override double GetByCoordinates(int row, int column)
        {
            if (column < _a.Width)
                return _a.GetByCoordinates(row, column);
            else
                return _b.GetByCoordinates(row, column - _a.Width);
        }
    }
}