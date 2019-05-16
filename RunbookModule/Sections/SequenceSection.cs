using RunbookModule.Loggers;
using System;
using System.Collections.Generic;

namespace RunbookModule.Sections
{
    public class SequenceSection : Section
    {
        private readonly Dictionary<string, TimeSpan> _timeoutDict = new Dictionary<string, TimeSpan>();

        public SequenceSection(string sectionName) : base(sectionName)
        {
        }

        public override StatusCode Invoke(ILogger logger)
        {
            Sw.Reset();
            Sw.Start();
            ExecuteChapters(logger);
            Sw.Stop();
            return StatusCode;
        }


        private void ExecuteChapters(ILogger logger)
        {
            foreach (var chapter in Chapters)
            {
              var report = chapter.Invoke(SectionName, logger);
              ChaptersExecutionInfos.Add(report);
              if (report.StatusCode == StatusCode.Fail)
                break;
            }
        }

    }
}