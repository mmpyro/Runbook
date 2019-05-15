using RunbookModule.Sections;

namespace RunbookModule.Factories
{
    public class WindowSectionFactory : IWindowSectionFactory
  {
    public ISection Create(string sectionName, int bufferSize)
    {
      return new WindowSection(sectionName, bufferSize);
    }
  }
}
