using RunbookModule.Sections;
using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, "Chapters")]
    public class AddChaptersCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Section object")]
        public ISection Section { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Chapters array")]
        public IChapter[] Chapters { get; set; }

        protected override void ProcessRecord()
        {
            Validate();

            Section.AddRange(Chapters);
            WriteObject(Section);
        }

        private void Validate()
        {
            if (Section == null)
            {
                throw new ArgumentException("Section cannot be null");
            }
            if (Chapters == null)
            {
                throw new ArgumentException("Chapters cannot be null");
            }
        }
    }
}
