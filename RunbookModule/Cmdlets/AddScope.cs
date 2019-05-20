using RunbookModule.Constants;
using RunbookModule.Providers;
using RunbookModule.Validators;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.Add, "Scope")]
    public class AddScope : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = HelpMessages.ChapterObjectMessage)]
        public IChapter Chapter { get; set; }

        [Parameter(Mandatory = true, Position = 1, HelpMessage = HelpMessages.ScopeMessage)]
        public ScriptBlock Scope { get; set; }

        protected override void ProcessRecord()
        {
            Validate();
            Chapter.AddScope(Scope);
        }

        private void Validate()
        {
            var propertyValidator = ContainerProvider.Resolve<IPropertyValidator>();
            propertyValidator
                .NotNull(Chapter, ErrorMessages.ChapterNullErrorMessage)
                .NotNull(Scope, ErrorMessages.ScopeNullErrorMessage);
        }
    }
}
