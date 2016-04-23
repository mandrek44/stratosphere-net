## Introduction to Neural Networks - Part 1 

Before we start talking about neural networks, let talk about specific implementation of simple logic functions. Using them we learn basic math and graphical representations of Neural Networks.

## Sigmoid

When we implemented the [Logistic Regression](<http://marcindrobik.pl/Post/LogisticRegression>) we used the [sigmoid function](<https://en.wikipedia.org/wiki/Sigmoid_function>). This function is defined as:

![](https://mandrostorage.blob.core.windows.net/blogfiles/SigmoidFunction.jpg)

... and looks like that:

![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-04-22_21-05-41.png)

The important properties for us is that for large values of `t` the `S(t)` approaches `1` and for small (negative) values of `t` the `S(t)` approaches `0`.


## And

Logic "and" is a function of two parameters - `a` and `b`. For use in arithmetic we'll assume that values close to `0` represents logical `false`, and values close to `1` represent logical `true`.

Let's look at following function (`S` is sigmoid):

    f = S(-30 + 20a + 20b)
    
When looking at the various values of `a` and `b` we see that it indeed implements the logical And - it only outputs value close to `1` when both `a` and `b` are `1`:    
    
| `a` | `b` | `f`    |
|:---:|:---:|:-------:|
| 0 |  0 | S(-30) ~ 0 |
| 1 |  0 | S(-10) ~ 0 |
| 0 |  1 | S(-10) ~ 0 |
| 1 |  1 | S(10) ~ 1  |


## Neuron representation

Our function `f` can be represented as a single neuron on following graph:

![](https://mandrostorage.blob.core.windows.net/blogfiles/NN_And.png)

In neural networks terminology, the `a` and `b` are *Inputs*, the `1` is special input called *Bias Input*, the orange circle is a *Neuron* (with activation function `S(t)`) and the arrows in between represent *Weights* of the inputs for particular *Neuron*. The arrow on the right represents *Output* of the neuron - in this case, our function `f = S(-30 + 20a + 20b)`

## Vectorized version

It's very easy to represent this single neuron using matrix operations. 
First let's define our inputs as vector `x`:

    var x = Matrix.Vector(1, 0, 1); // a = 0; b = 1
    
... and the weights as vector `theta`:    
    
    var theta = Matrix.Vector(-30, 20, 20);
    
Now the neuron output is simply the multiplication:

    var f = Sigmoid(x*theta.T);

In next post we'll see how to create simple neural networks.