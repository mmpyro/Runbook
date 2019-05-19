using System.Management.Automation;
using RunbookModule.Providers;
using RunbookModule.Factories;
using RunbookModule.Constants;
using RunbookModule.Validators;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "ParallelSection")]
    public class ParallelSectionCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = HelpMessages.SectionNameMessage)]
        public string SectionName { get; set; }

        protected override void ProcessRecord()
        {
            Validate();
            var section = ContainerProvider.Resolve<IParallelSectionFactroy>();
            WriteObject(section.Create(SectionName));
        }

        private void Validate()
        {
            var propertyValidator = ContainerProvider.Resolve<IPropertyValidator>();
            propertyValidator
                .NotNullOrEmpty(SectionName, ErrorMessages.NullSectionNameErrorMessage);
        }
    }
}