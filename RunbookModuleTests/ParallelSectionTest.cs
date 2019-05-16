using NSubstitute;
using NUnit.Framework;
using RunbookModule;
using RunbookModule.Loggers;
using RunbookModule.Sections;
using RunbookModule.Wrappers;
using static RunbookModuleTests.Helpers.ScriptBlockHelper;

namespace RunbookModuleTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ParallelSectionTest
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
        public void ShouldHasSuccessStatusWhenAllChapterFinishedWithoutErrors()
        {
            //Arrange
            var section = new ParallelSection("");
            _psWrapper.HadErrors.Returns(x => false, x => false);
            section.AddRange(new []{new Chapter("1", CreateScriptBlock(), _factory), new Chapter("2", CreateScriptBlock(), _factory) });
            //Act
            var statusCode = section.Invoke(_logger);
            //Assert
            Assert.That(statusCode, Is.EqualTo(StatusCode.Success));
        }

        [Test]
        public void ShouldHasFailureStatusWhenAanyChapterFinishedWithError()
        {
            //Arrange
            var section = new ParallelSection("");
            _psWrapper.HadErrors.Returns(x => false, x => true, x => true);
            section.AddRange(new[] { new Chapter("1", CreateScriptBlock(), _factory), new Chapter("2", CreateScriptBlock(), _factory), new Chapter("3", CreateScriptBlock(), _factory) });
            //Act
            var statusCode = section.Invoke(_logger);
            //Assert
            _psWrapper.Received(3).Invoke();
            Assert.That(statusCode, Is.EqualTo(StatusCode.Fail));
        }
    }
}