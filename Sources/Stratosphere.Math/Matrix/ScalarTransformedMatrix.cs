using System;
using System.Collections.Generic;

namespace Stratosphere.Math.Matrix
{
    public class ScalarTransformedMatrix : Matrix
    {

        private readonly Matrix _matrix;
        private readonly Func<double, double> _transformation;

        public ScalarTransformedMatrix(Matrix matrix, Func<double, double> transformation ) : base(matrix)
        {
            _matrix = matrix;
            _transformation = transformation;
        }

        public override IEnumerable<int> IndexesByRows()
        {
            return _matrix.IndexesByRows();
        }

        public override IEnumerable<int> IndexesByColumns()
        {
            return _matrix.IndexesByColumns();
        }

        public override double GetByColumnIndex(int columnIndex)
        {
            return _transformation(_matrix.GetByColumnIndex(columnIndex));
        }

        public override double GetByRowIndex(int rowIndex)
        {
            return _transformation(_matrix.GetByRowIndex(rowIndex));
        }

        public override double GetByCoordinates(int row, int column)
        {
            return _transformation(_matrix.GetByCoordinates(row, column));
        }

        protected override bool Equals(Matrix other)
        {
            throw new NotImplementedException();
        }
    }
}