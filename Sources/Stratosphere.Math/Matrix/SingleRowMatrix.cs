namespace Stratosphere.Math
{
    public class SingleRowMatrix : Matrix
    {
        private readonly Matrix _matrix;
        private readonly int _row;

        public SingleRowMatrix(Matrix matrix, int row) : base(new[] { 1, matrix.Width })
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