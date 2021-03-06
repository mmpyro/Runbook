﻿using RunbookModule.Constants;
using RunbookModule.Factories;
using RunbookModule.Providers;
using RunbookModule.RetriesStrategies;
using RunbookModule.Validators;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Chapter")]
    public class ChapterCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Chapter name")]
        public string Name { get; set; }

        [Parameter(HelpMessage = "Arguments for Chapter script block")]
        public object[] Arguments { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Script block which will be invoked")]
        public ScriptBlock Action { get; set; }

        [Parameter(HelpMessage = "Number of retires when chapter executions fails")]
        public int NumberOfRetries { get; set; } = 0;

        [Parameter(HelpMessage = "If true error stream will be ignore when checking completion status")]
        public SwitchParameter IgnoreErrorStream { get; set; }

        [Parameter(HelpMessage = "Retry strategy for chapter")]
        public IRetryStrategy RetryStrategy { get; set; }

        protected override void ProcessRecord()
        {
            Validate();
            var chapterFactory = ContainerProvider.Resolve<IChapterFactory>();
            IChapter chapter = null;
            if (Arguments == null)
            {
                if (RetryStrategy != null)
                {
                    chapter = chapterFactory.Create(Name, Action, RetryStrategy, IgnoreErrorStream.IsPresent);
                }
                else
                {
                    chapter = chapterFactory.Create(Name, Action, IgnoreErrorStream.IsPresent);
                }
            }
            else
            {
                if (RetryStrategy != null)
                {
                    chapter = chapterFactory.Create(Name, Arguments, Action, RetryStrategy, IgnoreErrorStream.IsPresent);
                }
                else
                {
                    chapter = chapterFactory.Create(Name, Arguments, Action, IgnoreErrorStream.IsPresent);
                }
            }
            chapter.SetNumberOfRetries(NumberOfRetries);
            WriteObject(chapter);
        }

        private void Validate()
        {
            var propertyValidator = ContainerProvider.Resolve<IPropertyValidator>();
            propertyValidator
                     .NotNullOrEmpty(Name, ErrorMessages.NullChapterNameErrorMessage)
                     .NotNull(Action, ErrorMessages.InvalidActionErrorMessage)
                     .GreaterOrEqualZero(NumberOfRetries, ErrorMessages.InvalidNumberOfRetriesErrorMessage);
        }
    }
}