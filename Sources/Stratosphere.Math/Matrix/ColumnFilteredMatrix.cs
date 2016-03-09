using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratosphere.Math.Matrix
{
    public class ColumnFilteredMatrix : Matrix
    {
        private readonly Matrix _matrix;
        private readonly int _column;

        public ColumnFilteredMatrix(Matrix matrix, int column) : base(new int[] { matrix.Height, 1 })
        {
            _matrix = matrix;
            _column = column;
        }

        public override IEnumerable<int> IndexesByRows()
        {
            return Enumerable.Range(0, Height);
        }

        public override IEnumerable<int> IndexesByColumns()
        {
            return Enumerable.Range(0, Height);
        }

        public override double GetByColumnIndex(int columnIndex)
        {
            return _matrix.GetByColumnIndex(_column * Height + columnIndex);
        }

        public override double GetByRowIndex(int rowIndex)
        {
            return _matrix.GetByColumnIndex(_column * Height + rowIndex);
        }

        public override double GetByCoordinates(int row, int column)
        {
            return _matrix.GetByCoordinates(row, _column);
        }
        
        protected override bool Equals(Matrix other)
        {
            throw new NotImplementedException();
        }
    }
}