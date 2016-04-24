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

            Console.WriteLine($"Cost = {_Cost(result.As<One, k>(), Matrix.Vector(0, 1, 1, 0).As<k>().T)}");

            FeedforwardXor_Gradient(x, "0, 1, 1, 0");

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

            var z2 = theta1.T * a1.Prepend(1);
            Matrix<n, k> a2 = Sigmoid(z2);

            var z3 = theta2.T * a2.Prepend(1);
            Matrix<p, k> a3 = Sigmoid(z3);

            var z4 = theta3.T * a3.Prepend(1);
            Matrix<One, k> a4 = Sigmoid(z4);



            return a4.Inner;
        }

        private static Matrix FeedforwardXor_Gradient(Matrix x, Matrix y)
        {
            var theta1_1 = Matrix.Vector(-10, 20, 0);
            var theta1_2 = Matrix.Vector(30, -20, -20);
            var theta1_3 = Matrix.Vector(-10, 0, 20);

            var theta2_1 = Matrix.Vector(30, -20, -20, 0);
            var theta2_2 = Matrix.Vector(30, 0, -20, -20);

            var theta3_1 = Matrix.Vector(30, -20, -20);

            Matrix<m, n> theta1 = theta1_1.Concat(theta1_2).Concat(theta1_3).Evaluate().As<m, n>();
            Matrix<n, p> theta2 = theta2_1.Concat(theta2_2).Evaluate().As<n, p>();
            Matrix<p, One> theta3 = theta3_1.As<p, One>();

            var k = x.Width;
            Matrix<m, k> a1 = x.As<m, k>();

            Matrix<n, k> z2 = theta1.T * a1.Prepend(1);
            Matrix<n, k> a2 = Sigmoid(z2);

            Matrix<p, k> z3 = theta2.T * a2.Prepend(1);
            Matrix<p, k> a3 = Sigmoid(z3);

            Matrix<One, k> z4 = theta3.T * a3.Prepend(1);
            Matrix<One, k> a4 = Sigmoid(z4);

            Matrix<One, k> delta4 = a4 - y.As<One, k>();

            // Remove the bias units in thetas
            Matrix<p, k> delta3 = (theta3.RemoveFirstRow() * delta4).MultiplyEach(SigmoidDerivative(z3));
            Matrix<n, k> delta2 = (theta2.RemoveFirstRow() * delta3).MultiplyEach(SigmoidDerivative(z2));

            Matrix<m, n> dJ1 = Matrix.Zeros(theta1.RemoveFirstRow().Size).As<m, n>();
            Matrix<n, p> dJ2 = Matrix.Zeros(theta2.RemoveFirstRow().Size).As<n, p>();
            Matrix<p, One> dJ3 = Matrix.Zeros(theta3.RemoveFirstRow().Size).As<p, One>();
            for (var i = 0; i < k; ++i)
            {
                dJ1 += a1.GetColumn(i) * delta2.GetColumn(i).T;
                dJ2 += a2.GetColumn(i) * delta3.GetColumn(i).T;
                dJ3 += a3.GetColumn(i) * delta4.GetColumn(i).T;
            }

            return a4.Inner;
        }

        private static double _Cost(Matrix<One, k> networkOutput, Matrix<One, k> y)
        {
            int k = networkOutput.Width;

            var t1 = y.MultiplyEach(Log(networkOutput));
            var t2 = (1 - y).MultiplyEach(Log(1 - networkOutput));
            Matrix<One, k> temp = t1 + t2;

            return (temp.SumColumns().SumRows() * (-1.0 / k)).Inner;
        }

        private static double NonZero(double v) => v == 0 ? 0.0000000001 : v;

        public static Matrix SigmoidDerivative(Matrix m) => Sigmoid(m).MultiplyEach(1 - Sigmoid(m));

        public static Matrix<D1, D2> SigmoidDerivative<D1, D2>(Matrix<D1, D2> m) => SigmoidDerivative(m.Inner).As<D1, D2>();

        private static Matrix Log(Matrix m) => m.Map(v => System.Math.Log(NonZero(v)));

        private static Matrix<D1, D2> Log<D1, D2>(Matrix<D1, D2> m) => Log(m.Inner).As<D1, D2>();
    }
}