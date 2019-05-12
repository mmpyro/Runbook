using RunbookModule.Sections;

namespace RunbookModule.Factories
{
    public interface IParallelSectionFactroy
    {
        ISection Create(string sectionName);
    }
}