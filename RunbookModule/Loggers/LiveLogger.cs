using System;
using System.Collections.Concurrent;

namespace RunbookModule.Loggers
{
  public class LiveLogger : ILogger
  {
    private readonly ConcurrentQueue<string> MessageQ = new ConcurrentQueue<string>();

    private void Enqueue(string message)
    {
      MessageQ.Enqueue(message);
    }

    public void WriteLogToConsole()
    {
      while (!IsEmpty())
      {
        var message = Dequeue();
        if (!string.IsNullOrEmpty(message))
        {
          Console.WriteLine(message);
        }
      }
    }

    private bool IsEmpty()
    {
      return MessageQ.IsEmpty;
    }

    private string Dequeue()
    {
      if (MessageQ.TryDequeue(out string message))
        return message;
      return null;
    }

    public void Log(string msg)
    {
      Enqueue(msg);
    }

    public void Log(string sectionName, string msg)
    {
      Enqueue(msg);
    }
  }
}