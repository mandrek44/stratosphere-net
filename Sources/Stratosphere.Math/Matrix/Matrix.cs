using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratosphere.Math.Matrix
{
    public abstract class Matrix
    {
        protected const double Tolerance = 0.0001;
        private readonly int[] _dimensions;
        protected double[] Data { get; }

        protected Matrix()
        {
        }

        protected Matrix(double[] data, int[] dimensions)
        {
            Data = data;
            _dimensions = dimensions;
        }

        protected Matrix(Matrix template)
        {
            Data = template.Data;
            _dimensions = template._dimensions;
        }

        public abstract IEnumerable<int> IndexesByRows();

        public abstract IEnumerable<int> IndexesByColumns();

        // TODO: If RowMajorMatrix (or any other version) is implemented, this will need to change as it assumes the indexes are Column Major
        public IEnumerable<double> EnumerateByColumns() => IndexesByColumns().Select(GetByColumnIndex);

        public IEnumerable<double> EnumerateByRows() => IndexesByRows().Select(GetByColumnIndex);

        public virtual Matrix Multiply(double scalar) => new ScalarTransformedMatrix(this, v => v*scalar);

        public virtual Matrix Multiply(Matrix other) => Multiply(this, other);

        public virtual Matrix Substract(Matrix other) => new MatrixTransformedMatrix(this, other, (thisV, otherV) => thisV - otherV);
        public virtual Matrix Add(Matrix other) => new MatrixTransformedMatrix(this, other, (thisV, otherV) => thisV + otherV);

        public Matrix Transpose() => new TransposedMatrix(this);

        public static Matrix operator *(Matrix a, double scalar) => a.Multiply(scalar);
        public static Matrix operator *(Matrix a, int scalar) => a.Multiply(scalar);
        public static Matrix operator *(int scalar, Matrix a) => a.Multiply(scalar);
        public static Matrix operator *(double scalar, Matrix a) => a.Multiply(scalar);
        public static Matrix operator *(Matrix a, Matrix b) => a.Multiply(b);

        public static Matrix operator /(Matrix a, double scalar) => a.Multiply(1d/scalar);
        public static Matrix operator -(Matrix a) => new ScalarTransformedMatrix(a, v => -v);

        public double this[int row, int column = 0] => GetByCoordinates(row, column);

        public static Matrix operator -(Matrix a, Matrix b) => a.Substract(b);
        public static Matrix operator +(Matrix a, Matrix b) => a.Add(b);

        public double Length => System.Math.Sqrt(Data.Sum(v => v*v));

        public abstract double GetByColumnIndex(int columnIndex);

        public abstract double GetByRowIndex(int rowIndex);

        public abstract double GetByCoordinates(int row, int column);

        public virtual int[] Size => _dimensions;

        public int Height => Size[0];

        public int Width => Size[1];

        public static Matrix Multiply(Matrix a, Matrix b)
        {
            if (a.Size.Length != 2)
                throw new MultiDimensionalMatrixNotSupportedException();

            if (a.Width != b.Height)
                throw new InvalidOperationException("Matrix dimensions must agree");

            var resultData = new double[a.Height * b.Width];

            for (int column = 0; column < b.Width; ++column)
            {
                for (int row = 0; row < a.Height; ++row)
                {
                    double sum = 0;
                    for (int k = 0; k < a.Width; ++k)
                    {
                        sum += a.GetByCoordinates(row, k) * b.GetByCoordinates(k, column);
                    }

                    resultData[(column * a.Height) + row] = sum;
                }
            };

            return new ColumnMajorMatrix(resultData, new[] { a.Height, b.Width });
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine();

            int i = 0;
            foreach (var value in EnumerateByRows())
            {
                builder.AppendFormat("{0:0.##}", value);
                if ((i + 1) % Size[1] == 0)
                    builder.AppendLine();
                else
                    builder.Append(' ');

                ++i;
            }

            return builder.ToString();
        }

        protected abstract bool Equals(Matrix other);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var matrix = obj as Matrix;
            if (matrix == null) return false;
            return Equals(matrix);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Data?.GetHashCode() ?? 0) * 397) ^ (_dimensions?.GetHashCode() ?? 0);
            }
        }
    }
}