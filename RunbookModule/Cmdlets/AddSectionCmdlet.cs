using RunbookModule.Constants;
using RunbookModule.Sections;
using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, "Section")]
    public class AddSectionCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = HelpMessages.RunbookObjectMessage)]
        public IRunbook Runbook { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = HelpMessages.SectionObjectMessage)]
        public ISection Section { get; set; }

        protected override void ProcessRecord()
        {
            Validate(Section, ErrorMessages.SectionNullErrorMessage);
            Validate(Runbook, ErrorMessages.RunbookNullErrorMessage);
            Runbook.Add(Section);
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
