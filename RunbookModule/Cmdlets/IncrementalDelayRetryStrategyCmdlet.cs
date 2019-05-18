using RunbookModule.Constants;
using RunbookModule.Providers;
using RunbookModule.RetriesStrategies;
using RunbookModule.Validators;
using System;
using System.Management.Automation;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "IncrementalDelayRetryStrategy")]
    public class IncrementalDelayRetryStrategyCmdlet : Cmdlet
    {
        [Parameter(Mandatory = false, Position = 0, HelpMessage = HelpMessages.DelayMilliSecondsMessage)]
        public int MilliSeconds { get; set; }

        [Parameter(Mandatory = false, Position = 1, HelpMessage = HelpMessages.DelaySecondsMessage)]
        public int Seconds { get; set; }

        [Parameter(Mandatory = false, Position = 2, HelpMessage = HelpMessages.DelayMinutesMessage)]
        public int Minutes { get; set; }

        [Parameter(Mandatory = false, Position = 3, HelpMessage = HelpMessages.DelayHoursMessage)]
        public int Hours { get; set; }

        protected override void ProcessRecord()
        {
            Validate();
            var delay = new TimeSpan(0, Hours, Minutes, Seconds, MilliSeconds);
            WriteObject(new IncrementalDelayRetryStrategy(delay));
        }

        private void Validate()
        {
            var propertyValidator = ContainerProvider.Resolve<IPropertyValidator>();
            propertyValidator
                .GreaterOrEqualZero(MilliSeconds, $"{nameof(MilliSeconds)} has to be greater or equal zero")
                .GreaterOrEqualZero(Seconds, $"{nameof(Seconds)} has to be greater or equal zero")
                .GreaterOrEqualZero(Minutes, $"{nameof(Minutes)} has to be greater or equal zero")
                .GreaterOrEqualZero(Hours, $"{nameof(Hours)} has to be greater or equal zero");
        }
    }
}
