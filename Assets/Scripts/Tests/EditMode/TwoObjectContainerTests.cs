using NUnit.Framework;
using Utils; // Ensure this namespace includes TwoObjectContainer

namespace Tests.EditMode
{
    [TestFixture]
    public class TwoObjectContainerTests
    {
        [Test]
        public void Constructor_SetsFirstAndSecondObject()
        {
            // Arrange
            int expectedFirst = 10; 
            string expectedSecond = "Hello";

            // Act
            var container = new TwoObjectContainer<int, string>(expectedFirst, expectedSecond);

            // Assert
            Assert.AreEqual(expectedFirst, container.FirstObject, "FirstObject should be set to the initial value provided.");
            Assert.AreEqual(expectedSecond, container.SecondObject, "SecondObject should be set to the initial value provided.");
        }

        [Test]
        public void Container_HoldsDifferentTypes()
        {
            // Arrange
            double expectedFirst = 3.14159; //testing with a double
            bool expectedSecond = true; // testing with a boolean

            // Act
            var container = new TwoObjectContainer<double, bool>(expectedFirst, expectedSecond);

            // Assert
            Assert.AreEqual(expectedFirst, container.FirstObject, "FirstObject should correctly hold a double.");
            Assert.AreEqual(expectedSecond, container.SecondObject, "SecondObject should correctly hold a bool.");
        }

        [Test]
        public void Container_AllowsNullValues()
        {
            // Arrange
            string expectedFirst = null;
            object expectedSecond = null;

            // Act
            var container = new TwoObjectContainer<string, object>(expectedFirst, expectedSecond);

            // Assert
            Assert.IsNull(container.FirstObject, "FirstObject should allow null.");
            Assert.IsNull(container.SecondObject, "SecondObject should allow null.");
        }
    }
}
