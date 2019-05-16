using System.IO;
using static RunbookModule.Helpers.DateGeneratorHelper;

namespace RunbookModule.Loggers
{

    public class FileFileLogger : ILogger
    {
        private readonly string _logDirPath;

        public FileFileLogger(string logDirPath)
        {
            _logDirPath = Path.GetFullPath(logDirPath);
        }

        public void Log(string taskName, string txt)
        {
            string filePath = Path.Combine(_logDirPath, $"{taskName}.log");
            using (var sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine($"{CurrentDateString()}\t{txt}");
            }
        }

        public void Log(string msg)
        {
            //do nothing
        }
    }
}