using System;
using NUnit.Framework;
using RunbookModule;
using RunbookModule.Sections;
using System.Management.Automation;
using NSubstitute;
using static RunbookModuleTests.Helpers.ScriptBlockHelper;
using RunbookModule.Wrappers;
using RunbookModule.Loggers;

namespace RunbookModuleTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SequenceSectionTests
    {
        private IPsWrapperFactory _factory;
        private IPsWrapper _psWrapper;
        private ILogger _logger;

        [SetUp]
        public void Before()
        {
            _factory = Substitute.For<IPsWrapperFactory>();
            _psWrapper = Substitute.For<IPsWrapper>();
            _factory.Create().Returns(_psWrapper);
            _logger = Substitute.For<ILogger>();
        }

        [Test]
        public void ShouldRetryChapterWhenItFailByTheFirstTime()
        {
            //Arrange
            var section = new SequenceSection("section1");
            var chapter = new Chapter("chapter1", ScriptBlock.Create(""), _factory);
            _psWrapper.HadErrors.Returns(x => true, x => false);
            chapter.SetNumberOfRetries(2);
            section.Add(chapter);
            //Act
            var statusCode = section.Invoke(_logger);
            //Assert
            _psWrapper.Received(2).Invoke();
            Assert.That(statusCode, Is.EqualTo(StatusCode.Success));
        }

        [Test]
        public void ShouldReturnsFailStatusCodeWhenRetriesMoreThanNumberOfRetries()
        {
            //Arrange
            var section = new SequenceSection("");
            var chapter = new Chapter("chapter1", CreateScriptBlock(), _factory);
            _psWrapper.HadErrors.Returns(x => true);
            chapter.SetNumberOfRetries(3);
            section.Add(chapter);
            //Act
            var statusCode = section.Invoke(_logger);
            //Assert
            _psWrapper.Received(4).Invoke();
            Assert.That(statusCode, Is.EqualTo(StatusCode.Fail));
        }

        [Test]
        public void ShouldReturnsFailStatusCodeWhenRetriesMoreThanNumberOfRetriesWithIgnoreStream()
        {
            //Arrange
            var section = new SequenceSection("");
            var chapter = new Chapter("chapter1", CreateScriptBlock(), _factory, true);
            _psWrapper.HadErrors.Returns(x => true);
            _psWrapper.State.Returns(x => PSInvocationState.Failed);
            chapter.SetNumberOfRetries(3);
            section.Add(chapter);
            //Act
            var statusCode = section.Invoke(_logger);
            //Assert
            _psWrapper.Received(4).Invoke();
            Assert.That(statusCode, Is.EqualTo(StatusCode.Fail));
        }

        [Test]
        public void ShouldReturnsSuccessStatusCodeWhenErrorStreamIsIgnoredAndErrorOccours()
        {
            //Arrange
            var section = new SequenceSection("");
            var chapter = new Chapter("chapter1", CreateScriptBlock(), _factory, true);
            chapter.SetNumberOfRetries(2);
            section.Add(chapter);
            _psWrapper.HadErrors.Returns(true);
            _psWrapper.State.Returns(PSInvocationState.Completed);
            //Act
            var statusCode = section.Invoke(_logger);
            //Assert
            _psWrapper.Received(1).Invoke();
            Assert.That(statusCode, Is.EqualTo(StatusCode.Success));
        }

        [Test]
        public void ShouldNotRetryChapterWhenItFinishWithoutErrors()
        {
            //Arrange
            var section = new SequenceSection("");
            var chapter = new Chapter("chapter1", CreateScriptBlock(), _factory);
            chapter.SetNumberOfRetries(2);
            section.Add(chapter);
            _psWrapper.HadErrors.Returns(false);
            //Act
            var statusCode = section.Invoke(_logger);
            //Assert
            _psWrapper.Received(1).Invoke();
            Assert.That(statusCode, Is.EqualTo(StatusCode.Success));
        }

        [Test]
        public void ShouldThrowsArgumentExceptionWhenAddedChapterIsNull()
        {
            //Arrange
            string sectionName = "section1";
            var section = new SequenceSection(sectionName);
            //Act - Assert
            var ex = Assert.Throws<ArgumentException>(() => section.Add(null));
            Assert.That(ex.Message, Is.EqualTo($"Chapter cannot be null. Section: {sectionName}."));
        }

        [Test]
        public void ShouldThrowsArgumentExceptionWhenChapterInCollectionIsNull()
        {
            //Arrange
            string sectionName = "section1";
            var section = new SequenceSection(sectionName);
            //Act - Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                  {
                      section.AddRange(new[] { new Chapter("", CreateScriptBlock(), _factory), new Chapter("", CreateScriptBlock(), _factory), null });
                  });
            Assert.That(ex.Message, Is.EqualTo($"Chapter cannot be null. Section: {sectionName}."));
        }

        [Test]
        public void ShouldThrowsArgumentExceptionWhenRemovedChapterIsNull()
        {
            //Arrange
            string sectionName = "section1";
            var section = new SequenceSection(sectionName);
            //Act - Assert
            var ex = Assert.Throws<ArgumentException>(() => section.Add(null));
            Assert.That(ex.Message, Is.EqualTo($"Chapter cannot be null. Section: {sectionName}."));
        }

        [Test]
        public void ShouldSectionSizeBeEqualNumberOfChapters()
        {
            //Arrange
            var section = new SequenceSection("");
            section.Add(new Chapter("1", CreateScriptBlock(), _factory));
            section.Add(new Chapter("2", CreateScriptBlock(), _factory));
            section.AddRange(new[] { new Chapter("3", CreateScriptBlock(), _factory), new Chapter("4", CreateScriptBlock(), _factory) });
            //Act
            int size = section.Size;
            //Assert
            Assert.That(size, Is.EqualTo(4));
        }

        [Test]
        public void ShouldStatusBasedOnPSInvocationStateWhenErrorStreamIsIgnored()
        {
            //Arrange
            var section = new SequenceSection("");
            section.AddRange(new[] { new Chapter("1", CreateScriptBlock(), _factory), new Chapter("2", CreateScriptBlock(), _factory), new Chapter("3", CreateScriptBlock(), _factory) });
            _psWrapper.HadErrors.Returns(true);
            _psWrapper.State.Returns(x => PSInvocationState.Completed, x => PSInvocationState.Completed, x => PSInvocationState.Failed);
            //Act
            var statusCode = section.Invoke(_logger);
            //Assert
            Assert.That(statusCode, Is.EqualTo(StatusCode.Fail));
        }

        [Test]
        public void ShouldFail()
        {
            //Arrange
            var section = new SequenceSection("");
            section.AddRange(new[] { new Chapter("1", CreateScriptBlock(), _factory), new Chapter("2", CreateScriptBlock(), _factory), new Chapter("3", CreateScriptBlock(), _factory) });
            _psWrapper.HadErrors.Returns(x => false, x => false, x => true);
            //Act
            var statusCode = section.Invoke(_logger);
            //Assert
            Assert.That(statusCode, Is.EqualTo(StatusCode.Fail));
        }


    }
}