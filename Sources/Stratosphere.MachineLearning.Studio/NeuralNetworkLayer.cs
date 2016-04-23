using Stratosphere.Math;

namespace Stratosphere.MachineLearning.Studio
{
    public class NeuralNetworkLayer
    {
        public Matrix Theta;

        public int Size => Theta.Height;
    }
}