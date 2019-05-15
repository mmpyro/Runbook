using RunbookModule.Sections;
using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, "Sections")]
    public class AddSectionsCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Runbook object")]
        public IRunbook Runbook { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Sections array")]
        public ISection[] Sections { get; set; }

        protected override void ProcessRecord()
        {
            Validate();

            Runbook.AddRange(Sections);
            WriteObject(Runbook);
        }

        private void Validate()
        {
            if (Runbook == null)
            {
                throw new ArgumentException("Runbook cannot be null");
            }
            if (Sections == null)
            {
                throw new ArgumentException("Sections cannot be null");
            }
        }
    }
}
