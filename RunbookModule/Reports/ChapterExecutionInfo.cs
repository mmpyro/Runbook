using System;

namespace RunbookModule.Report
{
    public class ChapterExecutionInfo
    {
        public string Name { get; set; }
        public string ErrorMessage { get; set; }
        public StatusCode StatusCode { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public int Retries { get; set; }
    }
}