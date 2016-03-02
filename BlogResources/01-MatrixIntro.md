
Before we get started with algorithms used in Machine Learning, we need to build some basic matrix-based math. We could use something that's already implemented, but hey - we'd miss all the fun of implementing it ourselves! 

## Data Structure

Let's start with some basic matrix structure. We'd like to build a matrix that isn't just s a 2-dimensional table of numbers but instead could hold any number of dimensions. How we could store this in memory? Let's  just use a simple array of values and append every next dimension at the end of array.

In Octave/MatLab column is the first dimension of matrix and row is second (and then higher order dimensions). This kind of matrix is called "column major". So, when we say A(2,1) we mean "second row, first column of matrix A". To store such matrix in memory we'll organize data as follows:

![Column major matrix](https://mandrostorage.blob.core.windows.net/blogfiles/MemoryStructure_ColumnMajor.png)

    public class ColumnMajorMatrix
    {
        private readonly double[] _data;
        private readonly int[] _dimensions;
    
        public ColumnMajorMatrix( double[] data, int[] dimensions)
        {
            _data = data;
            _dimensions = dimensions;
        }
    }

Alternative approach would be to store the data in row-first (or "row major") fashion:

![Row major matrix](https://mandrostorage.blob.core.windows.net/blogfiles/MemoryStructure_RowMajor.png)

We'll get back to it on another occasion.

## Data Access

Like in storage, there are two main approaches to index and retrieve the data from the matrix:
### 1) Column-first indexing:

![Column first index](https://mandrostorage.blob.core.windows.net/blogfiles/Enumerate_ColumnFirst.png)

For column major matrix, this reflects the order of data in memory, so the code for such index is straightforward:

    public double GetByColumnIndex(int index) => _data[index];

### 2) Row-first indexing:

![Row first index](https://mandrostorage.blob.core.windows.net/blogfiles/Enumerate_RowFirst.png)

For column major matrix, this indexing is bit more complex. Simplifying to 2D matrices only:

    public double GetByRowIndex(int index)
    {
        var row = index/_dimensions[1];
        var column = index % _dimensions[1];

        return _data[row + column*_dimensions[0]];
    }


## Summary

In next post we'll look into basic Matrix operations - like Transposing and Multiplication - and see how different indexing will help us designing efficient algorithms for those operations.
