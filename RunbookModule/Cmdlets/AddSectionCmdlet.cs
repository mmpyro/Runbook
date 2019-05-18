using RunbookModule.Constants;
using RunbookModule.Providers;
using RunbookModule.Sections;
using RunbookModule.Validators;
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
            Validate();
            Runbook.Add(Section);
        }

        private void Validate()
        {
            var propertyValidator = ContainerProvider.Resolve<IPropertyValidator>();
            propertyValidator
                .NotNull(Section, ErrorMessages.SectionNullErrorMessage)
                .NotNull(Runbook, ErrorMessages.RunbookNullErrorMessage);
        }
    }
}
