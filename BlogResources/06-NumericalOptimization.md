
# Numerical Optimization - Steepest Gradient Descent

Having built basic Matrix operations we're now ready to look into some useful algorithms that will form basis for our Machine Learning library - a optimization algorithms.

Numerical optimization algorithms are used to determine best fitting solution for given problem. In their purest form they are used to find a minimum or maximum of a function, but in fact they form a base for every complex machine learning algorithm.

Let's look into one of the simplest algorithms for finding local minimum - Steepest Gradient Descent.

## Gradient

The "gradient" in algorithm name refers to function gradient - or derivative - that we'll minimize. For those of you not feeling that well about calculus: the derivative of the function describes how the function is changing in a neighborhood of a given point.

Let's say we take some point x. If derivative is greater that zero, it means that the function value near this point is increasing. If it's less than zero, then the function is decreasing. If derivative is exactly 0 it means the function is constant at this point.

## Descent

Our algorithm will start at some arbitrary point. "Being" at this point it has to choose where to go next. It will use the gradient information - it will move in the direction where the function is guaranteed to become smaller. 

Assuming that `df(x0)` is gradient at point `x0`, if `df(x0) > 0` then it means the function is increasing, so to find minimum, we choose next point `x1` so that `x1 < x0`. If the `df(x0) < 0` then function is decreasing, and then incrementing x is the right direction: `x1 > x0`.

Looking at this, we can generalize that want go in the opposite direction of gradient. In fact this is exactly what the algorithm does even for function of multiple variables (then the gradient is a vector of all derivatives). 

## xNext

Now when we know what direction to look at, we have to choose how far we want to go in that direction. But since the at optimum the derivate is zero (function doesn't change) it's safe to assume that as it approaches the optimum it will become more flat. 

If so, we can then make our step proportional to the actual value of derivate (but in opposite direction!):

`x1 ~ x0 - df(x0)`

This is fine, however let's just add final element to this:

`x1 = x0 - alpha*df(x0)`, where `0 < alpha < 1`

Since we don't know how fast the function in flattening out near the optimum we'll need to adjust our step for different function so it's not to big.

## Implementation

Wrapping it up, you could write down the algorithm as follows:

    Matrix x = x0;
    var fx = f(x0);
    
Terminate the algorithm if runs for too many iterations:    
    
    for (var i = 0; i < MaxIterations; ++i)
    {
    
Calculate next step:    
    
        var x1 = (x - df(x) * alpha).Evaluate();
    
    
Terminate if value is not changed:    

        var fx1 = f(x1);
        if (fx - fx1 < Epsilon)
            return x1;
    
        x = x1;
        fx = fx1;
    }


## Using the code: