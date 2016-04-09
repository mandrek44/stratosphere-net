using NUnit.Framework;

namespace Stratosphere.Tests.Math
{
    public class Given_single_element_matrix : Given_transformed_matrix
    {
        [SetUp]
        public void Given_test_matrix()
        {
            _matrix = "2";
            _expectedMatrix = "2";
        }

        [Test]
        public void Then_can_be_converted_to_number()
        {
            Assert.AreEqual(2.0, (double)_matrix);
        }
    }
}