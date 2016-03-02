using System.Linq;

namespace Stratosphere.Math
{
    public class Matrix
    {
    }

    public class ColumnMajorMatrix
    {
        private readonly double[] _data;
        private readonly int[] _dimensions;

        public ColumnMajorMatrix(double[] data, int[] dimensions)
        {
            _data = data;
            _dimensions = dimensions;
        }

        public ColumnMajorMatrix Multiply(double scalar)
        {
            return new ColumnMajorMatrix(
                _data.Select(value => value*scalar).ToArray(),
                _dimensions.ToArray());
        }

        public double GetByColumnIndex(int index) => _data[index];

        public double GetByRowIndex(int index)
        {
            var row = index/_dimensions[1];
            var column = index % _dimensions[1];

            return _data[row + column*_dimensions[0]];
        }
    }
}
