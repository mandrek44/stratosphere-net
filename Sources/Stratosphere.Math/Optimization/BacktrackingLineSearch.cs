using System;
using static System.Math;

namespace Stratosphere.Math.Optimization
{
    public class LineSearch : LineSearchAlgorithm
    {
        private const double C = 0.001;
        private const double C2 = 0.9;

        public Func<Matrix, double> f { get; private set; }
        public Func<Matrix, Matrix> df { get; private set; }
        public Matrix x_start { get; private set; }
        public Matrix p { get; private set; }

        public Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix p, Matrix x_start, Matrix dfx_start)
        {
            this.f = f;
            this.df = df;
            this.p = p;
            this.x_start = x_start;

            return Find();
        }

        private double phi(double alpha) => f(x_start + alpha * p);
        private Matrix dphi(double alpha) => df(x_start + alpha * p).T * p;

        public Matrix Find()
        {
            return x_start + FindAlpha() * p;
        }

        private double FindAlpha()
        {
            double alpha0 = 0;
            double alpha1 = 0.5;
            double alphaMax = 1;

            double alphai = alpha1;
            double alphaBefore = alpha0;

            while (true)
            {
                var phiAlpha = phi(alphai);

                if (phiAlpha > phi(0) + C * alphai * dphi(0))
                {
                    return Zoom(alphaBefore, alphai);
                }

                var dphiAlpha = dphi(alphai);
                if (Abs(dphiAlpha) <= -C2 * dphi(0))
                {
                    return alphai;
                }

                if (dphiAlpha >= 0)
                {
                    return Zoom(alphai, alphaBefore);
                }

                alphaBefore = alphai;
                alphai = ChooseAlpha(alphai, alphaMax);
            }
        }

        private double ChooseAlpha(double alphaMin, double alphaMax)
        {
            return (alphaMax - alphaMin) / 2.0;
        }

        private double Zoom(double alphaLow, double alphaHigh)
        {
            while (true)
            {
                double alphaj = (alphaHigh - alphaLow) / 2;
                var phiAlphaj = phi(alphaj);

                if (phiAlphaj > phi(0) + C * alphaj * dphi(0) || phiAlphaj >= phi(alphaLow))
                {
                    alphaHigh = alphaj;
                }
                else
                {
                    var dphiAlphaJ = dphi(alphaj);
                    if (Abs(dphiAlphaJ) <= -C2 * dphi(0))
                    {
                        return alphaj;
                    }
                    if (dphiAlphaJ * (alphaHigh - alphaLow) >= 0)
                    {
                        alphaHigh = alphaLow;
                    }

                    alphaLow = alphaj;
                }
            }
        }
    }

    public interface LineSearchAlgorithm
    {
        Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix p, Matrix x_start, Matrix dfx_start);
    }

    public class BacktrackingLineSearch : LineSearchAlgorithm
    {
        private const double K = 0.5;
        private const double C = 0.001;
        private const double C2 = 0.9;
        private const double Epsilon = 0;

        /// <summary>
        /// Finds inexact local minimum at direction p.
        /// </summary>
        /// <param name="f">Function to minimize.</param>
        /// <param name="df"></param>
        /// <param name="p">Direction vector along which to minimize the function f.</param>
        /// <param name="x_start">Starting point.</param>
        /// <param name="dfx_start">Gradient value at point x_start.</param>
        /// <returns>Value of x for which f(x) is close to local minimum.</returns>
        public Matrix Find(Func<Matrix, double> f, Func<Matrix, Matrix> df, Matrix p, Matrix x_start, Matrix dfx_start)
        {
            var x0 = x_start;
            var fx0 = f(x0);

            double alpha = 1;
            int i = 0;

            // Simple implementation of Wolfe conditions
            // TODO: Implement algorithm from page 60 in "Numerical Optimization", J. Nocedal, S.J. Wright
            while (
                !Armijo(f, x0, fx0, dfx_start, p, alpha)
                && Curvature(df, p, dfx_start, x0, alpha)
                && i < 16)
            {
                alpha = K * alpha;
                i++;
            }

            return x0 + alpha * p;
        }

        private static bool Armijo(Func<Matrix, double> f, Matrix x0, double fx0, Matrix dfx0, Matrix p, double alpha)
        {
            return f(x0 + alpha * p) <= fx0 + C * alpha * (dfx0.T * p) + Epsilon;
        }

        private static bool Curvature(Func<Matrix, Matrix> df, Matrix p, Matrix dfx_start, Matrix x0, double alpha)
        {
            return df(x0 + alpha * p).T * p >= C2 * dfx_start.T * p;
        }

    }
}