using System;
using System.Management.Automation;
using RunbookModule.Providers;
using RunbookModule.Factories;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "SequenceSection")]
    public class SequenceSectionCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of section")]
        public string SectionName { get; set; }

        protected override void ProcessRecord()
        {
            Validate();
            var section = ContainerProvider.Resolve<ISequenceSectionFactory>();
            WriteObject(section.Create(SectionName));
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(SectionName))
            {
                throw new ArgumentException("SectionName cannot be null or empty");
            }
        }
    }
}