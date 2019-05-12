using RunbookModule.Sections;

namespace RunbookModule.Factories
{
    public class ParallelSectionFactroy : IParallelSectionFactroy
    {
        public ISection Create(string sectionName)
        {
            return new ParallelSection(sectionName);
        }
    }
}
