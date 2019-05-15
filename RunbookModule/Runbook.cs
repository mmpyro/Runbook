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
using Ninject;
using RunbookModule.Dtos;

namespace RunbookModule
{
    public class Runbook : IRunbook
    {
        private readonly ILogger _logger;
        private readonly IReportCreator _reportCreator;
        private readonly ISectionValidator _sectionValidator;
        private readonly Stopwatch _sw = new Stopwatch();
        private bool _isSuccessStatus = true;

        public string Name { get; set; }

        public Runbook([Named(ContainerConstants.LiveLogger)] ILogger logger, IReportCreator reportCreator, ISectionValidator sectionValidator)
        {
            _logger = logger;
            _reportCreator = reportCreator;
            _sectionValidator = sectionValidator;
            Sections = new List<ISection>();
        }

        public List<ISection> Sections { get; }

        public void Add(ISection section)
        {
            _sectionValidator.Validate(section);
            Sections.Add(section);
        }

        public void AddRange(IEnumerable<ISection> sections)
        {
            _sectionValidator.Validate(sections);
            Sections.AddRange(sections);
        }

        public void Remove(ISection section)
        {
            _sectionValidator.Validate(section);
            Sections.Remove(section);
        }

        public void Invoke()
        {
            _logger.SetLoggerName(Name);
            var task = Task.Run(() =>
            {
                _sw.Reset();
                _sw.Start();
                foreach (var section in Sections)
                {
                    _logger.Log($"Execute: {section.SectionName}");
                    var statusCode = section.Invoke();
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

        public ReportDto OverallReport()
        {
            return _reportCreator.CreateReport(this, _sw.Elapsed);
        }

        public bool HasSuccessStatusCode()
        {
            return _isSuccessStatus;
        }
    }
}