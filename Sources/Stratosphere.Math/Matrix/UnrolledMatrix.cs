using System;
using System.Linq;

namespace Stratosphere.Math
{
    public class UnrolledMatrix : Matrix
    {
        public Matrix[] Matrices { get; }

        public UnrolledMatrix(params Matrix[] matrices) : base(new[] { matrices.Select(m => ArrayExtension.Product((int[]) m.Size)).Sum(), 1 })
        {
            Matrices = matrices;
        }

        public override double Get(int index)
        {
            int unrolledIndex = 0;
            for (int m = 0; m < Matrices.Length; ++m)
            {
                var deltaIndex = Matrices[m].Size.Product();
                if (index < unrolledIndex + deltaIndex)
                    return Matrices[m].Get(index - unrolledIndex);

                unrolledIndex += deltaIndex;
            }

            throw new IndexOutOfRangeException();
        }

        public override double GetByCoordinates(int row, int column)
        {
            return Get(row);
        }

        public static UnrolledMatrix Parse(Matrix thetaParameters, params int[][] sizes)
        {
            var alreadyUnrolled = thetaParameters as UnrolledMatrix;
            if (alreadyUnrolled != null)
                return alreadyUnrolled;

            if (thetaParameters.Width != 1)
                throw new ArgumentException("Matrix must be of width 1", nameof(thetaParameters));

            var matrices = new Matrix[sizes.Length];

            int unrolledIndex = 0;
            for (int matrixIndex = 0; matrixIndex < sizes.Length; ++matrixIndex)
            {
                var data = new double[sizes[matrixIndex].Product()];
                for (int i = 0; unrolledIndex < thetaParameters.Height && i < data.Length; ++unrolledIndex, ++i)
                {
                    data[i] = thetaParameters[unrolledIndex];
                }

                matrices[matrixIndex] = new ColumnMajorMatrix(data, sizes[matrixIndex]);
            }

            return new UnrolledMatrix(matrices);
        }
    }
}