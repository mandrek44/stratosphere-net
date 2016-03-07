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

        public override IEnumerable<int> IndexesByRows() => IndexesByColumns();
        
        public override IEnumerable<int> IndexesByColumns() => Enumerable.Range(0, Size.Product());

        public override double GetByColumnIndex(int columnIndex)
        {
            if (columnIndex < _a.Size.Product())
                return _a.GetByColumnIndex(columnIndex);
            else
                return _b.GetByColumnIndex(columnIndex - _a.Size.Product());
        }

        public override double GetByRowIndex(int rowIndex) => GetByColumnIndex(rowIndex);

        public override double GetByCoordinates(int row, int column)
        {
            if (column < _a.Width)
                return _a.GetByCoordinates(row, column);
            else
                return _b.GetByCoordinates(row, column - _a.Width);
        }

        protected override bool Equals(Matrix other)
        {
            throw new NotImplementedException();
        }
    }

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