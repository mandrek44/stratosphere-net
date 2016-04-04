# Optimizing optimization algorithms - Line Search

Both [Steepest Gradient Decent](<http://marcindrobik.pl/Post/NumericalOptimizationSteepestGradientDescent>) and [Newton's Method](<http://marcindrobik.pl/Post/OptimizingwithNewtonsMethod>) used parameter `alpha` to control the length of algorithm step. This would vary for each optimized function and for now we could only set it manually.

## Wolfie Conditions

We need some sort of mathematical constrains for the step length to be able to choose it automatically. [Wolfie Conditions](<https://en.wikipedia.org/wiki/Wolfe_conditions>) give us such constrains. The conditions are as follows:

Armijo rule:
![Armijo Rule](https://mandrostorage.blob.core.windows.net/blogfiles/ArmijoRule.jpg)

Curvature Condition:
![Curvature Condition](https://mandrostorage.blob.core.windows.net/blogfiles/CurvatureCondition.jpg)

where:

 - <img src="https://mandrostorage.blob.core.windows.net/blogfiles/p_k.png" align="center" border="0" alt="p_{k}" /> is  search direction (<img src="https://mandrostorage.blob.core.windows.net/blogfiles/minusNablaFx.png" align="center" border="0" alt="- \nabla f(x_{k})"/> for Steepest Descent).
 - <img src="https://mandrostorage.blob.core.windows.net/blogfiles/f.png" align="center" border="0" alt="f" /> is function to optimize.
 - <img src="https://mandrostorage.blob.core.windows.net/blogfiles/nablaF.png" align="center" border="0" alt="\nabla f"/> is function derivative.
 - <img src="https://mandrostorage.blob.core.windows.net/blogfiles/alpha.png" align="center" border="0" alt="\alpha" /> is step length.
 - <img src="https://mandrostorage.blob.core.windows.net/blogfiles/c1c2.png" align="center" border="0" alt="0 < c_{1} < c_{2} < 1" /> are two coefficients, usually `0.0001` and `0.9` respectively.


In essence, the Armijo rule is constraining the maximum step length and curvature condition is constraining the minimum step length. 

## Straight forward implementation

The algorithm starts with <img src="https://mandrostorage.blob.core.windows.net/blogfiles/alpha.png" align="center" border="0" alt="\alpha" /> set to `1` and then makes it smaller until it fulfills the Armijo rule:

    public static Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix p, Matrix x_start, Matrix dfx_start)
    {
        var x0 = x_start;
        var fx0 = f(x0);
    
... limit the number of iterations - after 16 divisions alpha will be so small that it probably doesn't make sense to continue:    
    
        double alpha = 1;
        int i = 0;                              
                
        while (!Armijo(f, x0, fx0, dfx_start, p, alpha) && i < 16)
        {
            alpha = K * alpha; // K == 0.5
            i++;
        }
    
.. and return next point in direction `p` using calculated `alpha`:    
    
        return x0 + alpha * p; 
    }
    
The Armijo function is direct implementation of the Armijo formula:

    private static bool Armijo(Func<Matrix, double> f, Matrix x0, double fx0, Matrix dfx0, Matrix p, double alpha)
    {
        return f(x0 + alpha * p) <= fx0 + C * alpha * (dfx0.T * p) + Epsilon;
    }
    
## Alpha-less Gradient Descent    
    
Let's put the algorithm to use. In Steepest Gradient Descent we had following line to calculate next point:

    var x2 = x - Alpha * df(x);
    
Instead, we can use algorithm we just implemented (let's call it `Backtracking Line Serch`):

    var dfx = df(x);
    var p = -dfx / dfx.Length; // Normalize the direction vector

    var x2 = BacktrackingLineSearch.Find(f, df, p, x, dfx);
    
 
 When run against Banana function, we get following result (2000 iterations):
 
 ![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-04-02_20-21-37.png)

(Notice that the Steepest Gradient Decent didn't reach the minimum).

## Alpha-less Newton's Method

Similarly, Newton's method defines step as: 

    var dfx = df(x);
    var x2 = x - Alpha * ddf(x).Inverse() * dfx;

With the `Backtracking Line Search` we can rewrite it as:

    var dfx = df(x);
    var p = -ddf(x).Inverse()*dfx;

    var x2 = BacktrackingLineSearch.Find(f, df, p, x, dfx).Evaluate();

This will give following result for Banana function:

![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-04-02_20-28-14.png)

Notice that the Newton method approximated the minimum much better and with fewer steps (< 30 iterations).