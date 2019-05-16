using RunbookModule.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace RunbookModule.Sections
{
    public class WindowSection : Section
    {
        private readonly int _windowSize;
        private readonly List<Task> _jobs = new List<Task>();
        private static readonly object Locker = new object();

        public WindowSection(string sectionName, int windowSize) : base(sectionName)
        {
            _windowSize = windowSize;
        }


        public override StatusCode Invoke(ILogger logger)
        {
            Sw.Reset();
            Sw.Start();
            Chapters.ToObservable().Buffer(_windowSize, _windowSize).Subscribe( col =>
            {
                col.ToList().ForEach(chapter =>
                {
                    var job = Task.Run(() =>
                    {
                        var report = chapter.Invoke(SectionName, logger);
                        lock (Locker)
                        {
                            ChaptersExecutionInfos.Add(report);
                        }
                    });
                    _jobs.Add(job);
                });
                Task.WaitAll(_jobs.ToArray());
            });
            Sw.Stop();
            return StatusCode;
        }
    }
}