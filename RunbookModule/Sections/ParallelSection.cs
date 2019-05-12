using System.Collections.Generic;
using System.Threading.Tasks;

namespace RunbookModule.Sections
{
    public class ParallelSection : Section
    {
        private static readonly object Locker = new object();
        private readonly List<Task> _jobs = new List<Task>();

        public ParallelSection(string sectionName) : base(sectionName)
        { }

        public override StatusCode Invoke()
        {
            Sw.Reset();
            Sw.Start();
            Chapters.ForEach(chapter =>
                {
                    var job = Task.Run(() =>
                    {
                        var report = chapter.Run(SectionName);
                        lock (Locker)
                        {
                            ChaptersExecutionInfos.Add(report);
                        }
                    });
                    _jobs.Add(job);
                });

            Task.WaitAll(_jobs.ToArray());
            

            Sw.Stop();
            return StatusCode;
        }
    }
}