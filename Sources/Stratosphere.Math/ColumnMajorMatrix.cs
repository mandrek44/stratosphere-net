using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Stratosphere.Math
{
    public class ColumnMajorMatrix
    {
        private const double Tolerance = 0.0001;
        private readonly double[] _data;
        private readonly int[] _dimensions;

        public ColumnMajorMatrix(double[] data, int[] dimensions)
        {
            _data = data;
            _dimensions = dimensions;
        }

        /// <summary>
        /// Creates Matrix from string representation. Supports two formats:
        ///  1 2 3;4 5 6
        /// or
        ///  1,2,3\n4,5,6
        /// </summary>
        /// <param name="matrix">Comma or space delimited matrix string.</param>
        public ColumnMajorMatrix(string matrix)
        {
            var stringRows = matrix.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);

            var rows = stringRows.Select(stringRow =>
                stringRow.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray()).ToArray();

            var columns = rows.First().Length;

            _data = Enumerable.Range(0, columns).SelectMany(i => rows.Select(row => row[i])).ToArray();
            _dimensions = new int[] { rows.Length, columns };
        }

        public static implicit operator ColumnMajorMatrix(string matrix) => new ColumnMajorMatrix(matrix);
        public static ColumnMajorMatrix operator *(ColumnMajorMatrix a, double scalar) => a.Multiply(scalar);
        public static ColumnMajorMatrix operator *(ColumnMajorMatrix a, int scalar) => a.Multiply(scalar);
        public static ColumnMajorMatrix operator *(int scalar, ColumnMajorMatrix a) => a.Multiply(scalar);
        public static ColumnMajorMatrix operator *(double scalar, ColumnMajorMatrix a) => a.Multiply(scalar);

        public ColumnMajorMatrix Multiply(double scalar)
        {
            return new ColumnMajorMatrix(
                _data.Select(value => value * scalar).ToArray(),
                _dimensions.ToArray());
        }

        public double GetByColumnIndex(int index) => _data[index];

        public double GetByRowIndex(int index)
        {
            if (_dimensions.Length != 2)
                throw new MultiDimensionalMatrixNotSupportedException();

            var row = index / _dimensions[1];
            var column = index % _dimensions[1];

            return _data[row + column * _dimensions[0]];
        }

        protected bool Equals(ColumnMajorMatrix other)
        {
            if (_data.Length != other._data.Length || _dimensions.Length != other._dimensions.Length)
                return false;

            for (int i = 0; i < _dimensions.Length; i++)
            {
                if (_dimensions[i] != other._dimensions[i])
                    return false;
            }

            for (int i = 0; i < _data.Length; i++)
            {
                if (System.Math.Abs(_data[i] - other._data[i]) > Tolerance)
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ColumnMajorMatrix)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_data?.GetHashCode() ?? 0) * 397) ^ (_dimensions?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            var rowsStringBuilders = new StringBuilder[_dimensions[0]];

            for (int i = 0; i < rowsStringBuilders.Length; ++i)
                rowsStringBuilders[i] = new StringBuilder();

            rowsStringBuilders[0].AppendLine();

            for (int i = 0; i < _data.Length; ++i)
            {
                var stringBuilder = rowsStringBuilders[i % _dimensions[0]];
                stringBuilder.AppendFormat("{0:0.##}", _data[i]);
                stringBuilder.Append(' ');
            }

            for (int i = 0; i < rowsStringBuilders.Length; ++i)
                rowsStringBuilders[i].AppendLine();

            return string.Concat(rowsStringBuilders.Select(sb => sb.ToString()));
        }
    }
}