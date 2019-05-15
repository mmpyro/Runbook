using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RunbookModule.Dtos;
using RunbookModule.Sections;

namespace RunbookModule.Report
{
    public interface IReportCreator
    {
        ReportDto CreateReport(IRunbook runbook, TimeSpan overallExecutionTime);
    }

    public class ReportCreator : IReportCreator
    {
        public ReportDto CreateReport(IRunbook runbook, TimeSpan overallExecutionTime)
        {
            var sb = new StringBuilder("\nOverall report:\n");
            runbook.Sections.ForEach(section =>
            {
                string report = CreateReport(section);
                sb.AppendLine(!string.IsNullOrEmpty(report) ? report : $"{section.SectionName} skip because previous error");
            });
            sb.AppendLine($"Overall execution time: {overallExecutionTime.TotalSeconds} [s]");
            return new ReportDto(sb.ToString(), overallExecutionTime);
        }

        private string CreateReport(ISection section)
        {
            if (!section.ChaptersExecutionInfos.Any())
                return null;           
            var sb = new StringBuilder();
            sb.AppendLine($"Section: {section.SectionName}");
            sb.AppendLine( CreateReport(section.ChaptersExecutionInfos));
            sb.AppendLine($"Section execution time: {section.OverallExecutionSeconds} [s]");
            return sb.ToString();
        }

        private string CreateReport(List<ChapterExecutionInfo> sectionChaptersExecutionInfos)
        {
            StringBuilder sb = new StringBuilder();
            sectionChaptersExecutionInfos.ForEach(
                r =>
                {
                    sb.AppendLine(
                        $"{r.Name}: {r.StatusCode.ToString()} {r.ExecutionTime.TotalSeconds} [s] Retries: {GetNumberOfRetries(r)} {r.ErrorMessage}");
                });
            return sb.ToString();
        }

        private static int GetNumberOfRetries(ChapterExecutionInfo chapterExecutionInfo)
        {
            int numberOfRetries = chapterExecutionInfo.Retries;
            return numberOfRetries > 1 ? numberOfRetries-1 : 0;
        }
    }
}