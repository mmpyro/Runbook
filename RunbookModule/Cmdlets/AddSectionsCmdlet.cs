using RunbookModule.Constants;
using RunbookModule.Providers;
using RunbookModule.Sections;
using RunbookModule.Validators;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, "Sections")]
    public class AddSectionsCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = HelpMessages.RunbookObjectMessage)]
        public IRunbook Runbook { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = HelpMessages.SectionsArrayMessage)]
        public ISection[] Sections { get; set; }

        protected override void ProcessRecord()
        {
            Validate();
            Runbook.AddRange(Sections);
        }

        private void Validate()
        {
            var propertyValidator = ContainerProvider.Resolve<IPropertyValidator>();
            propertyValidator
                .NotNull(Runbook, ErrorMessages.RunbookNullErrorMessage)
                .NotNull(Sections, ErrorMessages.SectionsNullErrorMessage);
        }
    }
}
