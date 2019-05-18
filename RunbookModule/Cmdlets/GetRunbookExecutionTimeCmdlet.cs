using RunbookModule.Constants;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "ExecutionTime")]
    public class GetRunbookExecutionTimetCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = HelpMessages.RunbookObjectMessage)]
        public IRunbook Runbook { get; set; }

        protected override void ProcessRecord()
        {
            var executionTime = Runbook?.OverallReport()?.ExecutionTime;
            WriteObject(executionTime);
        }
    }
}