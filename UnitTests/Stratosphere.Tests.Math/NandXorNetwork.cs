using System;
using System.Linq;
using NUnit.Framework;
using Stratosphere.Math;
using Stratosphere.Math.Regression;
using Stratosphere.Math.StronglyTypedMatrix;
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

            var theta1_1 = Matrix.Vector(-10, 20, 0);
            var theta1_2 = Matrix.Vector(30, -20, -20);
            var theta1_3 = Matrix.Vector(-10, 0, 20);

            var theta2_1 = Matrix.Vector(30, -20, -20, 0);
            var theta2_2 = Matrix.Vector(30, 0, -20, -20);

            var theta3_1 = Matrix.Vector(30, -20, -20);

            var theta1 = theta1_1.Concat(theta1_2).Concat(theta1_3).Evaluate().As<m, n>();
            var theta2 = theta2_1.Concat(theta2_2).Evaluate().As<n, p>();
            var theta3 = theta3_1.As<p, One>();

            var a1 = x;

            Matrix<n, One> a2 = Sigmoid(theta1.T * a1.Prepend(1));
            Matrix<p, One> a3 = Sigmoid(theta2.T * a2.Prepend(1));
            Matrix<One, One> a4 = Sigmoid(theta3.T * a3.Prepend(1));

            int result = a4.Inner > 0.5 ? 1 : 0;

            return result;
        }
    }

    public static class MatrixExtensions
    {
        public static Matrix<D, One> Prepend<D>(this Matrix<D, One> z2, params double[] values)
        {
            return Matrix.Ones(1).Concat(z2.T.Inner).T.Evaluate().As<D, One>();
        }
    }
}