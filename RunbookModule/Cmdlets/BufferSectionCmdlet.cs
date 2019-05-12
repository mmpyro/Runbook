using System;
using System.Management.Automation;
using RunbookModule.Providers;
using RunbookModule.Factories;

namespace RunbookModule.Cmdlets
{
  [Cmdlet(VerbsCommon.New, "BufferSection")]
  public class BufferSectionCmdlet : Cmdlet
  {
    [Parameter(Mandatory = true, Position = 0, HelpMessage = "Name of section")]
    public string SectionName { get; set; }

    [Parameter(Mandatory = true, Position = 1, HelpMessage = "Size of buffer window")]
    public int BufferSize { get; set; }

    protected override void ProcessRecord()
    {
      Validate();
      var sectionFactory = ContainerProvider.Resolve<IBufferSectionFactory>();
      WriteObject(sectionFactory.Create(SectionName, BufferSize));
    }

    private void Validate()
    {
      if (string.IsNullOrEmpty(SectionName))
      {
        throw new ArgumentException("SectionName cannot be null or empty");
      }
      if (BufferSize < 1)
      {
        throw new ArgumentException("BufferSize cannot be lower than 1");
      }
    }
  }
}