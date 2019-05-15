using System;

namespace RunbookModule.Dtos
{
    public class ReportDto
    {
        public ReportDto(string content, TimeSpan executionTime)
        {
            Content = content;
            ExecutionTime = executionTime;
        }

        public string Content { get; private set; }
        public TimeSpan ExecutionTime { get; private set; }
    }
}
