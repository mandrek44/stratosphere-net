using System;
using System.Collections.Generic;
using System.Linq;

namespace Stratosphere.Math.Matrix
{
    public class TransposedMatrix : Matrix
    {
        private readonly Matrix _matrix;

        public TransposedMatrix(Matrix matrix) : base(matrix)
        {
            _matrix = matrix;
            Size = matrix.Size.Reverse().ToArray();
        }

        public override int[] Size { get; }

        public override double GetByColumnIndex(int columnIndex)
        {   
            return _matrix.GetByRowIndex(columnIndex);
        }

        public override double GetByRowIndex(int rowIndex)
        {
            return _matrix.GetByColumnIndex(rowIndex);
        }

        public override double GetByCoordinates(int row, int column)
        {
            return _matrix.GetByCoordinates(column, row);
        }

        public override IEnumerable<int> IndexesByColumns()
        {
            return _matrix.IndexesByRows();
        }

        public override IEnumerable<int> IndexesByRows()
        {
            return _matrix.IndexesByColumns();
        }

        protected override bool Equals(Matrix other)
        {
            throw new NotImplementedException();
        }
    }
}