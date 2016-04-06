using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratosphere.Math
{
    // TODO: If RowMajorMatrix (or any other version) is implemented, this will need to change as it assumes the indexes are Column Major
    public abstract class Matrix
    {
        protected const double Tolerance = 0.0001;
        private readonly int[] _dimensions;

        protected Matrix()
        {
        }

        protected Matrix(int[] dimensions)
        {
            _dimensions = dimensions;
        }

        public static implicit operator Matrix(string matrix) => ColumnMajorMatrix.Parse(matrix);

        public static Matrix Scalar(double v) => new ColumnMajorMatrix(new[] { v }, new[] { 1, 1 });

        public static Matrix Vector(params double[] v) => new ColumnMajorMatrix(v, new[] { v.Length, 1 });

        public IEnumerable<int> IndexesByRows()
        {
            for (int j = 0; j < Height; ++j)
                for (int i = 0; i < Width * Height; i += Height)
                    yield return i + j;
        }

        public IEnumerable<int> IndexesByColumns()
        {
            for (int i = 0; i < Size.Product(); ++i)
                yield return i;
        }

        public IEnumerable<double> EnumerateByColumns() => IndexesByColumns().Select(Get);

        public IEnumerable<double> EnumerateByRows() => IndexesByRows().Select(Get);

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
        public static Matrix operator -(double scalar, Matrix a) => a.Map(v => scalar - v);
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

        public abstract double Get(int index);

        public abstract double GetByCoordinates(int row, int column);

        public Matrix GetColumn(int column) => new SingleColumnMatrix(this, column);

        public Matrix FilterRows(Func<Matrix, bool> func) => new RowFilteredMatrix(this, func);

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
                    maxes[maxIndex] += Get(i);
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
                    var value = Get(i);
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
                    var value = Get(i);
                    if (value < maxes[maxIndex])
                        maxes[maxIndex] = value;
                }
            }

            return new ColumnMajorMatrix(maxes, dimension == 0 ? new[] { 1, _dimensions[1] } : new[] { _dimensions[0], 1 });
        }

        public Matrix Inverse()
        {
            if (Width != Height)
                throw new NotSupportedException("Only square matrices are supported");

            if (Width == 1)
            {
                return Vector(1.0/this[0]);
            }

            if (Width == 2)
            {
                var a = this[0, 0];
                var b = this[1, 0];
                var c = this[0, 1];
                var d = this[1 ,1];

                return Vector(d, -c).Concat(Vector(-b, a)) / (a * d - b * c);
            }

            if (Width == 3)
            {
                var a = this[0, 0];
                var b = this[0, 1];
                var c = this[0, 2];
                var d = this[1, 0];
                var e = this[1, 1];
                var f = this[1, 2];
                var g = this[2, 0];
                var h = this[2, 1];
                var i = this[2, 2];

                var A = e * i - f * h;
                var B = -d * i + f * g;
                var C = d * h - e * g;
                var D = -b * i + c * h;
                var E = a * i - c * g;
                var F = -a * h + b * g;
                var G = b * f - c * e;
                var H = -a * f + c * d;
                var I = a * e - b * d;

                var detA = a * A + b * B + c * C;

                return Vector(A, B, C).Concat(Vector(D, E, F)).Concat(Vector(G, H, I)) / detA;
            }
            else
            {
                throw new NotSupportedException("Only 1 x 1, 2 x 2 or 3 x 3 matrices are supported.");
            }
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

        public bool Equals(Matrix other)
        {
            if (Size.Length != other.Size.Length)
                return false;

            if (Size.Where((t, i) => t != other.Size[i]).Any())
            {
                return false;
            }

            var thisValues = EnumerateByColumns().GetEnumerator();
            var otherValues = other.EnumerateByColumns().GetEnumerator();

            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (thisValues.Current != otherValues.Current)
                    return false;
            }

            return true;
        }

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
                return (_dimensions?.GetHashCode() ?? 0);
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

        public static Matrix Identity(int rank)
        {
            var identity = new double[rank * rank];
            for (int i = 0; i < rank; i++)
            {
                identity[i * rank + i] = 1;
            }

            return new ColumnMajorMatrix(identity, new[] { rank, rank });
        }
    }

}