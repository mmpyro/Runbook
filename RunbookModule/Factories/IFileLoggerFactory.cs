using RunbookModule.Loggers;

namespace RunbookModule.Factories
{
    public interface IFileLoggerFactory
    {
        ILogger Create(string logDirPath);
    }
}