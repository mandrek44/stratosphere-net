# Playing with C# type system - Part 3
## Dot product

There a special case we need to handle: when the result multiplying two matrices is a single number. This happen if `A` is *1 x n* matrix and `B` is *n x 1* matrix, then the result is by definition *1 x 1*, which is just a number. It should be simple to define this special case with overload:

    public class One { }

    public class Matrix<D1, D2>
    {
        // ...
    
        public static Matrix<D1, One> operator *(Matrix<D1, D2> a, Matrix<D2, One> b) => a.Inner.Multiply(b.Inner).As<D1, One>();
        
        // ...
    }
    
Having this we can define additional operator for division:

    public static Matrix<D1, D2> operator / (Matrix<D1, D2> a, Matrix<One, One> scalar) => a.Inner.Map(v => v / scalar.Inner).As<D1, D2>();
    
Notice that the second operand is explicitly defined as `Matrix<One, One>`, so compiler won't let you divide anything else.

## Back to the formula

So now when you write down the formula again:

    Matrix<n, n> H = ...
    Matrix<n, One> q = ...
    Matrix<n, One> s = ...

    var dH = (s*s.T)/(s.T*q) - (H*q)*((H*q).T/(q.T*H*q));

The compiler will tell you `dH` is of type `Matrix<n, n>`
