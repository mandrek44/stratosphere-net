using System;

namespace Stratosphere.Math.Optimization
{
    public class LineSearch : LineSearchAlgorithm
    {
        private const double C = 0.0001;
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
                if (System.Math.Abs(alphai - alphaBefore) < 0.00001)
                    return alphai;
                

                var phiAlpha = phi(alphai);

                if (phiAlpha > phi(0) + C * alphai * dphi(0))
                {
                    return Zoom(alphaBefore, alphai);
                }

                var dphiAlpha = dphi(alphai);
                if (System.Math.Abs(dphiAlpha) <= -C2 * dphi(0))
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
            return alphaMin + (alphaMax - alphaMin) / 8.0;
        }

        private double Zoom(double alphaLow, double alphaHigh)
        {
            while (true)
            {
                double alphaj = alphaLow + System.Math.Abs((alphaHigh - alphaLow)) / 2;
                var phiAlphaj = phi(alphaj);

                if (phiAlphaj > phi(0) + C * alphaj * dphi(0) || phiAlphaj >= phi(alphaLow))
                {
                    alphaHigh = alphaj;
                }
                else
                {
                    var dphiAlphaJ = dphi(alphaj);
                    if (System.Math.Abs(dphiAlphaJ) <= -C2 * dphi(0))
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
}