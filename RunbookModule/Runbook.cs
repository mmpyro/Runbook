using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using RunbookModule.Loggers;
using RunbookModule.Report;
using RunbookModule.Sections;
using RunbookModule.Validators;
using RunbookModule.Providers;
using RunbookModule.Constants;
using RunbookModule.Dtos;

namespace RunbookModule
{
    public class Runbook : IRunbook
    {
        private readonly IReportCreator _reportCreator;
        private readonly ISectionValidator _sectionValidator;
        private readonly Stopwatch _sw = new Stopwatch();
        private bool _isSuccessStatus = true;

        public string Name { get; set; }

        public Runbook(IReportCreator reportCreator, ISectionValidator sectionValidator)
        {
            _reportCreator = reportCreator;
            _sectionValidator = sectionValidator;
            Sections = new List<ISection>();
        }

        public List<ISection> Sections { get; }

        public void Add(ISection section)
        {
            _sectionValidator.Validate(Sections, section);
            Sections.Add(section);
        }

        public void AddRange(IEnumerable<ISection> sections)
        {
            _sectionValidator.Validate(Sections, sections);
            Sections.AddRange(sections);
        }

        public void Invoke(ILogger logger)
        {
            var task = Task.Run(() =>
            {
                _sw.Reset();
                _sw.Start();
                foreach (var section in Sections)
                {
                    logger.Log($"Execute: {section.SectionName}");
                    var statusCode = section.Invoke(logger);
                    if (statusCode == StatusCode.Fail)
                    {
                        _isSuccessStatus = false;
                        break;
                    }
                }
                _sw.Stop();
            });

            Loggingloop(task);
        }

        public ReportDto OverallReport()
        {
            return _reportCreator.CreateReport(this, _sw.Elapsed);
        }

        public bool HasSuccessStatusCode => _isSuccessStatus;

        private static void Loggingloop(Task task)
        {
            var liveLogger = ContainerProvider.Resolve<ILogger>(ContainerConstants.LiveLogger) as LiveLogger;
            do
            {
                WriteLogToConsole(liveLogger);
                Thread.Sleep(TimeSpan.FromSeconds(.5));
            } while (!task.IsCompleted && !task.IsFaulted);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            WriteLogToConsole(liveLogger);
        }

        private static void WriteLogToConsole(LiveLogger liveLogger)
        {
            liveLogger?.WriteLogToConsole();
        }
    }
}