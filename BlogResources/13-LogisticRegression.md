# Logistic Regression

Using the Matrix class and optimization algorithms (like Quasi-Newton Method) we're now ready to build another Machine Learning tool - the Logistic Regression.

## The Border
Let's say we have a data set like the following:

![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-04-07_18-02-17.png)


Notice that there are two subsets - the green diamonds and orange circles (i.e. rock planets and gas planets).
With logistic regression, we can find such line that divides the two subsets in best possible way:

![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-04-07_18-01-56.png)

## Optimization goal

To work with the math we need some notations: 

 - `X` - training example
 - `y` - will hold `1` if corresponding example is our search goal (i.e. "Is that planet a gas planet?"). It will hold `0` otherwise.
 - `sigma(t)` - logistic function (we'll define it later)

 