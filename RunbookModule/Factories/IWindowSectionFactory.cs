using RunbookModule.Sections;

namespace RunbookModule.Factories
{
  public interface IWindowSectionFactory
  {
    ISection Create(string sectionName, int bufferSize);
  }
}