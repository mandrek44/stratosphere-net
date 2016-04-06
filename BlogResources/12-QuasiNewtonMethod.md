# Quasi-Newton Method

We've already seen the [Newton's Method](<http://marcindrobik.pl/Post/OptimizingwithNewtonsMethod>) in action. If you recall, the Newton's Method requires computing inverse matrix of functions' second derivative.

This poses two problems: first, calculating the inverse matrix is computationally expensive (my implementation is hard-coded for 2x2 and 3x3 matrices). Second, calculating second derivative may be complex and also computationally expensive (generally we want call the optimized function - or its derivatives - as few times as possible).

## Broyden–Fletcher–Goldfarb–Shanno algorithm

The [Broyden–Fletcher–Goldfarb–Shanno (BFGS) algorithm](<https://en.wikipedia.org/wiki/Broyden%E2%80%93Fletcher%E2%80%93Goldfarb%E2%80%93Shanno_algorithm>) is used to approximate the second derivative of the function. Moreover, with small change it can be used to directly calculate the inversed second derivative making it computationally very efficient. 

## Implementation

The formula is bit complex, but can be easily - almost directly - implemented using our Matrix class:

    var dfx1 = df(x1);
    var q = df(x2) - dfx1;
    var s = x2 - x1;
    
    H = H + (s*s.T)/(q.T*s) - (H*q*q.T*H.T)/(q.T*H*q);

`H` is our inverted matrix of second derivatives (inverted [Hessian](<https://en.wikipedia.org/wiki/Hessian_matrix>)). Having it, we can use it in Newton's Method. Recall, that original Newton Method used second derivative to calculate the search direction and then uses line search to find minimum in that direction:

    var dfx = df(x1);
    var p = -ddf(x1).Inverse()*dfx;
    
    var x2 = BacktrackingLineSearch.Find(f, df, p, x1, dfx).Evaluate();

Now all we need to do is replace second derivative calculation (`ddf(x)`) with our inverted Hessian:

    var dfx = df(x1);
    var p = -H*dfx;
    
    var x2 = BacktrackingLineSearch.Find(f, df, p, x1, dfx).Evaluate();  

And that's the Quasi Newton Method.

## Initial value

The BFGS algorithm is recursive - it uses previous approximation to calculate next. So to run it, we need some initial value. If second derivative is not available, [Identity matrix](<https://en.wikipedia.org/wiki/Identity_matrix>) may be used instead.

## Result

![](<https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-04-06_20-22-39.png>)