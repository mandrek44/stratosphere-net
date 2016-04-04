using System;
using System.Collections.Generic;

namespace Stratosphere.Math
{
    public class RowFilteredMatrix : Matrix
    {
        private readonly Matrix _matrix;
        private readonly List<int> _matchingRows;

        public RowFilteredMatrix(Matrix matrix, Func<Matrix, bool> predicate)
        {
            _matrix = matrix;
            _matchingRows = new List<int>();

            for (int row = 0; row < matrix.Height; row++)
            {
                if (predicate(new SingleRowFromMatrix(matrix, row)))
                {
                    _matchingRows.Add(row);
                }
            }

            Size = new[] { _matchingRows.Count, _matrix.Width };
        }

        public override int[] Size { get; }

        public override double Get(int index)
        {
            var column = index / _matchingRows.Count;
            var row = index % _matchingRows.Count;

            return GetByCoordinates(row, column);
        }

        public override double GetByCoordinates(int row, int column)
        {
            return _matrix.GetByCoordinates(_matchingRows[row], column);
        }
    }
}