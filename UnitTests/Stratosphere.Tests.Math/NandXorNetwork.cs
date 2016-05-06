using System;
using System.Linq;
using NUnit.Framework;
using Stratosphere.Math;
using Stratosphere.Math.NeuralNetwork;
using static Stratosphere.Math.Matrix;

namespace Stratosphere.Tests.Math
{
    public class NandXorNetwork
    {
        [TestCase(0, 0, ExpectedResult = 0)]
        [TestCase(1, 1, ExpectedResult = 0)]
        [TestCase(0, 1, ExpectedResult = 1)]
        [TestCase(1, 0, ExpectedResult = 1)]
        public int Test_XOR_Network(params double[] input)
        {
            var x = Vector(input);

            var theta_1 = Vector(-10, 20, 0)
                .Concat(Vector(30, -20, -20))
                .Concat(Vector(-10, 0, 20));

            var theta_2 = Vector(30, -20, -20, 0)
                .Concat(Vector(30, 0, -20, -20));

            var theta_3 = Vector(30, -20, -20);

            var xorNetwork = new TrainableNeuralNetwork(2, 3, 2, 1) { Thetas = new[] { theta_1, theta_2, theta_3 } };

            xorNetwork.Run(x);

            return xorNetwork.Activations.Last() > 0.5 ? 1 : 0;
        }

        [Test]
        public void XOR_TrainableNetwork()
        {
            var traingData = Vector(0, 0)
                .Concat(Vector(0, 1))
                .Concat(Vector(1, 0))
                .Concat(Vector(1, 1));
            Matrix traingOutput = "0, 1, 1, 0";

            var X = traingData.Evaluate();
            var Y = traingOutput.Evaluate();

            var xorNetwork = new TrainableNeuralNetwork(2, 3, 2, 1);
            xorNetwork.Train(X, Y);

            Console.WriteLine($"Learned Cost = {xorNetwork.Cost}");
            Console.WriteLine($"Learned parameters: {new UnrolledMatrix(xorNetwork.Thetas)}");

            MatrixAssert.AreEqual(Y, xorNetwork.Activations.Last());
        }
    }
}