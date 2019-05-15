using RunbookModule.Sections;
using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, "Section")]
    public class AddSectionCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Runbook object")]
        public IRunbook Runbook { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Section object")]
        public ISection Section { get; set; }

        protected override void ProcessRecord()
        {
            Validate(Section, "Section cannot be null");
            Validate(Runbook, "Runbook cannot be null");
            Runbook.Add(Section);
            WriteObject(Runbook);
        }

        private void Validate(object obj, string message)
        {
            if(obj == null)
            {
                throw new ArgumentException(message);
            }
        }
    }
}
