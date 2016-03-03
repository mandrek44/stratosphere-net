using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Stratosphere.Math
{
    public class ColumnMajorMatrix : Matrix
    {
        private ColumnMajorMatrix(double[] data, int[] dimensions) : base(data, dimensions)
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
        public static ColumnMajorMatrix operator *(ColumnMajorMatrix a, double scalar) => a.Multiply(scalar);
        public static ColumnMajorMatrix operator *(ColumnMajorMatrix a, int scalar) => a.Multiply(scalar);
        public static ColumnMajorMatrix operator *(int scalar, ColumnMajorMatrix a) => a.Multiply(scalar);
        public static ColumnMajorMatrix operator *(double scalar, ColumnMajorMatrix a) => a.Multiply(scalar);

        public ColumnMajorMatrix Multiply(double scalar)
        {
            return new ColumnMajorMatrix(
                Data.Select(value => value * scalar).ToArray(),
                Size.ToArray());
        }

        public ColumnMajorMatrix Multiply(Matrix other)
        {
            if (Size.Length != 2)
                throw new MultiDimensionalMatrixNotSupportedException();

            if (Size[1] != other.Size[0])
                throw new InvalidOperationException("Matrix dimensions must agree");
            
            var resultData = new double[Size[0] * other.Size[1]];

            int rowSize = Size[0];

            for (int col = 0; col < other.Size[1]; ++col)
            {
                var columnOffset = col * other.Size[0];
                for (int row = 0; row < Size[0]; ++row)
                {
                    var rowOffset = row * Size[1];

                    double sum = 0;
                    for (int i = 0; i < Size[1]; ++i)
                    {
                        sum += GetByRowIndex(i + rowOffset) * other.GetByColumnIndex(i + columnOffset);
                    }

                    resultData[row + col * rowSize] = sum;
                }
            };

            return new ColumnMajorMatrix(resultData, new[] { Size[0], other.Size[1] });
        }

        public override double GetByColumnIndex(int index) => Data[index];

        public override IEnumerable<int> IndexesByColumns()
        {
            for (int i = 0; i < Data.Length; ++i)
                yield return i;
        }

        public override double GetByRowIndex(int index)
        {
            if (Size.Length != 2)
                throw new MultiDimensionalMatrixNotSupportedException();

            var row = index / Size[1];
            var column = index % Size[1];

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

        public Matrix Transpose()
        {
            return new TransposedMatrix(this);
        }
    }
}