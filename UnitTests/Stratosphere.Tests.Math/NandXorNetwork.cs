﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Stratosphere.Math;
using Stratosphere.Math.Optimization;
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
            var traingData = Matrix.Vector(0, 0)
                .Concat(Matrix.Vector(0, 1))
                .Concat(Matrix.Vector(1, 0))
                .Concat(Matrix.Vector(1, 1));
            Matrix traingOutput = "0, 1, 1, 0";

            var X = traingData.Evaluate();
            var Y = traingOutput.Evaluate();
            
            var result = FeedforwardXor(X);

            Console.WriteLine($"Cost = {Cost(result.As<One, k>(), Y.As<One, k>())}");

            var learned = TraingNetwork(X, Y).Take(1).OrderBy(thetas => Cost(thetas, X, Y)).First();
            
            Console.WriteLine($"Learned Cost = {Cost(learned, X, Y)}");
            Console.WriteLine($"Learned parameters: {learned.ToString()}");

            MatrixAssert.AreEqual(Y, result);
        }

        public IEnumerable<Matrix> TraingNetwork(Matrix x, Matrix Y)
        {
            while (true)
            {
                yield return FindMinimum(x, Y);
            }
        }

        public static Matrix FindMinimum(Matrix X, Matrix Y)
        {
            var fminunc = new BacktrackingSteepestDescentMethod(maxIterations: 2000);

            return fminunc.Find(
                f: unrolledThetas => Cost(unrolledThetas, X, Y),
                df: unrolledThetas => Gradient(unrolledThetas, X, Y),
                initial: InitialThetas());
        }

        private static Matrix InitialThetas()
        {
            var r = new Random((int)DateTime.Now.Ticks);
            double epsilon = 2;
            var initialT1 = Matrix.Ones(3, 3).Map(v => v * (2 * epsilon * r.NextDouble() - 1 * epsilon)).Evaluate();
            var initialT2 = Matrix.Ones(4, 2).Map(v => v * (2 * epsilon * r.NextDouble() - 1 * epsilon)).Evaluate();
            var initialT3 = Matrix.Ones(3, 1).Map(v => v * (2 * epsilon * r.NextDouble() - 1 * epsilon)).Evaluate();

            return new UnrolledMatrix(initialT1, initialT2, initialT3);
        }

        private static double Cost(Matrix unrolledThetas, Matrix X, Matrix y)
        {
            var unrolledMatrix = UnrolledMatrix.Parse(unrolledThetas, new [] {3, 3}, new [] {4, 2}, new [] { 3, 1});

            var theta1 = unrolledMatrix.Matrices[0];
            var theta2 = unrolledMatrix.Matrices[1];
            var theta3 = unrolledMatrix.Matrices[2];

            Matrix<m, n> dJ1;
            Matrix<n, p> dJ2;
            Matrix<p, One> dJ3;

            var result = FeedforwardBackpropagate(X, y, theta1.As<m, n>(), theta2.As<n, p>(), theta3.As<p, One>(), out dJ1, out dJ2, out dJ3);
            return Cost(result, y.As<One, k>());
        }

        static Matrix Gradient(Matrix untolledThetas, Matrix X, Matrix y)
        {
            var unrolledMatrix = UnrolledMatrix.Parse(untolledThetas, new[] { 3, 3 }, new[] { 4, 2 }, new[] { 3, 1 });

            var theta1 = unrolledMatrix.Matrices[0];
            var theta2 = unrolledMatrix.Matrices[1];
            var theta3 = unrolledMatrix.Matrices[2];

            Matrix<m, n> dJ1;
            Matrix<n, p> dJ2;
            Matrix<p, One> dJ3;

            FeedforwardBackpropagate(X, y, theta1.As<m, n>(), theta2.As<n, p>(), theta3.As<p, One>(), out dJ1, out dJ2, out dJ3);

            return new UnrolledMatrix(dJ1, dJ2, dJ3);
        }

        private static double Cost(Matrix<One, k> networkOutput, Matrix<One, k> y)
        {
            int k = networkOutput.Width;

            var t1 = y.MultiplyEach(Log(networkOutput));
            var t2 = (1 - y).MultiplyEach(Log(1 - networkOutput));
            Matrix<One, k> temp = t1 + t2;

            return (temp.SumColumns().SumRows() * (-1.0 / k)).Inner;
        }

        private static Matrix FeedforwardXor(Matrix x)
        {
            //Matrix untolledThetas =
            //    "23.33; -166.59; -69.27;-6.95;97.16;-3.67;6.24;-19.96;49.36;-28.34;107.01;4.24;-47.46;19.31;-17.04;-18.19;-17.23;-7.44;-88.44;103.23;";
            //    var unrolledMatrix = UnrolledMatrix.Parse(untolledThetas, new[] { 3, 3 }, new[] { 4, 2 }, new[] { 3, 1 });

            //var theta1 = unrolledMatrix.Matrices[0].As<m, n>();
            //var theta2 = unrolledMatrix.Matrices[1].As<n, p>();
            //var theta3 = unrolledMatrix.Matrices[2].As<p, One>();


            var theta1_1 = Matrix.Vector(-10, 20, 0);
            var theta1_2 = Matrix.Vector(30, -20, -20);
            var theta1_3 = Matrix.Vector(-10, 0, 20);

            var theta2_1 = Matrix.Vector(30, -20, -20, 0);
            var theta2_2 = Matrix.Vector(30, 0, -20, -20);

            var theta3_1 = Matrix.Vector(30, -20, -20);

            Matrix<m, n> theta1 = theta1_1.Concat(theta1_2).Concat(theta1_3).Evaluate().As<m, n>();
            Matrix<n, p> theta2 = theta2_1.Concat(theta2_2).Evaluate().As<n, p>();
            Matrix<p, One> theta3 = theta3_1.As<p, One>();

            var a4 = Feedforward(x, theta1, theta2, theta3);

            return a4.Inner;
        }

        private static Matrix<One, k> Feedforward(Matrix x, Matrix<m, n> theta1, Matrix<n, p> theta2, Matrix<p, One> theta3)
        {
            Matrix<m, n> dJ1;
            Matrix<n, p> dJ2;
            Matrix<p, One> dJ3;
            return FeedforwardBackpropagate(x, Matrix.Ones(1, x.Width), theta1, theta2, theta3, out dJ1, out dJ2, out dJ3);
        }

        private static Matrix<One, k> FeedforwardBackpropagate(Matrix x, Matrix y, Matrix<m, n> theta1, Matrix<n, p> theta2, Matrix<p, One> theta3, out Matrix<m, n> dJ1,
            out Matrix<n, p> dJ2, out Matrix<p, One> dJ3)
        {
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

            dJ1 = Matrix.Zeros(theta1.Size).As<m, n>();
            dJ2 = Matrix.Zeros(theta2.Size).As<n, p>();
            dJ3 = Matrix.Zeros(theta3.Size).As<p, One>();

            for (var i = 0; i < k; ++i)
            {
                dJ1 += a1.Prepend(1).GetColumn(i) * delta2.GetColumn(i).T;
                dJ2 += a2.Prepend(1).GetColumn(i) * delta3.GetColumn(i).T;
                dJ3 += a3.Prepend(1).GetColumn(i) * delta4.GetColumn(i).T;
            }

            return a4;
        }

        private static double NonZero(double v) => v == 0 ? 0.0000000001 : v;

        public static Matrix SigmoidDerivative(Matrix m) => Sigmoid(m).MultiplyEach(1 - Sigmoid(m));

        public static Matrix<D1, D2> SigmoidDerivative<D1, D2>(Matrix<D1, D2> m) => SigmoidDerivative(m.Inner).As<D1, D2>();

        private static Matrix Log(Matrix m) => m.Map(v => System.Math.Log(NonZero(v)));

        private static Matrix<D1, D2> Log<D1, D2>(Matrix<D1, D2> m) => Log(m.Inner).As<D1, D2>();
    }
}