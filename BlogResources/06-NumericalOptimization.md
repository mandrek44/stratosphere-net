
# Numerical Optimization - Steepest Gradient Descent

Having built basic Matrix operations we're now ready to look into some useful algorithms that will form basis for our machine learning library.

Numerical optimization algorithms are used to find a minimum or maximum of a function. In machine learning problems, they form a base for building complex algorithms like neural networks, support vector machines etc.

Let's look into one of the simplest algorithms for finding local minimum - [Steepest Gradient Descent](<https://en.wikipedia.org/wiki/Gradient_descent>).

## Gradient

The "gradient" in algorithm name refers to derivative of function we'll minimize. For those of you not feeling that well about calculus: the derivative of the function describes how the function is changing in a neighborhood of a given point.

Say we take some point `x`. If derivative (`df(x)`) is greater that zero, it means that the function value (`f(x)`) near this point is increasing. If it's less than zero, then the function is decreasing. If derivative is exactly 0 it means the function is constant at this point.

![Function derivative](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-03-17_20-16-22.png)

## Descent

Our algorithm will start at some arbitrary point. "Being" at this point it has to choose where to go next. It will use the gradient information - it will move in the direction where the function is guaranteed to become smaller. 

Assuming that `df(x0)` is gradient at point `x0`, if `df(x0) > 0` then the function is increasing (right-hand side of plot above), so to find the minimum we must look at left-hand side of `x0`: `x1 < x0` 

If the `df(x0) < 0` then function is decreasing (left-hand side of plot) so we want to look to the right: `x1 > x0`.

Let's write it in more compact form:

 - if `df(x0) > 0` then `x1 < x0`
 - if `df(x0) < 0` then `x1 > x0`

Looking at this, we can generalize that we must search in the opposite direction of gradient. In fact this is exactly what the algorithm does, even for functions of multiple variables (then the gradient is a [vector of all partial derivatives](<https://en.wikipedia.org/wiki/Jacobian_matrix_and_determinant>)). 

## xNext

Now when we know in what direction to search, we have to choose how far we want to go in that direction. Since function derivate is zero at optimum (function doesn't change there) it's safe to assume that as it approaches the minimum it will become more flat. 

If so, we can then make our step proportional to the actual value of derivative (but in opposite direction!):

`x1 ~ x0 - df(x0)`

This is fine, however let's just add final element to this:

`x1 = x0 - alpha*df(x0)`, where `0 < alpha < 1`

Since we don't know the what the slope is we'll need to control our step size - that's what the `alpha` parameter is for.

This algorithm will produce following steps (with `alpha` set to `0.1`):

![Gradient descent steps](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-03-17_20-28-45.png)

Notice that steps become denser as we approach the minimum - that's because we made it the proportional to the derivative.

## Step size

If the step is too small we'll end up doing very little progress towards minimum. If it's too big, we can end up with situation like this:

![Too big step size](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-03-17_20-48-51.png)

... which is also sub optimal. 

There are algorithms for find right value, but that's topic for another time.

## Implementation

Wrapping it up, you could write down the algorithm as follows:

    Matrix x = x0;
    var fx = f(x0);
    
Terminate the algorithm if it runs for too many iterations:    
    
    for (var i = 0; i < MaxIterations; ++i)
    {
    
Calculate next step:    
    
        var x1 = (x - df(x) * alpha).Evaluate();
    
    
Terminate if value is not changed (`Epsilon` is very small number):    

        var fx1 = f(x1);
        if (fx - fx1 < Epsilon)
            return x1;
    
        x = x1;
        fx = fx1;
    }


## Using the code

After putting above into single method, here's example of running the algorithm against `f(x) = x^2`:

    var xmin = SimpleSteepestDescentMethod.Find(
        f: x => x[0]*x[0], 
        df: x => 2 * x, 
        x0: Matrix.Scalar(-2), 
        alpha: 0.1, 
        maxIterations: 1000);
        

In next post we'll see how we can put this algorithm to use in some basic machine learning problem.
