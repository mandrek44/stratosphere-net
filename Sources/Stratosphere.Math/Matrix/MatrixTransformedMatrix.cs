using System;
using System.Collections.Generic;

namespace Stratosphere.Math.Matrix
{
    public class MatrixTransformedMatrix : Matrix
    {
        private readonly Matrix _matrix;
        private readonly Matrix _transformationValue;
        private readonly Func<double, double, double> _transformation;

        public MatrixTransformedMatrix(Matrix matrix, Matrix transformationValue, Func<double, double, double> transformation) : base(matrix.Size)
        {
            _matrix = matrix;
            _transformationValue = transformationValue;
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
            return _transformation(_matrix.GetByColumnIndex(columnIndex), _transformationValue.GetByColumnIndex(columnIndex));
        }

        public override double GetByRowIndex(int rowIndex)
        {
            return _transformation(_matrix.GetByRowIndex(rowIndex), _transformationValue.GetByRowIndex(rowIndex));
        }

        public override double GetByCoordinates(int row, int column)
        {
            return _transformation(_matrix.GetByCoordinates(row, column), _transformationValue.GetByCoordinates(row, column));
        }

        protected override bool Equals(Matrix other)
        {
            throw new NotImplementedException();
        }
    }
}