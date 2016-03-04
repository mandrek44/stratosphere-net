# Matrix transpose

Matrix transposition, for 2 dimensional matrix, is done by writing columns as rows and vice versa. 

For example, given following matrix:

    1 2 3
    4 5 6

... it's transposition will look like that:

    1 4
    2 5
    3 6

Switching column and rows - that sounds familiar! Different ways of [indexing our matrix](<http://marcindrobik.pl/Post/MatricesIntroduction>) does exactly that! Let's give it a try and create a new class using [decorator pattern](<https://en.wikipedia.org/wiki/Decorator_pattern>), that will represent transposed matrix:

    public class TransposedMatrix : Matrix
    {
        private readonly Matrix _matrix;
    
        public TransposedMatrix(Matrix matrix) :base(matrix)
        {
            Size = matrix.Size.Reverse().ToArray();
            _matrix = matrix;
        }
    
        public override double GetByColumnIndex( int index)
        {
            return _matrix.GetByRowIndex(index);
        }
    
        public override double GetByRowIndex( int index)
        {
            return _matrix.GetByColumnIndex(index);
        }
    }

You'll notice that `GetByColumnIndex` in fact returns `GetByRowIndex` of decorated matrix and `GetByRowIndex` returnes `GetByColumnIndex` of decorated matrix, thus swaping the columns and rows on the fly - no memory copying is needed.

The dimensions are also reversed - Transposing 3 x 2 matrix will result in 2 x 3 matrix.

In Matrix class, we simply return transposed (decorated) self:

    public Matrix Transpose()
    {
        return new TransposedMatrix( this);
    }

Next, add [little test](<https://github.com/mandrek44/stratosphere-net/blob/master/UnitTests/Stratosphere.Tests.Math/Given_2x3_matrix.cs#L47>) to verify it works and Voilà! 

You can find [TransposedMatrix](<https://github.com/mandrek44/stratosphere-net/blob/master/Sources/Stratosphere.Math/TransposedMatrix.cs>) class on project's [GitHub repo](<https://github.com/mandrek44/stratosphere-net>).