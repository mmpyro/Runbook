using RunbookModule.Constants;
using RunbookModule.Providers;
using RunbookModule.Sections;
using RunbookModule.Validators;
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
            Validate();
            Section.Add(Chapter);
        }

        private void Validate()
        {
            var propertyValidator = ContainerProvider.Resolve<IPropertyValidator>();
            propertyValidator
                .NotNull(Section, ErrorMessages.SectionNullErrorMessage)
                .NotNull(Chapter, ErrorMessages.ChapterNullErrorMessage);
        }
    }
}
