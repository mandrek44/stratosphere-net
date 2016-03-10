Matrix has some operations we didn't cover yet: multiply by number, add number to every element, compute squares etc. The result of all those operations is same size matrix with every element transformed by some function, i.e:

For example: multiplying matrix by 2 (`A * 2`) can be written as `A.Map(value => value * 2)`, where `Map` works similar to LINQ `Select` function - applies transformation to every element.

## Scalar transformation

Instead of performing the operation immediately (and copy the matrix memory in process) we can instead create an object that represents the transformation behavior (again this is similar to the way `Select` works). Such object will only evaluate the transformed value when accessing matrix data:

    public class ScalarTransformedMatrix : Matrix
    {

We store the original matrix and the transformation function that we'll apply to each element
  
        private readonly Matrix _matrix;
        private readonly Func<double, double> _transformation;
    
        public ScalarTransformedMatrix( Matrix matrix, Func< double, double> transformation ) : base(matrix.Size)
        {
            _matrix = matrix;
            _transformation = transformation;
        }

Then we override methods accessing the data so they transform returned value:

        public override double GetByColumnIndex( int columnIndex)
        {
            return _transformation(_matrix.GetByColumnIndex(columnIndex));
        }
    
        public override double GetByRowIndex( int rowIndex)
        {
            return _transformation(_matrix.GetByRowIndex(rowIndex));
        }
    
        public override double GetByCoordinates( int row, int column)
        {
            return _transformation(_matrix.GetByCoordinates(row, column));
        }
    }
    
Now the scalar multiplication can be expressed as:

    public static Matrix operator *(Matrix a, double scalar) => new ScalarTransformedMatrix(a, v => v * scalar);
    
And used in code:

    Matrix b = a * 2.5;

## Element wise operations

Another type of operation we'd like to handle is element-wise matrix operation. Imagine we have two matrices - `A` and `B` - and we want to multiply each element of `A` by corresponding element of `B` (known as [Hadamard product](https://en.wikipedia.org/wiki/Hadamard_product_(matrices))). In [Octave/MatLab](http://www.mathworks.com/help/matlab/ref/times.html) this is done by using "." operator: `A .* B`. 

To implement this, we can create another transformation class that represents such operation:

    public class MatrixTransformedMatrix : Matrix
    {

Main difference between this and `ScalarTransofmedMatrix` is that we have to store additional matrix and our transformation function now accepts two values (for both matrices):

        private readonly Matrix _matrix;
        private readonly Matrix _transformationValue;
        private readonly Func<double, double, double> _transformation;
    
        public MatrixTransformedMatrix( Matrix matrix, Matrix transformationValue, Func<double , double , double > transformation) : base(matrix.Size)
        {
            _matrix = matrix;
            _transformationValue = transformationValue;
            _transformation = transformation;
        }

The value access functions look almost exactly the same - the value returned from inner matrix is transformed using the transformation function and corresponding value from second matrix:

        public override double GetByColumnIndex( int columnIndex)
        {
            return _transformation(_matrix.GetByColumnIndex(columnIndex), _transformationValue.GetByColumnIndex(columnIndex));
        }
    
        public override double GetByRowIndex( int rowIndex)
        {
            return _transformation(_matrix.GetByRowIndex(rowIndex), _transformationValue.GetByRowIndex(rowIndex));
        }
    
        public override double GetByCoordinates( int row, int column)
        {
            return _transformation(_matrix.GetByCoordinates(row, column), _transformationValue.GetByCoordinates(row, column));
        }
    }

In fact, this is very similar to approach taken when designing the [`TransposedMatrix`](https://github.com/mandrek44/stratosphere-net/blob/master/Sources/Stratosphere.Math/Matrix/TransposedMatrix.cs) class, which  flips the coordinates when value is accessed. Sub-matrices - like
[`ColumnFilteredMatrix`](https://github.com/mandrek44/stratosphere-net/blob/master/Sources/Stratosphere.Math/Matrix/ColumnsConcatMatrix.cs) - and composite matrices - like [`ColumnsConcatMatrix`](https://github.com/mandrek44/stratosphere-net/blob/master/Sources/Stratosphere.Math/Matrix/ColumnFilteredMatrix.cs) - use exactly the same technique.

Thanks to that, after adding [few operator overloads to our class](<https://github.com/mandrek44/stratosphere-net/blob/master/Sources/Stratosphere.Math/Matrix/Matrix.cs#L52>), we can write complex equations without copying matrix data multiple times, i.e:

    var resultMatrix = (1 + Y).Transpose().MultiplyEach(X.GetColumn(1)) - 1;
 
The matrix class now has enough operations to start building basic algorithms for machine learning - but that's topic for next articles.

