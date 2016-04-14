# Playing with C# type system

The Quasi-Newton method uses quite complex formula for calculating the approximation of second derivative. Implemented in Stratosphere.NET it looks like that:

    H = H + (s * s.T) / (s.T * q) - (H * q) * ((H * q).T / (q.T * H * q));
    
All operands - `H`, `s` and `q` are matrices. However until we run the code we can't even say if the operation is valid, because matrix operations require specific matrix sizes (i.e. `+` and `-` requires matrices of the same size).

## Generic Matrix

What if we'd make the compiler check if the matrix sizes are OK, and even give us information of what the resulting matrix size is? Let's try to do it with generics:  

    public class Matrix<D1, D2> 
    {
        public Matrix Inner { get; }
    
        public Matrix(Matrix inner)
        {
            Inner = inner;
        }
    
        public static implicit operator Matrix(Matrix<D1, D2> a) => a.Inner;
    }

Nothing fancy yet - just a generic class that can be constructed from matrix and converted easily back to it using implicit operator. In Matrix class, we can add a method to easily convert to this class:

    public abstract class Matrix
    {
        // ...
        
        public Matrix<D1, D2> As<D1, D2>() => new Matrix<D1, D2>(this);
        
        // ... 
    }
    
 
## Strongly typed addition

We can now define a first operation for strongly typed matrix:

    public class Matrix<D1, D2> 
    {
        // ...
        
        public static Matrix<D1, D2> operator +(Matrix<D1, D2> a, Matrix<D1, D2> b) => a.Inner.Add(b.Inner).As<D1, D2>();
        
        // ...
    }
    
Addition requires both Matrices to be same size. Just bare in mind that this code won't enforce that the matrices are indeed of the same size - it only guarantees that we declared them as the same size.

Using new addition operator we can now say:

    // Types to distinguish the dimensions
    public class M { }    
    public class N { }
    
    // ... somewhere in code ...
    public void SeriousCalculations(Matrix<M, N> m1, Matrix<M, N> m2)
    {
        Matrix<M, N> result = m1 + m2;
    }
    
That's simple, but in next post we'll try to write entire Quasi-Newton formula using strongly typed generics.