using System.Collections.Generic;
using System.Linq;

namespace Stratosphere.Math
{
    public class TransposedMatrix : Matrix
    {
        private readonly Matrix _matrix;

        public TransposedMatrix(Matrix matrix) : base(matrix)
        {
            _matrix = matrix;
            Size = matrix.Size.Reverse().ToArray();
        }

        public override double GetByColumnIndex(int index)
        {
            return _matrix.GetByRowIndex(index);
        }

        public override double GetByRowIndex(int index)
        {
            return _matrix.GetByColumnIndex(index);
        }

        public override IEnumerable<int> IndexesByColumns()
        {
            return _matrix.IndexesByRows();
        }

        public override IEnumerable<int> IndexesByRows()
        {
            return _matrix.IndexesByColumns();
        }

        public override int[] Size { get; }

        protected override bool Equals(Matrix other)
        {
            return _matrix.Equals(other);
        }
    }
}