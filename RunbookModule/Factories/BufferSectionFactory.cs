using RunbookModule.Providers;
using RunbookModule.Sections;
using RunbookModule.Wrappers;

namespace RunbookModule.Factories
{
  public class BufferSectionFactory : IBufferSectionFactory
  {
    public ISection Create(string sectionName, int bufferSize)
    {
      return new BufferSection(sectionName, bufferSize);
    }
  }
}
