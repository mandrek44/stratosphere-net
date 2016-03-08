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

        protected Matrix(int[] dimensions)
        {
            _dimensions = dimensions;
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

        public virtual Matrix Multiply(Matrix other) => Multiply(this, other);
        public virtual Matrix Multiply(double scalar) => Map(v => v * scalar);

        public Matrix Transpose() => new TransposedMatrix(this);
        public Matrix T => Transpose();

        public static implicit operator double(Matrix a) => a.EnumerateByColumns().Single();

        public static Matrix operator *(Matrix a, double scalar) => a.Multiply(scalar);
        public static Matrix operator *(Matrix a, int scalar) => a.Multiply(scalar);
        public static Matrix operator *(int scalar, Matrix a) => a.Multiply(scalar);
        public static Matrix operator *(double scalar, Matrix a) => a.Multiply(scalar);
        public static Matrix operator *(Matrix a, Matrix b) => a.Multiply(b);

        public static Matrix operator /(Matrix a, double scalar) => a.Multiply(1d / scalar);

        public static Matrix operator -(Matrix a) => a.Map(v => -v);
        public static Matrix operator -(Matrix a, Matrix b) => a.Substract(b);
        public static Matrix operator -(double scalar, Matrix a) => a.Map(v => v - scalar);
        public static Matrix operator -(Matrix a, double scalar) => a.Map(v => v - scalar);
        public static Matrix operator +(Matrix a, Matrix b) => a.Add(b);
        public static Matrix operator +(double scalar, Matrix a) => a.Map(v => v + scalar);
        public static Matrix operator +(Matrix a, double scalar) => a.Map(v => v + scalar);

        public Matrix Map(Func<double, double> map) => new ScalarTransformedMatrix(this, map);
        public Matrix Map(Matrix b, Func<double, double, double> map) => new MatrixTransformedMatrix(this, b, map);

        public Matrix Substract(Matrix other) => Map(other, (v1, v2) => v1 - v2);
        public Matrix Add(Matrix other) => Map(other, (v1, v2) => v1 + v2);
        public Matrix MultiplyEach(Matrix matrix) => Map(matrix, (v1, v2) => v1 * v2);

        public Matrix Concat(Matrix matrix) => new ColumnsConcatMatrix(this, matrix);
        public double Sum() => EnumerateByColumns().Sum();

        public double Length => System.Math.Sqrt(EnumerateByColumns().Sum(v => v * v));

        public double this[int row, int column = 0] => GetByCoordinates(row, column);

        public abstract double GetByColumnIndex(int columnIndex);

        public abstract double GetByRowIndex(int rowIndex);

        public abstract double GetByCoordinates(int row, int column);

        public Matrix GetColumn(int column) => new ColumnFilteredMatrix(this, column);

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

        public Matrix Sum(int dimension)
        {
            if (_dimensions.Length > 2)
                throw new NotSupportedException();

            int maxesLength = _dimensions[1 - dimension];
            var maxes = new double[maxesLength];

            int i = 0;
            for (int col = 0; col < _dimensions[1]; ++col)
            {
                for (int row = 0; row < _dimensions[0]; ++row, ++i)
                {
                    var maxIndex = dimension == 0 ? col : row;
                    maxes[maxIndex] += GetByColumnIndex(i);
                }
            }

            return new ColumnMajorMatrix(maxes, dimension == 0 ? new[] { 1, _dimensions[1] } : new[] { _dimensions[0], 1 });
        }

        public Matrix Evaluate()
        {
            var result = new double[Size.Product()];
            int i = 0;
            foreach (var value in EnumerateByColumns())
            {
                result[i++] = value;
            }

            return new ColumnMajorMatrix(result, Size.ToArray());
        }

        public Matrix Max(int dimension = 0)
        {
            if (_dimensions.Length == 2 && (_dimensions[0] == 1 || _dimensions[1] == 1))
                return new ColumnMajorMatrix(new[] { EnumerateByColumns().Max() }, new int[] { 1, 1 });

            if (_dimensions.Length != 2)
                throw new MultiDimensionalMatrixNotSupportedException();

            int maxesLength = _dimensions[1 - dimension];
            var maxes = new double[maxesLength];
            for (int k = 0; k < maxesLength; ++k)
            {
                maxes[k] = double.MinValue;
            }

            int i = 0;
            for (int col = 0; col < _dimensions[1]; ++col)
            {
                for (int row = 0; row < _dimensions[0]; ++row, ++i)
                {
                    var maxIndex = dimension == 0 ? col : row;
                    var value = GetByColumnIndex(i);
                    if (value > maxes[maxIndex])
                        maxes[maxIndex] = value;
                }
            }

            return new ColumnMajorMatrix(maxes, dimension == 0 ? new[] { 1, _dimensions[1] } : new[] { _dimensions[0], 1 });
        }

        public Matrix Min(int dimension = 0)
        {
            if (_dimensions.Length == 2 && (_dimensions[0] == 1 || _dimensions[1] == 1))
                return new ColumnMajorMatrix(new[] { EnumerateByColumns().Min() }, new int[] { 1, 1 });

            if (_dimensions.Length != 2)
                throw new MultiDimensionalMatrixNotSupportedException();

            int maxesLength = _dimensions[1 - dimension];
            var maxes = new double[maxesLength];
            for (int k = 0; k < maxesLength; ++k)
            {
                maxes[k] = double.MaxValue;
            }

            int i = 0;
            for (int col = 0; col < _dimensions[1]; ++col)
            {
                for (int row = 0; row < _dimensions[0]; ++row, ++i)
                {
                    var maxIndex = dimension == 0 ? col : row;
                    var value = GetByColumnIndex(i);
                    if (value < maxes[maxIndex])
                        maxes[maxIndex] = value;
                }
            }

            return new ColumnMajorMatrix(maxes, dimension == 0 ? new[] { 1, _dimensions[1] } : new[] { _dimensions[0], 1 });
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
                    builder.AppendLine(";");
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

        public static Matrix Zeros(int rank) => Zeros(rank, rank);
        public static Matrix Zeros(params int[] dimensions) => new ColumnMajorMatrix(new double[dimensions.Product()], dimensions);

        public static Matrix Ones(int rank) => Ones(rank, rank);
        public static Matrix Ones(params int[] dimensions)
        {
            var ones = new double[dimensions.Product()];
            for (int i = 0; i < ones.Length; i++)
            {
                ones[i] = 1;
            }

            return new ColumnMajorMatrix(ones, dimensions);
        }
    }

}