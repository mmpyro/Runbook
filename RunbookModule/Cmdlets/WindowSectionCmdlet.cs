using System.Management.Automation;
using RunbookModule.Providers;
using RunbookModule.Factories;
using RunbookModule.Constants;
using RunbookModule.Validators;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "WindowSection")]
    public class WindowSectionCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = HelpMessages.SectionNameMessage)]
        public string SectionName { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = HelpMessages.BufferSizeMessage)]
        public int BufferSize { get; set; }

        protected override void ProcessRecord()
        {
            Validate();
            var sectionFactory = ContainerProvider.Resolve<IWindowSectionFactory>();
            WriteObject(sectionFactory.Create(SectionName, BufferSize));
        }

        private void Validate()
        {
            var propertyValidator = ContainerProvider.Resolve<IPropertyValidator>();
            propertyValidator
                     .NotNullOrEmpty(SectionName, ErrorMessages.NullSectionNameErrorMessage)
                     .GreaterThanOne(BufferSize, ErrorMessages.InvalidBufferSizeErrorMessage);
        }
    }
}