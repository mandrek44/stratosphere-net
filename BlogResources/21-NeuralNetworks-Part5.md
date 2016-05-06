# Introduction to Neural Networks - Part 5 (Learning)

Until now we discussed a neural network for which both the structure - number and sizes of each layer - and the weights were known.

Today we'll see how we can find the parameters `theta` for given neural network (it's structure is still defined upfront). 

## Putting it all together

In [Part 3](<http://marcindrobik.pl/Post/IntroductiontoNeuralNetworksPart3CostFunction>) of these series we saw how to calculate the cost for specific choice of parameters `theta`. In [Part 4](<http://marcindrobik.pl/Post/IntroductiontoNeuralNetworksPart4Backpropagation>) we used the Backpropagation algorithm to calculate errors for each layer. Having those error we could then calculate partial derivatives of the cost function.

Having the cost function and it's derivatives, we should have everything to use some optimization method and find best `theta` parameters, right?

The optimization algorithms we used until now expect the cost function to be of type `Func<Matrix, double> f`, where the input parameter of the function are the `theta` parameter. So how do we pass all our theta parameters as single matrix?

## That's how I roll

We may use simple trick to pack `theta` for each layer into single Matrix by putting their raw data next to each other, and pretend they are 1D matrix. Let's call this operation *unrolling*. Then, when we need to calculate something we can convert the 1D matrix back to 2D matrices by splitting the data back to original form.

Example:

    theta 1:
    1 3
    2 4
    
    theta 2:
    5 8
    6 9
    7 10

Then the *unrolled* version would be:

    1 2 3 4 5 6 7 8 9 10
    
You can easily reverse the operation if you know the original matrix dimensions:

    | theta 1             | theta 2             |
    | column 1 | column 2 | column 1 | column 2 |
     1 2         3 4        5 6 7      8 9 10

I prepared a simple Matrix decorator class - `UnrolledMatrix` - that handles those operations.

## Unrolled functions

Using this technique we can prepare a cost function that accepts the unrolled parameters and the training data:

    double Cost(Matrix unrolledThetas, Matrix X, Matrix y)
    {

For new we'll use the `theta` parameters for the Xor network example:

        var unrolledMatrix = UnrolledMatrix.Parse(unrolledThetas, new [] {3, 3}, new [] {4, 2}, new [] { 3, 1});
    
        var theta1 = unrolledMatrix.Matrices[0];
        var theta2 = unrolledMatrix.Matrices[1];
        var theta3 = unrolledMatrix.Matrices[2];
    
        Matrix<m, n> dJ1;
        Matrix<n, p> dJ2;
        Matrix<p, One> dJ3;

Notice that the backpropagation will calculate both the activations and the partial derivatives in one pass, but for cost we don't use the partial derivatives:

        var result = FeedforwardBackpropagate(X, y, theta1.As<m, n>(), theta2.As<n, p>(), theta3.As<p, One>(), out dJ1, out dJ2, out dJ3);
        
        return Cost(result, y.As<One, k>());
    }
    
Function that calculates gradient information looks exactly the same, but instead of cost it returns the unrolled partial derivatives:


    static Matrix Gradient(Matrix untolledThetas, Matrix X, Matrix y)
    {
        // same code as in Cost function ...
    
        FeedforwardBackpropagate(X, y, theta1.As<m, n>(), theta2.As<n, p>(), theta3.As<p, One>(), out dJ1, out dJ2, out dJ3);
    
        return new UnrolledMatrix(dJ1, dJ2, dJ3);
    }

## Let's run it!

Finally we're now able to optimize the parameters of the neural network:

    Matrix FindMinimum(Matrix X, Matrix Y)
    {
        var fminunc = new BacktrackingSteepestDescentMethod(maxIterations: 2000);
    
        return fminunc.Find(
            f: unrolledThetas => Cost(unrolledThetas, X, Y),
            df: unrolledThetas => Gradient(unrolledThetas, X, Y),
            initial: InitialThetas());
    }
    
(`InitialThetas` returns random, unrolled `theta` matrices)

Result? It will be different every time you run it because of the random initialization, here's one example (it's unrolled):
 
    0,83  -1  1,2  0,48  4,43  -4,51  -3,29  5,58  -5,71  0,23  2,99  -9,44  11,6  -1,19  -0,18  5,24  -6,02  -6,8  22,75  -8,93 
    
 They look very different from the original `XOR` parameters, however if we run the tests for `XOR` for them:
 
 ![](https://mandrostorage.blob.core.windows.net/blogfiles/devenv_2016-04-30_18-12-25.png)

Works, yay!