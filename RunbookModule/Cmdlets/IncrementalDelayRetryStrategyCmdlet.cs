using RunbookModule.Constants;
using RunbookModule.RetriesStrategies;
using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "IncrementalDelayRetryStrategy")]
    public class IncrementalDelayRetryStrategyCmdlet : Cmdlet
    {
        [Parameter(Mandatory = false, Position = 0, HelpMessage = HelpMessages.DelayMilliSecondsMessage)]
        public int MilliSeconds { get; set; }

        [Parameter(Mandatory = false, Position = 1, HelpMessage = HelpMessages.DelaySecondsMessage)]
        public int Seconds { get; set; }

        [Parameter(Mandatory = false, Position = 2, HelpMessage = HelpMessages.DelayMinutesMessage)]
        public int Minutes { get; set; }

        [Parameter(Mandatory = false, Position = 3, HelpMessage = HelpMessages.DelayHoursMessage)]
        public int Hours { get; set; }

        protected override void ProcessRecord()
        {
            var delay = new TimeSpan(0, Hours, Minutes, Seconds, MilliSeconds);
            WriteObject(new IncrementalDelayRetryStrategy(delay));
        }
    }
}
