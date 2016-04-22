## Introduction to Neural Networks - Part 1 

Before we start talking about neural networks, let talk about specific implementation of simple logic functions. Knowing how they work and how we can represent them will help us understanding the neural networks.

## Sigmoid

When we implemented the Logistic Regression we used sigmoid function function. Just to remind, the function is defined as:

![](https://mandrostorage.blob.core.windows.net/blogfiles/SigmoidFunction.jpg)

... and looks like that:

![](https://mandrostorage.blob.core.windows.net/blogfiles/Stratosphere.MachineLearning.Studio_2016-04-22_21-05-41.png)

The important properties for us is that for large values of `t` the `S(t)` approaches `1` and for small (negative) values of `t` the `S(t)` approaches `0`.


## And

Logic "and" is a function of two parameters - `a` and `b`. For use in arithmetic we'll assume that values close to `0` represents logical `false`, and values close to `1` represent logical `true`.

If so, we can come up with following function:

    f = S(-30 + 20a + 20b)
    
When looking at the various values of `a` and `b` we see that it indeed implements the logical And:    
    
| `a` | `b` | `f`    |
|:---:|:---:|:-------:|
| 0 |  0 | S(-30) ~ 0 |
| 1 |  0 | S(-10) ~ 0 |
| 0 |  1 | S(-10) ~ 0 |
| 1 |  1 | S(10) ~ 1  |


## Neuron representation

Our function `f` can represented as a single neuron if following way:

![](https://mandrostorage.blob.core.windows.net/blogfiles/NN_And.png)

In neural networks terminology, the `a` and `b` are *Inputs*, the `1` is special input called *Bias Input*, the orange circle is a *Neuron* (with activation function `S(t)`) and the arrows represent *Weights* of the inputs for particular *Neuron*. The arrow on the right represents *Output* of the neuron - in this case, our function `f = S(-30 + 20a + 20b)`

## Matrix representation

Negation

10 -20

f = g(10 - 20)

Or 

10 20 20

f = g(-10 + 20x1 + 20x2)

Sigmoid function




