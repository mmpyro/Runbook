using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet("Start", "Runbook")]
    public class StartRunbookCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Runbook object")]
        public IRunbook Runbook { get; set; }

        protected override void ProcessRecord()
        {
            if(Runbook == null)
            {
                throw new ArgumentException("Runbook cannot be null");
            }
            Runbook.Invoke();
            WriteObject(Runbook);
        }
    }
}