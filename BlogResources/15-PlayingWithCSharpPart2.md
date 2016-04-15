# Playing with C# type system - Part 2

Last time I introduced the strongly typed `Matrix` class with some simple operation. Let's try to leverage this so that compiler can help us construct more advanced formulas.

## The formula

Imagine that writing down following formula (`s`, `q` and `H` are matrices): 

    var dH = (s * s.T) / (s.T * q) - (H * q) * ((H * q).T / (q.T * H * q));
    
To use strongly typed matrices in this formula we need several operations defined: `+` (defined in last post), `-` (same as `+`), Transposition, `/` and `*`.

Let's do Transposition first. It quite simple - the returned matrix will have swapped dimensions:

    public class Matrix<D1, D2>
    {    
        // ...

        public Matrix<D2, D1> T => Inner.Transpose().As<D2, D1>();
    
        // ...
    }
 
## Multiplication

It's bit more complex for multiplication. From Wikipedia: if `A` is an *n × m* matrix and `B` is an *m × p* matrix, their matrix product `AB` is an *n × p* matrix. Let's give it a try: 

    // I changed type parameter names to match the definition above
    public class Matrix<n, m>
    {    
        // Type p is not defined!
        public static Matrix<n, p> operator *(Matrix<n, m> a, Matrix<m, p> b) => a.Inner.Multiply(b.Inner).As<n, p>();
    }

    
The problem is that type `p` is not defined - it cannot be a type parameter, so the only option is to actually have if defined as a class:

    public class p { }
    
A bit dirty hack but it compiles and gives a necessary compiler check:

    public class n { }
    public class m { }

    public void MultiplicationTest()
    {
        Matrix<n, m> A = null; // some actual values go here
        Matrix<m, p> B = null;
    
        Matrix<n, p> AB = A*B; 
    }
    
 However if you try this:
 
    public void NotValid()
    {
        Matrix<m, m> A = null; // some actual values go here
        Matrix<m, p> B = null;
    
        // Does not compile - operator * is not defined for those matrices
        var AB = A*B; 
    }
    
 ... you'll get a compiler error.
