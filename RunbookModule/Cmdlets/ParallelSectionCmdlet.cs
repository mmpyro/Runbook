using System;
using System.Management.Automation;
using RunbookModule.Providers;
using RunbookModule.Factories;
using RunbookModule.Constants;

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
            if (string.IsNullOrEmpty(SectionName))
            {
                throw new ArgumentException(ErrorMessages.NullSectionNameErrorMessage);
            }
        }
    }
}