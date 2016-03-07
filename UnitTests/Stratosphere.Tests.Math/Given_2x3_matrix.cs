using NUnit.Framework;
using Stratosphere.Math;
using Stratosphere.Math.Matrix;

namespace Stratosphere.Tests.Math
{
    public class Given_2x3_matrix
    {
        private ColumnMajorMatrix _matrix;

        [SetUp]
        public void Given_test_matrix()
        {
            /* 0 2 4
               1 3 5  */
            _matrix = ColumnMajorMatrix.Parse("0 2 4;1 3 5");
        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(2, ExpectedResult = 2)]
        [TestCase(3, ExpectedResult = 3)]
        [TestCase(4, ExpectedResult = 4)]
        [TestCase(5, ExpectedResult = 5)]
        public double Then_can_be_accessed_by_column_index(int columnIndex)
        {
            return _matrix.GetByColumnIndex(columnIndex);
        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 2)]
        [TestCase(2, ExpectedResult = 4)]
        [TestCase(3, ExpectedResult = 1)]
        [TestCase(4, ExpectedResult = 3)]
        [TestCase(5, ExpectedResult = 5)]
        public double Then_can_be_accessed_by_row_index(int rowIndex)
        {
            return _matrix.GetByRowIndex(rowIndex);
        }

        [Test]
        public void Then_can_be_multiplied_by_scalar()
        {
            Assert.AreEqual((ColumnMajorMatrix)"0 4 8;2 6 10", 2 * _matrix);
        }

        [Test]
        public void Then_can_be_transposed()
        {
            Assert.AreEqual((ColumnMajorMatrix)"0 1;2 3;4 5", _matrix.Transpose());
        }

        [Test]
        public void Then_can_be_enumerated_by_columns()
        {
            CollectionAssert.AreEqual(new double[] {0, 1, 2, 3, 4, 5}, _matrix.EnumerateByColumns());
        }

        [Test]
        public void Then_can_be_enumerated_by_rows()
        {
            CollectionAssert.AreEqual(new double[] { 0, 2, 4, 1, 3, 5}, _matrix.EnumerateByRows());
        }

        [Test]
        public void Then_can_be_multiplied_by_other_matrix()
        {
            Assert.AreEqual((ColumnMajorMatrix)"20 26;26 35", _matrix.Multiply((ColumnMajorMatrix)"0 1;2 3;4 5"));
        }

        [Test]
        public void Then_can_be_multiplied_by_other_matrix_2()
        {
            Assert.AreEqual((ColumnMajorMatrix)"20;26", _matrix.Multiply((ColumnMajorMatrix)"0;2;4"));
        }

        [Test]
        public void Then_can_be_substracted_by_other_matrix()
        {
            Assert.AreEqual((ColumnMajorMatrix)"0 -2 -4;-1 -3 -5", _matrix.Substract((ColumnMajorMatrix)"0 4 8;2 6 10"));
        }

        [Test]
        public void Then_column_can_be_retrieved()
        {
            Assert.AreEqual((ColumnMajorMatrix)"2;3", _matrix.GetColumn(1));
        }

        [Test]
        public void Then_column_can_be_added()
        {
            Assert.AreEqual(ColumnMajorMatrix.Parse("0 2 4 6;1 3 5 7"), new ColumnsConcatMatrix(_matrix, ColumnMajorMatrix.Parse("6;7")));
        }
    }
}