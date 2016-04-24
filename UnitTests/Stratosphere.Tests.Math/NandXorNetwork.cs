using System;
using System.Linq;
using NUnit.Framework;
using Stratosphere.Math;
using Stratosphere.Math.Regression;
using Stratosphere.Math.StronglyTypedMatrix;
using static System.Math;
using static Stratosphere.Math.Regression.LogisticRegression;

namespace Stratosphere.Tests.Math
{
    public class NandXorNetwork
    {
        [TestCase(0, 0, ExpectedResult = 0)]
        [TestCase(1, 1, ExpectedResult = 0)]
        [TestCase(0, 1, ExpectedResult = 1)]
        [TestCase(1, 0, ExpectedResult = 1)]
        public int XOR(params double[] input)
        {
            var x = Matrix.Vector(input).As<m>();

            return FeedforwardXor(x) > 0.5 ? 1 : 0;
        }

        [Test]
        public void XOR_EntireDataset()
        {
            var x = Matrix.Vector(0, 0)
                .Concat(Matrix.Vector(0, 1))
                .Concat(Matrix.Vector(1, 0))
                .Concat(Matrix.Vector(1, 1));

            var result = FeedforwardXor(x);

            MatrixAssert.AreEqual("0, 1, 1, 0", result);
        }

        private static Matrix FeedforwardXor(Matrix x)
        {
            var theta1_1 = Matrix.Vector(-10, 20, 0);
            var theta1_2 = Matrix.Vector(30, -20, -20);
            var theta1_3 = Matrix.Vector(-10, 0, 20);

            var theta2_1 = Matrix.Vector(30, -20, -20, 0);
            var theta2_2 = Matrix.Vector(30, 0, -20, -20);

            var theta3_1 = Matrix.Vector(30, -20, -20);

            var theta1 = theta1_1.Concat(theta1_2).Concat(theta1_3).Evaluate().As<m, n>();
            var theta2 = theta2_1.Concat(theta2_2).Evaluate().As<n, p>();
            var theta3 = theta3_1.As<p, One>();

            var a1 = x.As<m, k>();

            Matrix<n, k> a2 = Sigmoid(theta1.T * a1.Prepend(1));
            Matrix<p, k> a3 = Sigmoid(theta2.T * a2.Prepend(1));
            Matrix<One, k> a4 = Sigmoid(theta3.T * a3.Prepend(1));

            return a4.Inner;
        }

        //private static double _Cost(Matrix<m, k> X, Matrix<One, k> y, Func<Matrix<m, k>, Matrix<One, k>> neuralNetwork)
        //{
        //    Matrix<One, k> networkOutput = neuralNetwork(X);


        //    Matrix<One, k> t1 = Log(networkOutput) + Log(1 - networkOutput);

        //    t1.Inner.Sum(0);
        //    return 0;

        //    //for (int k = 0; k < networkOutput.Height; ++k)
        //    //{
        //    //    var h = networkOutput[k];

        //    //    return (
        //    //        -y.MultiplyEach(h.Map(v => Log(NonZero(v))))
        //    //        - (1 - y).MultiplyEach((1 - h).Map(v => Log(NonZero(v))))).Sum() / m;
        //    //}
        //}

        private static double NonZero(double v)
        {
            return v == 0 ? 0.00000001 : v;
        }

        private static Matrix Log(Matrix m) => m.Map(v => System.Math.Log(v));

        private static Matrix<D1, D2> Log<D1, D2>(Matrix<D1, D2> m) => Log(m.Inner).As<D1, D2>();
    }
}