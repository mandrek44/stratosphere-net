using System;
using System.Globalization;
using System.Linq;

namespace Stratosphere.Math.Matrix
{
    public class ColumnMajorMatrix : Matrix
    {
        private readonly double[] _data;

        protected internal ColumnMajorMatrix(double[] data, int[] dimensions) : base(dimensions)
        {
            _data = data;
        }

        /// <summary>
        /// Creates Matrix from string representation. Supports two formats:
        ///  1 2 3;4 5 6
        /// or
        ///  1,2,3\n4,5,6
        /// </summary>
        /// <param name="matrix">Comma or space delimited other string.</param>
        public static ColumnMajorMatrix Parse(string matrix)
        {
            var stringRows = matrix.Trim().Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);

            var rows = stringRows
                .Select(stringRow =>
                stringRow.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s))
                .Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray()).ToArray();

            var columns = rows.First().Length;

            var data = Enumerable.Range(0, columns).SelectMany(i => rows.Select(row => row[i])).ToArray();
            var dimensions = new[] { rows.Length, columns };

            return new ColumnMajorMatrix(data, dimensions);
        }

        public static implicit operator ColumnMajorMatrix(string matrix) => Parse(matrix);

        public override double GetByCoordinates(int row, int column)
        {
            return _data[Coordinate2ColumnIndex(row, column)];
        }

        public override double Get(int index) => _data[index];
        
        private int Coordinate2ColumnIndex(int row, int column)
        {
            return (column * Height) + row;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_data?.GetHashCode() ?? 0) * 397) ^ (Size?.GetHashCode() ?? 0);
            }
        }
    }
}