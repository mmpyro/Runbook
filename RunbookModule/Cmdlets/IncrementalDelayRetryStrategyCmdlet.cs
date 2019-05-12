using RunbookModule.RetriesStrategies;
using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "IncrementalDelayRetryStrategy")]
    public class IncrementalDelayRetryStrategyCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Delay time")]
        public TimeSpan Delay { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(new IncrementalDelayRetryStrategy(Delay));
        }
    }
}
