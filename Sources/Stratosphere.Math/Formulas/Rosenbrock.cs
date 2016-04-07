namespace Stratosphere.Math.Formulas
{
    public class Rosenbrock
    {
        public static double Function(Matrix x)
        {
            var a = x[1] - (x[0] * x[0]);
            var b = 1 - x[0];

            return 100 * a * a + b * b;
        }

        public static Matrix Derivative(Matrix x)
        {
            // http://www.wolframalpha.com/input/?i=d%2Fdx(+100*((y+-+x%5E2)%5E2)+%2B+(1-x)%5E2)
            var dx = 2 * (200 * x[0] * x[0] * x[0] - 200 * x[0] * x[1] + x[0] - 1);

            // http://www.wolframalpha.com/input/?i=d%2Fdy(+100*((y+-+x%5E2)%5E2)+%2B+(1-x)%5E2)
            var dy = 200 * (x[1] - x[0] * x[0]);

            return Matrix.Vector(dx, dy);
        }

        public static Matrix SecondDerivative(Matrix x)
        {
            // http://www.wolframalpha.com/input/?i=d%2Fdx(+100*((y+-+x%5E2)%5E2)+%2B+(1-x)%5E2)
            var dxx = 1200 * x[0] * x[0] - 400 * x[1] + 2;
            var dxy = -400 * x[0];

            var dyx = -400 * x[0];
            var dyy = 200;

            return Matrix.Vector(dxx, dyx).Concat(Matrix.Vector(dxy, dyy));
        }
    }
}