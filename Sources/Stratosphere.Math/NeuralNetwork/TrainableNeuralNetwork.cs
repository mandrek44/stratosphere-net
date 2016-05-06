using System;
using System.Linq;
using Stratosphere.Math.Optimization;
using Stratosphere.Math.Regression;

namespace Stratosphere.Math.NeuralNetwork
{
    public class TrainableNeuralNetwork
    {
        public TrainableNeuralNetwork(params int[] layerSizes)
        {
            Thetas = InitialThetas(layerSizes);
        }

        public Matrix[] Thetas { get; set; }

        public Matrix[] Gradients { get; private set; }

        public Matrix[] Activations { get; private set; }

        public double Cost { get; private set; }

        public void Train(Matrix X, Matrix Y)
        {
            var fminunc = new BacktrackingSteepestDescentMethod(maxIterations: 2000);

            var learnedThetas = fminunc.Find(
                f: unrolledThetas =>
                {
                    Thetas = UnrolledMatrix.Parse(unrolledThetas, Thetas.Select(t => t.Size).ToArray()).Matrices;
                    Run(X, Y);
                    return this.Cost;
                },
                df: unrolledThetas =>
                {
                    Thetas = UnrolledMatrix.Parse(unrolledThetas, Thetas.Select(t => t.Size).ToArray()).Matrices;
                    Run(X, Y);

                    return new UnrolledMatrix(Gradients);
                },
                initial: new UnrolledMatrix(Thetas));

            Thetas = UnrolledMatrix.Parse(learnedThetas, Thetas.Select(t => t.Size).ToArray()).Matrices;
        }

        public void Run(Matrix x)
        {
            var networkSize = Thetas.Length + 1;

            Activations = new Matrix[networkSize];
            Activations[0] = x;

            for (int layer = 1; layer < networkSize; ++layer)
            {
                var z = Thetas[layer - 1].T * Activations[layer - 1].Prepend(1);
                Activations[layer] = LogisticRegression.Sigmoid(z);
            }
        }

        private void Run(Matrix x, Matrix y)
        {
            var networkSize = Thetas.Length + 1;

            var zs = new Matrix[networkSize];
            Activations = new Matrix[networkSize];

            Activations[0] = x;
            for (int layer = 1; layer < networkSize; ++layer)
            {
                zs[layer] = Thetas[layer - 1].T * Activations[layer - 1].Prepend(1);
                Activations[layer] = LogisticRegression.Sigmoid(zs[layer]);
            }

            var deltas = new Matrix[networkSize];
            deltas[networkSize - 1] = Activations.Last() - y;

            for (int layer = networkSize - 2; layer > 0; --layer)
            {
                deltas[layer] = (Thetas[layer].RemoveFirstRow() * deltas[layer + 1]).MultiplyEach(SigmoidDerivative(zs[layer]));
            }

            Gradients = new Matrix[Thetas.Length];
            for (int layer = 0; layer < networkSize - 1; ++layer)
            {
                Gradients[layer] = Matrix.Zeros(Thetas[layer].Size);
            }

            for (int i = 0; i < x.Width; ++i)
            {
                for (int layer = 0; layer < networkSize - 1; ++layer)
                {
                    Gradients[layer] += Activations[layer].Prepend(1).GetColumn(i) * deltas[layer + 1].GetColumn(i).T;
                    Gradients[layer] = Gradients[layer].Evaluate();
                }
            }

            Cost = CalculateCost(Activations.Last(), y);
        }

        private static Matrix[] InitialThetas(params int[] layerSizes)
        {
            var thetas = new Matrix[layerSizes.Length - 1];
            var r = new Random((int)DateTime.Now.Ticks);
            double epsilon = 2;

            for (int layer = 0; layer < layerSizes.Length - 1; ++layer)
            {
                thetas[layer] = Matrix.Ones(layerSizes[layer] + 1, layerSizes[layer + 1])
                    .Map(v => v * (2 * epsilon * r.NextDouble() - 1 * epsilon))
                    .Evaluate();
            }

            return thetas;
        }

        private static double CalculateCost(Matrix networkOutput, Matrix y)
        {
            int k = networkOutput.Width;

            var t1 = y.MultiplyEach(Log(networkOutput));
            var t2 = (1 - y).MultiplyEach(Log(1 - networkOutput));

            return ((t1 + t2).Sum() * (-1.0 / k));
        }

        private static double NonZero(double v) => v == 0 ? 0.0000000001 : v;

        private static Matrix SigmoidDerivative(Matrix m) => LogisticRegression.Sigmoid(m).MultiplyEach(1 - LogisticRegression.Sigmoid(m));

        private static Matrix Log(Matrix m) => m.Map(v => System.Math.Log(NonZero(v)));

    }
}