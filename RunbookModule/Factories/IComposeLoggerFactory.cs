using RunbookModule.Loggers;

namespace RunbookModule.Factories
{
    public interface IComposeLoggerFactory
    {
        ILogger Create(string logDirPath);
    }
}
