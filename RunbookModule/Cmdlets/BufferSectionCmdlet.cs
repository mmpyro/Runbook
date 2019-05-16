using System;
using System.Management.Automation;
using RunbookModule.Providers;
using RunbookModule.Factories;
using RunbookModule.Constants;

namespace RunbookModule.Cmdlets
{
  [Cmdlet(VerbsCommon.New, "BufferSection")]
  public class BufferSectionCmdlet : Cmdlet
  {
    [Parameter(Mandatory = true, Position = 0, HelpMessage = HelpMessages.SectionNameMessage)]
    public string SectionName { get; set; }

    [Parameter(Mandatory = true, Position = 1, HelpMessage = HelpMessages.BufferSizeMessage)]
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
        throw new ArgumentException(ErrorMessages.NullSectionNameErrorMessage);
      }
      if (BufferSize < 1)
      {
        throw new ArgumentException(ErrorMessages.InvalidBufferSizeErrorMessage);
      }
    }
  }
}