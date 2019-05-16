using RunbookModule.Constants;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet("Write", "Report")]
    public class WriteReportCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = HelpMessages.RunbookObjectMessage)]
        public IRunbook Runbook { get; set; }

        protected override void ProcessRecord()
        {
            var content = Runbook?.OverallReport()?.Content;
            WriteObject(content);
        }
    }
}