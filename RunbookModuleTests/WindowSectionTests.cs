using NSubstitute;
using NUnit.Framework;
using RunbookModule;
using RunbookModule.Sections;
using static RunbookModuleTests.Helpers.ScriptBlockHelper;
using RunbookModule.Wrappers;
using RunbookModule.Loggers;

namespace RunbookModuleTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class WindowSectionTests
    {
        private IPsWrapperFactory _factory;
        private IPsWrapper _psWrapper;
        private ILogger _logger;

        [SetUp]
        public void Before()
        {
            _factory = Substitute.For<IPsWrapperFactory>();
            _psWrapper = Substitute.For<IPsWrapper>();
            _logger = Substitute.For<ILogger>();
            _factory.Create().Returns(_psWrapper);
        }

        [Test]
        public void ShouldHasSuccessStatusWhenAllChapterFinishedWithoutErrors()
        {
            //Arrange
            var section = new WindowSection("", 2);
            _psWrapper.HadErrors.Returns(x => false, x => false, x => false);
            section.AddRange(new []{new Chapter("1", CreateScriptBlock(), _factory, _logger),new Chapter("2", CreateScriptBlock(), _factory, _logger), new Chapter("3", CreateScriptBlock(), _factory, _logger) });
            //Act
            var statusCode = section.Invoke();
            //Assert
            Assert.That(statusCode, Is.EqualTo(StatusCode.Success));
        }

        [Test]
        public void ShouldHasFailureStatusWhenAanyChapterFinishedWithError()
        {
            //Arrange
            var section = new WindowSection("", 2);
            _psWrapper.HadErrors.Returns(x => false, x => true, x=> false);
            section.AddRange(new[] { new Chapter("1", CreateScriptBlock(), _factory, _logger), new Chapter("2", CreateScriptBlock(), _factory, _logger), new Chapter("3", CreateScriptBlock(), _factory, _logger)});
            //Act
            var statusCode = section.Invoke();
            //Assert
            _psWrapper.Received(3).Invoke();
            section.ChaptersExecutionInfos.ForEach(ch => Assert.That(ch.Retries, Is.EqualTo(1)));
            Assert.That(statusCode, Is.EqualTo(StatusCode.Fail));
        }
    }
}