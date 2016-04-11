# Logistic Regression

Using the Matrix class and optimization algorithms (like Quasi-Newton Method) we're now ready to build another Machine Learning tool - the Logistic Regression.

## The Border
Let's say we have a data set like the following:

![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-04-07_18-02-17.png)


Notice that there are two subsets - the green diamonds and orange circles (i.e. rock planets and gas planets).
With logistic regression, we can find such line that divides the two subsets in best possible way:

![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-04-07_18-01-56.png)

## Optimization goal

Similar to Linear Regression, we need to calculate how well our solution fits the data.

To work with the math we need some notations: 

 - `X` - training example
 - `y` - will hold `1` if corresponding example is our search goal (i.e. "Is that planet a gas planet?"). It will hold `0` otherwise.
 - `m` - size of training set (number of training examples).
 - ![`theta`](https://mandrostorage.blob.core.windows.net/blogfiles/theta.jpg) - parameters for logistic regression, similar to linear regression parameters.
 - ![`sigma(t)`](https://mandrostorage.blob.core.windows.net/blogfiles/h_theta_x.jpg) - logistic function (we'll define it later).

  To find logistic regression parameters ![`theta`](https://mandrostorage.blob.core.windows.net/blogfiles/theta.jpg), we will optimize following cost function:
 
 ![Logistic regression cost](https://mandrostorage.blob.core.windows.net/blogfiles/logisticregression_cost.jpg)
 
 As you remember, optimization method uses function derivative, so need to define it as well:
 
 ![](https://mandrostorage.blob.core.windows.net/blogfiles/logisticregression_gradient.jpg)

Last thing we need is the logistic function ![`sigma(t)`](https://mandrostorage.blob.core.windows.net/blogfiles/h_theta_x.jpg):

![](https://mandrostorage.blob.core.windows.net/blogfiles/logisticregression_logisticfunction.jpg)


## Stratosphere.NET in action
 
I'm using C#'s `using static` to import `Math` and `Matrix` namespaces:

    using static System.Math;
    using static Stratosphere.Math.Matrix;

Using `Matrix` class we can write down the vectorized formulas. Lets start with cost function (`J`):

    double Cost(Matrix X, Matrix y, Matrix theta)
    {
        var m = X.Height;
        var h = Sigmoid(theta.T * X.T).T;
                
        return (
            -y.MultiplyEach(h.Map(v => Log(NonZero(v)))) 
            -(1 - y).MultiplyEach((1 - h).Map(v => Log(NonZero(v))))).Sum() / m;
    }
    
Vectorized gradient function:

    Matrix Gradient(Matrix X, Matrix y, Matrix theta)
    {
        var m = X.Height;
        var h = Sigmoid(theta.T * X.T).T;
    
        var temp = (Ones(theta.Height, 1) * (h - y).T).T;
        var gradient = temp.MultiplyEach(X).Sum(0) / m;
    
        return gradient.T;
    }
    
And last but not the least - logistic function (also called sigmoid function):

    public static double Sigmoid(double x) => 1.0 / (1.0 + Exp(-x));

    public static Matrix Sigmoid(Matrix x) => x.Map(Sigmoid);


## Running Optimization

Having all formulas implemented, we're now ready to pass them into our optimization algorithm:

    public static Matrix LogisticRegression(Matrix X, Matrix y)
    {
    
Prepend the data with ones - this way the found model parameters will have constant term:

        X = Matrix.Ones(X.Height, 1)
            .Concat(X)
            .Evaluate();

Pass the functions to Quasi-Newton Method that will found the optimal regression model:
    
        return QuasiNewtonMethod.Find(
            f: theta => _Cost(X, y, theta),
            df: theta => Gradient(X, y, theta),
            x0: Matrix.Zeros(X.Width, 1),
            maxIterations: 1000);
    }
    