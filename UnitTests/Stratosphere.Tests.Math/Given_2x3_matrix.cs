using System.Linq;
using NUnit.Framework;
using Stratosphere.Math;
using Stratosphere.Math.Regression;

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
            return _matrix.Get(columnIndex);
        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 2)]
        [TestCase(2, ExpectedResult = 4)]
        [TestCase(3, ExpectedResult = 1)]
        [TestCase(4, ExpectedResult = 3)]
        [TestCase(5, ExpectedResult = 5)]
        public double Then_can_be_accessed_by_row_index(int rowIndex)
        {
            return _matrix.EnumerateByRows().Skip(rowIndex).First();
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
            Assert.AreEqual((ColumnMajorMatrix)"20 26;26 35", _matrix * "0 1;2 3;4 5");
        }

        [Test]
        public void Then_can_be_multiplied_by_other_matrix_2()
        {
            Assert.AreEqual((ColumnMajorMatrix)"20;26", _matrix.Multiply("0;2;4"));
        }
        
        [Test]
        public void Then_max_per_column_can_be_calculated()
        {
            Assert.AreEqual((ColumnMajorMatrix)"1 3 5", _matrix.Max(0));
        }

        [Test]
        public void Then_max_per_row_can_be_calculated()
        {
            Assert.AreEqual((ColumnMajorMatrix)"4;5", _matrix.Max(1));
        }

        [Test]
        public void Then_min_per_column_can_be_calculated()
        {
            Assert.AreEqual((ColumnMajorMatrix)"0 2 4", _matrix.Min(0));
        }

        [Test]
        public void Then_min_per_row_can_be_calculated()
        {
            Assert.AreEqual((ColumnMajorMatrix)"0;1", _matrix.Min(1));
        }

        [Test]
        public void Then_length_can_be_calculated()
        {
            Assert.AreEqual(1, _matrix.GetColumn(0).Length, 0.001);
        }
    }
}