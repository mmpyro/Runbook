using NUnit.Framework;
using RunbookModule.Providers;
using RunbookModule.Validators;
using System;

namespace RunbookModuleTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class PropertyValidatorTests
    {
        private IPropertyValidator _propertyValidator;

        [SetUp]
        public void Setup()
        {
            _propertyValidator = ContainerProvider.Resolve<IPropertyValidator>();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void ShouldThrowArgumentExceptionWhenValueToValidateIsNullOrEmpty(string valueToValidate)
        {
            //Arrange
            const string errorMessage = "Value cannot be null or empty.";

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _propertyValidator.NotNullOrEmpty(valueToValidate, errorMessage));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(errorMessage));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenValueToValidateIsNull()
        {
            //Arrange
            const string errorMessage = "Value cannot be null.";
            const object value = null;

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _propertyValidator.NotNull(value, errorMessage));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(errorMessage));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentExceptionWhenValueIsNotGreaterThanZero(int value)
        {
            //Arrange
            const string errorMessage = "Value cannot be lower than zero.";

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _propertyValidator.GreaterThanZero(value, errorMessage));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(errorMessage));
        }

        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(0)]
        public void ShouldThrowArgumentExceptionWhenValueIsNotGreaterThanOne(int value)
        {
            //Arrange
            const string errorMessage = "Value cannot be lower than zero.";

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _propertyValidator.GreaterThanOne(value, errorMessage));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(errorMessage));
        }

        public void ShouldThrowArgumentExceptionWhenValueIsNotgreaterThanOne()
        {
            //Arrange
            const string errorMessage = "Value cannot be lower than zero.";
            const int value = -1;

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _propertyValidator.GreaterOrEqualZero(value, errorMessage));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(errorMessage));
        }
    }
}
