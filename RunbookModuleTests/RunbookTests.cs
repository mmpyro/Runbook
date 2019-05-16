using System;
using NSubstitute;
using NUnit.Framework;
using RunbookModule;
using RunbookModule.Report;
using RunbookModule.Sections;
using RunbookModule.Validators;
using RunbookModule.Wrappers;
using RunbookModule.Loggers;
using System.Collections.Generic;

namespace RunbookModuleTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class RunbookTests
    {
        private const string ErrorMessage = "Section cannot be null or empty";
        private const string RunbookName = "wusa";
        private ILogger _logger;
        private IPsWrapperFactory _factory;


        [SetUp]
        public void Before()
        {
            _factory = Substitute.For<IPsWrapperFactory>();
            _logger = Substitute.For<ILogger>();
        }

        [Test]
        public void ShouldThrowsArgumentExceptionWhenAddedSectionIsNull()
        {
            //Arrange
            var runBook = new Runbook(new ReportCreator(), new SectionValidator());
            //Act - Assert
            var ex = Assert.Throws<ArgumentException>(() => runBook.Add(null));
            Assert.That(ex.Message, Is.EqualTo(ErrorMessage));
        }

        [Test]
        public void ShouldThrowsArgumentExceptionWhenSectionInCollectionIsNull()
        {
            //Arrange
            var runBook = new Runbook(new ReportCreator(), new SectionValidator());
            //Act - Assert
            var ex = Assert.Throws<ArgumentException>(() => runBook.AddRange(new[] { new SequenceSection(""), null }));
            Assert.That(ex.Message, Is.EqualTo(ErrorMessage));
        }

        [Test]
        public void ShouldThrowsArgumentExceptionWhenRemovedSectionIsNull()
        {
            //Arrange
            var runBook = new Runbook(new ReportCreator(), new SectionValidator())
            {
                Name = RunbookName
            };
            //Act - Assert
            var ex = Assert.Throws<ArgumentException>(() => runBook.Remove(null));
            Assert.That(ex.Message, Is.EqualTo(ErrorMessage));
        }

        [Test]
        public void ShouldThrowsArgumentExceptionWhenAddEmptySection()
        {
            //Arrange
            var runBook = new Runbook(new ReportCreator(), new SectionValidator())
            {
                Name = RunbookName
            };
            //Act - Assert
            Assert.Throws<ArgumentException>(() => runBook.Add(new SequenceSection("")));
        }

        [Test]
        public void ShouldThrowsArgumentExceptionWhenAnySectionIsEmpty()
        {
            //Arrange
            var runBook = new Runbook(new ReportCreator(), new SectionValidator())
            {
                Name = RunbookName
            };
            //Act - Assert
            Assert.Throws<ArgumentException>(() => runBook.AddRange(new[] { new SequenceSection("") }));
        }

        [Test]
        public void ShouldInvokeSectionsUntilSectionFails()
        {
            //Arrange
            var runBook = new Runbook(new ReportCreator(), new SectionValidator())
            {
                Name = RunbookName
            };
            var section1 = Substitute.For<ISection>();
            section1.StatusCode.Returns(StatusCode.Fail);
            section1.Size.Returns(1);
            var section2 = Substitute.For<ISection>();
            section2.Size.Returns(1);
            runBook.AddRange(new[] { section1, section2 });
            //Act
            runBook.Invoke(_logger);
            //Assert
            section1.Received(1).Invoke(Arg.Any<ILogger>());
            section2.DidNotReceive().Invoke(Arg.Any<ILogger>());
        }

        [Test]
        public void ShouldInvokeAllSections()
        {
            //Arrange
            var runBook = new Runbook(new ReportCreator(), new SectionValidator())
            {
                Name = RunbookName
            };
            var section1 = Substitute.For<ISection>();
            section1.ChaptersExecutionInfos.Returns(_ => new List<ChapterExecutionInfo>());
            section1.Invoke(_logger).Returns(StatusCode.Success);
            section1.StatusCode.Returns(StatusCode.Success);
            section1.Size.Returns(1);
            section1.SectionName.Returns("section1");
            var section2 = Substitute.For<ISection>();
            section2.ChaptersExecutionInfos.Returns(_ => new List<ChapterExecutionInfo>());
            section2.Invoke(_logger).Returns(StatusCode.Success);
            section2.StatusCode.Returns(StatusCode.Success);
            section2.SectionName.Returns("section2");
            section2.Size.Returns(1);
            runBook.AddRange(new[] { section1, section2 });
            //Act
            runBook.Invoke(_logger);
            var report = runBook.OverallReport();
            //Assert
            section1.Received(1).Invoke(Arg.Any<ILogger>());
            section2.Received(1).Invoke(Arg.Any<ILogger>());
            Assert.NotNull(report);
        }
    }
}