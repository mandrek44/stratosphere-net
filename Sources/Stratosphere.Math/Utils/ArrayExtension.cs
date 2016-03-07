namespace Stratosphere.Math.Matrix
{
    public static class ArrayExtension
    {
        public static T[] Copy<T>(this T[] source)
        {
            var result = new T[source.Length];
            source.CopyTo(result, 0);

            return result;
        }

        public static bool EqualTo(this double[] source, double[] other, double delta = 0.0001)
        {
            if (source.Length != other.Length)
                return false;

            for (int i = 0; i < source.Length; i++)
            {
                if (System.Math.Abs(source[i] - other[i]) > delta)
                    return false;
            }

            return true;
        }

        public static double Product(this double[] source)
        {
            double result = 1;
            for (int i = 0; i < source.Length; i++)
            {
                result *= source[i];
            }

            return result;
        }

        public static int Product(this int[] source)
        {
            int result = 1;
            for (int i = 0; i < source.Length; i++)
            {
                result *= source[i];
            }

            return result;
        }
    }
}