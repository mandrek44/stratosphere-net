# Matrix Multiplication

Today let's look at one of the most important operations in matrix-based algorithms - matrix product (multiplication of two matrices). 

## Introduction

[![Matrix Product](https://mandrostorage.blob.core.windows.net/blogfiles/Matrix_multiplication_diagram_2.svg)](<https://en.wikipedia.org/wiki/File:Matrix_multiplication_diagram_2.svg>)

Wikipedia has a [great article](<https://en.wikipedia.org/wiki/Matrix_multiplication#Matrix_product_.28two_matrices.29>) together with [some examples](https://en.wikipedia.org/wiki/Matrix_multiplication#Row_vector_and_column_vector) describing the multiplication very well. Among tons of information it gives following formula for multiplication:

![Definition of matrix product](https://mandrostorage.blob.core.windows.net/blogfiles/MatrixProductDefinition.png)

## Code

Let's write a simple algorithm that implements the multiplication directly from above formula. For clarity we'll rename the indexes `i` and `j` to `row`and `column` respectively.


    public ColumnMajorMatrix Multiply(Matrix other)
    {
    
First validate the input matrices dimensions:
    
        if (Width != other.Height)
            throw new InvalidOperationException("Matrix dimensions must agree");                
               
        var resultData = new double[Height * other.Width];
    
Start by iterating through each cell in result matrix...    
    
        for (int column = 0; column < other.Width; ++column)
        {
            for (int row = 0; row < Height; ++row)
            {
            
... and compute its value by multiplying corresponding row and column from input matrices:
            
                double sum = 0;
                for (int k = 0; k < Width; ++k)
                {
                    sum += GetByCoordinates(row, k)*other.GetByCoordinates(k, column);
                }

Save the value of computed cell to result array:

                resultData[Coordinate2ColumnIndex(row, column)] = sum;
                  
            }
        };
    
    
Since the `resultData` is stored using [column major indexing](<http://marcindrobik.pl/Post/MatricesIntroduction>), we return it as `ColumnMajorMatrix`:  

        return new ColumnMajorMatrix(resultData, new[] { Height, other.Width });
    }


The `Coordinate2ColumnIndex` simply calculates column-first index:

    private int Coordinate2ColumnIndex(int row, int column)
    {
        return (column * Height) + row;
    }
    

## It that all?

First of all, this algorithm's computational complexity is [O(n<sup>3</sup>)](<https://en.wikipedia.org/wiki/Big_O_notation>). There are algorithms that can do [better than this](<https://en.wikipedia.org/wiki/Strassen_algorithm>), not only by reducing the complexity, but also by improving [data locality](<https://en.wikipedia.org/wiki/Locality_of_reference>) and [parallelism](<https://en.wikipedia.org/wiki/Matrix_multiplication_algorithm#Parallel_and_distributed_algorithms>). Some implementations may even use your [GPU](<https://en.wikipedia.org/wiki/CUDA>) to speed up things a little.

But that's a topic for another time.