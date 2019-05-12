using RunbookModule.Sections;

namespace RunbookModule.Factories
{
  public interface IBufferSectionFactory
  {
    ISection Create(string sectionName, int bufferSize);
  }
}