namespace RunbookModule.Loggers
{
    public interface ILogger
    {
        void SetLoggerName(string name);
        void Log(string msg);
        void Log(string sectionName, string msg);
    }
}