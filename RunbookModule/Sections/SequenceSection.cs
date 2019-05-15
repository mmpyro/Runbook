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

        public override StatusCode Invoke()
        {
            Sw.Reset();
            Sw.Start();
            ExecuteChapters();
            Sw.Stop();
            return StatusCode;
        }


        private void ExecuteChapters()
        {
            foreach (var chapter in Chapters)
            {
              var report = chapter.Run(SectionName);
              ChaptersExecutionInfos.Add(report);
              if (report.StatusCode == StatusCode.Fail)
                break;
            }
        }

    }
}