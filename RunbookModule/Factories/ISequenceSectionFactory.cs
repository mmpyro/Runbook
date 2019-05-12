using RunbookModule.Sections;

namespace RunbookModule.Factories
{
    public interface ISequenceSectionFactory
    {
        ISection Create(string sectionName);
    }
}