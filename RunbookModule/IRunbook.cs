using System.Collections.Generic;
using RunbookModule.Dtos;
using RunbookModule.Sections;

namespace RunbookModule
{
    public interface IRunbook
  {
    string Name { get; set; }
    List<ISection> Sections { get; }

    void Add(ISection section);
    void AddRange(IEnumerable<ISection> sections);
    bool HasSuccessStatusCode();
    void Invoke();
    ReportDto OverallReport();
    void Remove(ISection section);
  }
}