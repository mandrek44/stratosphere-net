# Introduction to Neural Networks - Part 2

In [previous post](<http://marcindrobik.pl/Post/IntroductiontoNeuralNetworksPart1>) we saw single neuron and learned how to represent and name it's components.

Today we'll see how to combine neurons together to form network!

## NAND for the start

[`NAND`](<https://en.wikipedia.org/wiki/NAND_gate>) operation is [*functionally complete*](<https://en.wikipedia.org/wiki/Functional_completeness>) which means that we can combine it into any logic function. Sounds like fun, so let's see if we can come up with neuron that represents it:

    f = S(30 - 20a - 20b)

![](https://mandrostorage.blob.core.windows.net/blogfiles/Nand.svg)


By looking at function values we can confirm it's indeed a `NAND` operation:

    f(0, 0) = S(30)  = ~1  
    f(0, 1) = S(10)  = ~1
    f(1, 0) = S(10)  = ~1
    f(1, 1) = S(-10) = ~0

## Composite operations

By combining `NAND` operations, we can now create a more complex [`XOR`](<https://en.wikipedia.org/wiki/NAND_logic#XOR>) gate:

![](https://mandrostorage.blob.core.windows.net/blogfiles/NandXor.svg)

I changed the notation a bit - instead of repeating the bias inputs for every neuron I just placed the bias weight inside the neuron circle. 

Notice the `XOR` is composed of 4 `NAND` neurons.

## Layers topology

The topology of `XOR` if very irregular. Neural networks should form a layers with each node in given layer connected to every node in next layer. 

Lets amend our `XOR` function to look like that:

![](https://mandrostorage.blob.core.windows.net/blogfiles/NandXorNetwork.svg)

Notice that I added 2 nodes in second layer. Those nodes are designed to pass one of the arguments. If the argument is 0, then it will become `S(-10) ~ 0`. If argument is 1 it will become `S(10) ~ 1`. Rest of the system is unchanged and should function exactly the same as previous one.

Now we clearly see that `XOR` has 4 layers. The first layer is called *input* layer. Last layer is called *output* layer. The layers in between (second and third in our example) are called *hidden* layers. 

## Defining Neural Network with Matrices

Matrices can be used to represent our neural network. First, there's a input matrix `x` (nodes `a` and `b` on the diagram):
  
    var x = Matrix.Vector(a, b);

For each neuron in second layer we define the weights of the inputs, taking into account additional bias input:

    var theta1_1 = Matrix.Vector(-10, 20, 0);
    var theta1_2 = Matrix.Vector(30, -20, -20);
    var theta1_3 = Matrix.Vector(-10, 0, 20);

You can take a moment and verify that above values correspond to the weights on `XOR` diagram. 
Together the weights form a 2-dimensional matrix that represents entire second layer:
    
    var theta1 = theta1_1.Concat(theta1_2).Concat(theta1_3).Evaluate().As<m, n>();
    
By convention those matrices are named with index of previous layer (`theta1` instead of `theta2` in this case) to indicate that they are used to calculate next layer activations.

Matrices for third and fourth layer are defined in the same way:   
    
    var theta2_1 = Matrix.Vector(30, -20, -20, 0);
    var theta2_2 = Matrix.Vector(30, 0, -20, -20);    
    var theta2 = theta2_1.Concat(theta2_2).Evaluate().As<n, p>();
    
    var theta3_1 = Matrix.Vector(30, -20, -20);
    var theta3 = theta3_1.As<p, One>();

Since the fourth layer is the output layer, there's no `theta4`.

## Feedforwarding      
    
Now, when we've defined neural network by its weights, we can derive the value of output layer by calculating individual activation of each neuron, layer by layer. This procedure is called *feedforwarding*. 

Using the matrices, we can calculate activation of entire layer at once. First layer activation is equal to the input values:  
  
    var a1 = x.As<m>();
    
To calculate the activation of second layer, we multiply the weight matrix by the activation of its previous layer. Resulting values are piped through Sigmoid function. Notice that inputs in each layer have additional bias value (`.Prepend(1)`):
    
    Matrix<n, One> a2 = Sigmoid(theta1.T * a1.Prepend(1));
    
We repeat the operation for each layer:

    Matrix<p, One> a3 = Sigmoid(theta2.T * a2.Prepend(1));
    Matrix<One, One> a4 = Sigmoid(theta3.T * a3.Prepend(1));
    
Last activation resulted in `Matrix<One, One>` which is what we expected (`XOR` has single output). It's helpful to round it to integer:    
    
    int result = a4.Inner > 0.5 ? 1 : 0;

## Testing

Several simple test cases reassure us that the network works correctly:

    [TestCase(0, 0, ExpectedResult = 0)]
    [TestCase(1, 1, ExpectedResult = 0)]
    [TestCase(0, 1, ExpectedResult = 1)]
    [TestCase(1, 0, ExpectedResult = 1)]

![](https://mandrostorage.blob.core.windows.net/blogfiles/devenv_2016-04-23_19-45-50.png)
