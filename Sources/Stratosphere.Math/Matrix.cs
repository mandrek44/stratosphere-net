using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stratosphere.Math
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

        public IEnumerable<double> EnumerateByColumns()
        {
            return IndexesByColumns().Select(i => Data[i]);
        }

        public IEnumerable<double> EnumerateByRows()
        {
            return IndexesByRows().Select(i => Data[i]);
        }

        public abstract double GetByColumnIndex(int index);

        public abstract double GetByRowIndex(int index);

        public virtual int[] Size => _dimensions;

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