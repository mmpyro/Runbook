using RunbookModule.Sections;

namespace RunbookModule.Factories
{
    public class SequenceSectionFactory : ISequenceSectionFactory
    {
        public ISection Create(string sectionName)
        {
            return new SequenceSection(sectionName);
        }
    }
}
