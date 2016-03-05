using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Stratosphere.Math.Matrix
{
    public class ColumnMajorMatrix : Matrix
    {
        protected internal ColumnMajorMatrix(double[] data, int[] dimensions) : base(data, dimensions)
        {
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
            var stringRows = matrix.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);

            var rows = stringRows.Select(stringRow =>
                stringRow.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray()).ToArray();

            var columns = rows.First().Length;

            var data = Enumerable.Range(0, columns).SelectMany(i => rows.Select(row => row[i])).ToArray();
            var dimensions = new[] { rows.Length, columns };

            return new ColumnMajorMatrix(data, dimensions);
        }

        public static implicit operator ColumnMajorMatrix(string matrix) => Parse(matrix);

        public override double GetByCoordinates(int row, int column)
        {
            return Data[Coordinate2ColumnIndex(row, column)];
        }

        public override double GetByColumnIndex(int columnIndex) => Data[columnIndex];

        public override IEnumerable<int> IndexesByColumns()
        {
            for (int i = 0; i < Data.Length; ++i)
                yield return i;
        }

        public override double GetByRowIndex(int rowIndex)
        {
            if (Size.Length != 2)
                throw new MultiDimensionalMatrixNotSupportedException();

            var row = rowIndex / Size[1];
            var column = rowIndex % Size[1];

            return Data[row + column * Size[0]];
        }

        public override IEnumerable<int> IndexesByRows()
        {
            int h = Size[0];
            int w = Size[1];

            for (int j = 0; j < h; ++j)
                for (int i = 0; i < w * h; i += h)
                    yield return i + j;
        }

        protected override bool Equals(Matrix other)
        {
            if (Size.Length != other.Size.Length)
                return false;

            for (int i = 0; i < Size.Length; i++)
            {
                if (Size[i] != other.Size[i])
                    return false;
            }

            for (int i = 0; i < Data.Length; i++)
            {
                if (System.Math.Abs(Data[i] - other.GetByColumnIndex(i)) > Tolerance)
                    return false;
            }

            return true;
        }

        private int Coordinate2ColumnIndex(int row, int column)
        {
            return (column * Height) + row;
        }
    }
}