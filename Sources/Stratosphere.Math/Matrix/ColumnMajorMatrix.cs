using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Stratosphere.Math
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

        public Matrix FilterRows(Func<Matrix, bool> func)
        {
            return new RowFilteredMatrix(this, func);
        }
    }

    public class RowFilteredMatrix : Matrix
    {
        private readonly Matrix _matrix;
        private readonly List<int> _matchingRows;

        public RowFilteredMatrix(Matrix matrix, Func<Matrix, bool> predicate)
        {
            _matrix = matrix;
            _matchingRows = new List<int>();

            for (int row = 0; row < matrix.Height; row++)
            {
                if (predicate(new SingleRowFromMatrix(matrix, row)))
                {
                    _matchingRows.Add(row);
                }
            }

            Size = new[] { _matchingRows.Count, _matrix.Width };
        }

        public override int[] Size { get; }

        public override double Get(int index)
        {
            var column = index / _matchingRows.Count;
            var row = index % _matchingRows.Count;

            return GetByCoordinates(row, column);
        }

        public override double GetByCoordinates(int row, int column)
        {
            return _matrix.GetByCoordinates(_matchingRows[row], column);
        }
    }

    public class SingleRowFromMatrix : Matrix
    {
        private readonly Matrix _matrix;
        private readonly int _row;

        public SingleRowFromMatrix(Matrix matrix, int row) : base(new[] { 1, matrix.Width })
        {
            _matrix = matrix;
            _row = row;
        }

        public override double Get(int index)
        {
            return _matrix.GetByCoordinates(_row, index);
        }

        public override double GetByCoordinates(int row, int column)
        {
            return _matrix.GetByCoordinates(_row + row, column);
        }
    }
}