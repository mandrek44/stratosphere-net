
In [previous post](<http://marcindrobik.pl/Post/NumericalOptimizationSteepestGradientDescent>) we designed some basic optimization algorithm - the Steepest Gradient Decent. Let's put it to use by implementing [linear regression](<https://en.wikipedia.org/wiki/Linear_regression>) algorithm.

## What's linear regression?

Linear regression works by finding linear function that fits given data set with smallest error.

For example, for given data:
![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-03-20_11-16-03.png)

Linear regression could find a line like this:

![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-03-20_11-17-38.png)

Linear Regression, can be used to model relationship between various features (height vs weight, distance vs time, [star wars planet size vs rotational period](<https://swapi.co/>) etc) and use this model to predict future values.

Linear regression is an example of [supervised learning](<https://en.wikipedia.org/wiki/Supervised_learning>) algorithm - we need a training dataset that will have examples with corresponding "right" answers.

## Optimizing line parameters

You may remember from school that line is often given by formula `y = ax + b`. Linear regression is an optimization problem for parameters `a` and `b`. We look for such `a` and `b` that gives best matching line.

How to define how line matches to our data set? The value of line function (`y`) for item from data set should be as close to the dataset value as possible, ideally pass through it. 

Having some sample data `(x_i, y_i)`, we can calculate the value of linear regression model for `x_i`: `y_regression = a*x_i + b`. Then, we calculate the error: `error_i = (y_regression - y_i)^2`. It's squared so it's always positive. 

To calculate the error for entire data set, we simply calculate the mean of errors for all items: 

![](https://mandrostorage.blob.core.windows.net/blogfiles/LinearRegression-SimpleError.png)

## Vectorizing line equation

Line equation `y = ax + b` can be written as multiplication of two Vectors (1D matrices):

![](https://mandrostorage.blob.core.windows.net/blogfiles/LinearRegression-VectorizedLine.png)

If we now run our optimization algorithm for error function, we'll obtain a matrix `|b a|` that will contain our linear regression model. 

The naming convention is such that the first matrix is called `theta` and second is called `X`. In this case the formula becomes: `theta * X`

There's just one more thing we need to run the optimization: the derivative. For linear regression error function, the [derivative is given by](<http://math.stackexchange.com/a/189792>):

![](https://mandrostorage.blob.core.windows.net/blogfiles/LinearRegression-SimpleDerivative.png)

## Stratosphere is action

Time for implementation! First we need the error function (also called "Cost" function):

    private static double ComputeCost(Matrix X, Matrix y, Matrix theta)
    {
        var m = y.Height;
        var error = X * theta - y;
        return error.Map(v => v * v).Sum() / (2.0 * m);
    }

Time for gradient function (vectorized version):

    private static Matrix Gradient(Matrix X, Matrix y, Matrix theta)
    {
        var error = (X * theta - y);
        var a = (Matrix.Ones(theta.Height, 1) * error.Transpose()).Transpose().MultiplyEach(X);
        return a.Sum(0).Transpose();
    }

Ready for some linear regression?

    private static Matrix LinearRegression(Matrix dataset, Matrix y)
    {
        var X = Matrix.Ones(dataset.Height, 1).Concat(dataset).Evaluate();
        return SimpleSteepestDescentMethod.Find(
            f: theta => ComputeCost(X, y, theta),
            df: theta => Gradient(X, y, theta),
            x0: Matrix.Ones(X.Width, 1),
            alpha: 0.0001,
            maxIterations: 5000);
    }

And voilà!

![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-03-20_12-25-29.png)
