using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "ImmediateRetryStrategy")]
    public class ImmediateRetryStrategyCmdlet : Cmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject(RetriesStrategies.RetryStrategy.Immediate);
        }
    }
}
