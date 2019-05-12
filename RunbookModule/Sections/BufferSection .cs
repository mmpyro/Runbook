using RunbookModule.Loggers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RunbookModule.Sections
{
    public class BufferSection : Section
    {
      private readonly int _bufferSize;
      private readonly List<Task> _jobs = new List<Task>();
      private static readonly object Locker = new object();

      public BufferSection(string sectionName, int bufferSize) : base(sectionName)
      {
        _bufferSize = bufferSize;
      }

      public override StatusCode Invoke()
      {
        Sw.Reset();
        Sw.Start();
        var semaphore = new Semaphore(_bufferSize, _bufferSize);
        foreach (var chapter in Chapters)
        {
          var job = Task.Run(() =>
          {
            semaphore.WaitOne();
            var report = chapter.Run(SectionName);
            lock (Locker)
            {
              ChaptersExecutionInfos.Add(report);
            }
            semaphore.Release();
          });
          _jobs.Add(job);
        }

        Task.WaitAll(_jobs.ToArray());
        Sw.Stop();
        return StatusCode;
      }
    }
}