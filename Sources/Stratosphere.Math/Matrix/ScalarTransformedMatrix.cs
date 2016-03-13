using System;
using System.Collections.Generic;

namespace Stratosphere.Math
{
    public class ScalarTransformedMatrix : Matrix
    {

        private readonly Matrix _matrix;
        private readonly Func<double, double> _transformation;

        public ScalarTransformedMatrix(Matrix matrix, Func<double, double> transformation ) : base(matrix.Size)
        {
            _matrix = matrix;
            _transformation = transformation;
        }
        
        public override double Get(int index)
        {
            return _transformation(_matrix.Get(index));
        }

        public override double GetByCoordinates(int row, int column)
        {
            return _transformation(_matrix.GetByCoordinates(row, column));
        }
    }
}