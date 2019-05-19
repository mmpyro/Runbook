using NSubstitute;
using NUnit.Framework;
using RunbookModule.Constants;
using RunbookModule.Providers;
using RunbookModule.Sections;
using RunbookModule.Validators;
using System;
using System.Collections.Generic;

namespace RunbookModuleTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SectionValidatorTests
    {
        private ISectionValidator _sectionValidator;

        [SetUp]
        public void SetUp()
        {
            _sectionValidator = ContainerProvider.Resolve<ISectionValidator>();
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenSectionIsNull()
        {
            //Arrange
            ISection sectionToValidate = null;

            //Act
            var ex = Assert.Throws<ArgumentException>( () => _sectionValidator.Validate(new List<ISection>(), sectionToValidate));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(ErrorMessages.SectionNullErrorMessage));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenSectionDoesntHaveAnyChapter()
        {
            //Arrange
            var sectionToValidate = new SequenceSection(nameof(SequenceSection));

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _sectionValidator.Validate(new List<ISection>(), sectionToValidate));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(ErrorMessages.SectionNullErrorMessage));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenSectionWithTheSameNameAlreadyExists()
        {
            //Arrange
            const string sectionName = "section1";
            var runbookSections = new List<ISection>
            {
                new SequenceSection(sectionName)
            };
            var sectionToValidate = Substitute.For<ISection>();
            sectionToValidate.SectionName.Returns(sectionName);
            sectionToValidate.Size.Returns(1);

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _sectionValidator.Validate(runbookSections, sectionToValidate));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo($"Section: {sectionName} already exists in that runbook."));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenAnySectionIsNull()
        {
            var section = Substitute.For<ISection>();
            section.SectionName.Returns("section");
            section.Size.Returns(1);
            //Arrange
            var sectionsToValidate = new List<ISection>
            {
                section,
                null
            };

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _sectionValidator.Validate(new List<ISection>(), sectionsToValidate));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(ErrorMessages.SectionNullErrorMessage));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenAnySectionHasNotChapter()
        {
            var firstSection = Substitute.For<ISection>();
            firstSection.SectionName.Returns("section1");
            firstSection.Size.Returns(1);
            var secondtSection = Substitute.For<ISection>();
            secondtSection.SectionName.Returns("section2");
            secondtSection.Size.Returns(0);

            //Arrange
            var sectionsToValidate = new List<ISection>
            {
                firstSection,
                secondtSection
            };

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _sectionValidator.Validate(new List<ISection>(), sectionsToValidate));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo(ErrorMessages.SectionNullErrorMessage));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenSectionsToValidateNamesAreNotUnique()
        {
            const string sectionName = "section1";
            var firstSection = Substitute.For<ISection>();
            firstSection.SectionName.Returns(sectionName);
            firstSection.Size.Returns(1);
            var secondtSection = Substitute.For<ISection>();
            secondtSection.SectionName.Returns(sectionName);
            secondtSection.Size.Returns(1);

            //Arrange
            var sectionsToValidate = new List<ISection>
            {
                firstSection,
                secondtSection
            };

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _sectionValidator.Validate(new List<ISection>(), sectionsToValidate));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo($"Sections {sectionName} are not unique inside runbook."));
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenSectionsNamesAreNotUnique()
        {
            const string sectionName = "section1";
            var firstSection = Substitute.For<ISection>();
            firstSection.SectionName.Returns(sectionName);
            firstSection.Size.Returns(1);
            var secondtSection = Substitute.For<ISection>();
            secondtSection.SectionName.Returns("section2");
            secondtSection.Size.Returns(1);

            //Arrange
            var sectionsToValidate = new List<ISection>
            {
                firstSection,
                secondtSection
            };

            var runbookSections = new List<ISection>
            {
                firstSection
            };

            //Act
            var ex = Assert.Throws<ArgumentException>(() => _sectionValidator.Validate(runbookSections, sectionsToValidate));

            //Assert
            Assert.NotNull(ex);
            Assert.That(ex.Message, Is.EqualTo($"Sections {sectionName} are not unique inside runbook."));
        }
    }
}
