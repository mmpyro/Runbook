using RunbookModule.Sections;
using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, "Chapter")]
    public class AddChapterCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "Section object")]
        public ISection Section { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = "Chapter object")]
        public IChapter Chapter { get; set; }


        protected override void ProcessRecord()
        {
            Validate(Section, "Section cannot be null");
            Validate(Chapter, "Chapter cannot be null");
            Section.Add(Chapter);
            WriteObject(Section);
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
