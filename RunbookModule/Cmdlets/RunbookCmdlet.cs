using System.Management.Automation;
using RunbookModule.Providers;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "Runbook")]
    public class RunbookCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of section")]
        public string RunbookName { get; set; }

        protected override void ProcessRecord()
        {
            var runbook = ContainerProvider.Resolve<IRunbook>();
            runbook.Name = RunbookName;
            WriteObject(runbook);
        }
    }
}