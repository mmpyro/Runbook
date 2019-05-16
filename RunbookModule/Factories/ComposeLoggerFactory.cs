using RunbookModule.Constants;
using RunbookModule.Loggers;
using RunbookModule.Providers;

namespace RunbookModule.Factories
{

    public class ComposeLoggerFactory : IComposeLoggerFactory
    {
        private readonly IFileLoggerFactory _fileLoggerFactory;

        public ComposeLoggerFactory(IFileLoggerFactory fileLoggerFactory)
        {
            _fileLoggerFactory = fileLoggerFactory;
        }

        public ILogger Create(string logDirPath)
        {
            var liveLogger = ContainerProvider.Resolve<ILogger>(ContainerConstants.LiveLogger);
            var fileLogger = _fileLoggerFactory.Create(logDirPath);
            return new ComposeLogger(liveLogger, fileLogger);
        }
    }
}
