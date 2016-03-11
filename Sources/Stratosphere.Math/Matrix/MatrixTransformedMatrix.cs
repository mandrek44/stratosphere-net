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

        public override double Get(int index)
        {
            return _transformation(_matrix.Get(index), _transformationValue.Get(index));
        }

        public override double GetByCoordinates(int row, int column)
        {
            return _transformation(_matrix.GetByCoordinates(row, column), _transformationValue.GetByCoordinates(row, column));
        }
    }
}