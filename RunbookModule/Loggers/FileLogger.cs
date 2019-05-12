using System.IO;
using static RunbookModule.Helpers.DateGeneratorHelper;

namespace RunbookModule.Loggers
{

  public class FileFileLogger : ILogger
  {
    private string _logDirPath;

    public void Log(string taskName, string txt)
    {
      CreateLogDirIfNotExists(_logDirPath);
      string filePath = Path.Combine(_logDirPath, $"{taskName}.log");
      using (var sw = new StreamWriter(filePath, true))
      {
        sw.WriteLine(txt);
      }
    }

    public void Log(string msg)
    {
      //do nothing
    }

    private void CreateLogDirIfNotExists(string dirPath)
    {
      if (!Directory.Exists(dirPath))
      {
        Directory.CreateDirectory(dirPath);
      }
    }

    public void SetLoggerName(string name)
    {
      _logDirPath = Path.Combine(Path.GetTempPath(), $"runbook_{CurrentDateString()}_{name}");
      CreateLogDirIfNotExists(_logDirPath);
    }
  }
}