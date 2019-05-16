using RunbookModule.Constants;
using RunbookModule.Factories;
using RunbookModule.Loggers;
using RunbookModule.Providers;
using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet("Start", "Runbook")]
    public class StartRunbookCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = HelpMessages.RunbookObjectMessage)]
        public IRunbook Runbook { get; set; }

        [Parameter(Mandatory = false, Position = 1, HelpMessage = HelpMessages.RunbookOutDirMessage)]
        public string OutDir { get; set; }

        protected override void ProcessRecord()
        {
            ILogger logger = null;
            if(Runbook == null)
            {
                throw new ArgumentException(ErrorMessages.RunbookNullErrorMessage);
            }

            if(!string.IsNullOrEmpty(OutDir))
            {
                var composeLoggerFactory = ContainerProvider.Resolve<IComposeLoggerFactory>();
                logger = composeLoggerFactory.Create(OutDir);
            }
            else
            {
                logger = ContainerProvider.Resolve<ILogger>(ContainerConstants.LiveLogger);
            }
            Runbook.Invoke(logger);
        }
    }
}