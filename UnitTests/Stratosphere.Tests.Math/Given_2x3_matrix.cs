using NUnit.Framework;
using Stratosphere.Math;

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
            _matrix = new ColumnMajorMatrix(new double [] { 0, 1, 2, 3, 4, 5 }, new []{2, 3});
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
            Assert.AreEqual(new ColumnMajorMatrix(new double [] {0, 2, 4, 6, 8, 10}, new [] {2, 3}), _matrix.Multiply(2));
        }
    }
}