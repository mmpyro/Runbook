using RunbookModule.Constants;
using RunbookModule.Sections;
using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, "Chapters")]
    public class AddChaptersCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = HelpMessages.SectionObjectMessage)]
        public ISection Section { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = HelpMessages.ChaptersArrayMessage)]
        public IChapter[] Chapters { get; set; }

        protected override void ProcessRecord()
        {
            Validate();

            Section.AddRange(Chapters);
        }

        private void Validate()
        {
            if (Section == null)
            {
                throw new ArgumentException(ErrorMessages.SectionNullErrorMessage);
            }
            if (Chapters == null)
            {
                throw new ArgumentException(ErrorMessages.ChapterNullErrorMessage);
            }
        }
    }
}
