using RunbookModule.Loggers;

namespace RunbookModule.Factories
{
    public class FileLoggerFactory : IFileLoggerFactory
    {
        public ILogger Create(string logDirPath)
        {
            return new FileFileLogger(logDirPath);
        }
    }
}
