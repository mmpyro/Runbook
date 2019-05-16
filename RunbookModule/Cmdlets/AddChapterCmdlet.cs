using RunbookModule.Constants;
using RunbookModule.Sections;
using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, "Chapter")]
    public class AddChapterCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = HelpMessages.SectionObjectMessage)]
        public ISection Section { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = HelpMessages.ChapterObjectMessage)]
        public IChapter Chapter { get; set; }


        protected override void ProcessRecord()
        {
            Validate(Section, ErrorMessages.SectionNullErrorMessage);
            Validate(Chapter, ErrorMessages.ChapterNullErrorMessage);
            Section.Add(Chapter);
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
