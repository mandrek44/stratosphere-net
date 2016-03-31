# Optimizing with Newton's Method

[Newton's Method](<https://en.wikipedia.org/wiki/Newton%27s_method>) is used to find [function's roots (zeros)](<https://en.wikipedia.org/wiki/Zero_of_a_function>). The method is somehow similar to [Steepest Descent](<http://marcindrobik.pl/Post/NumericalOptimizationSteepestGradientDescent>) - it uses derivative to perform steps. The goal is however different - Steepest Descent uses it to find local minimum.

Newton's Method uses following steps to approximate the roots:

![](https://mandrostorage.blob.core.windows.net/blogfiles/NewtonMethod.jpg)

Here's a good visualization of the method from [Wikipedia (by Ralf Pfeifer)](<https://commons.wikimedia.org/wiki/File:NewtonIteration_Ani.gif>):

![](https://upload.wikimedia.org/wikipedia/commons/e/e0/NewtonIteration_Ani.gif)

## Optimization?

What does it have to do with optimization? 

Recall that at local optimum, the function's first derivate equals to zero. If so, then we can say that searching for minimum is searching for first derivative's zero - and for that we can use Newton's method.

To do so, when using [Newton's Method for optimizing](<https://en.wikipedia.org/wiki/Newton%27s_method_in_optimization>) of function `f` (instead of finding it's roots) we'll need information about both first and second derivatives:

![](https://mandrostorage.blob.core.windows.net/blogfiles/NewtonMethodOptimization.jpg)


## Implementation ...

Here's a simple implementation that assumes both derivatives can be calculated:

    public Matrix Find(Func<Matrix, Matrix> df, Func<Matrix, Matrix> ddf, Matrix initial, double alpha)
    {
        var x = initial;
        
`MaxIterations` is some constant to make sure the algorithm won't iterate forever (i.e. `1000`)        
        
        for (int i = 0; i < MaxIterations; ++i)
        {        
        
This is vectorized version for Newton's optimization - second derivative is inversed instead of being in denominator. `alpha` is used to control the step length - similar to Gradient Descent.          
        
            var x2 = x - alpha * ddf(x).Inverse()*df(x);

Algorithm will terminate when the step made is very small (i.e. `0.0001`):

            if ((x2 - x).Length < Epsilon)
                return x2;

            x = x2;
        }
        
        return x;
    }
  
## Let's run it!

And here's the result for Banana function (with `alpha` set to `0.5`):

![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-03-31_22-11-36.png)
