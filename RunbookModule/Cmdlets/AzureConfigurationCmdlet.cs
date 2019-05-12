using System;
using System.Management.Automation;
using RunbookModule.Dtos;

namespace RunbookModule.Cmdlets
{
    [Cmdlet(VerbsCommon.New, "AzureConfiguration")]
    public class AzureConfigurationCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0, HelpMessage = "SubscriptionId")]
        public string SubscriptionId { get; set; }
        [Parameter(Mandatory = true, Position = 1, HelpMessage = "CurrentStorageAccount of deploying web services")]
        public string CurrentStorageAccount { get; set; }
        [Parameter(Mandatory = true, Position = 2, HelpMessage = "Azure credentials")]
        public PSCredential Credentials { get; set; }

        protected override void ProcessRecord()
        {
            Validate();
            var azureConfiguration = new AzureConfigurationDto
            {
                Credentials = Credentials,
                CurrentStorageAccount = CurrentStorageAccount,
                SubscriptionId = SubscriptionId
            };
            WriteObject(azureConfiguration);
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(SubscriptionId))
            {
                throw new ArgumentException("SubscriptionID cannot be null or empty.");
            }
            if (string.IsNullOrEmpty(CurrentStorageAccount))
            {
                throw new ArgumentException("CurrentStorageAccount cannot be null or empty.");
            }
        }
    }
}