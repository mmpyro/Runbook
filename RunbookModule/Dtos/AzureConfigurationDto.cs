using System.Management.Automation;

namespace RunbookModule.Dtos
{
    public class AzureConfigurationDto
    {
        public string SubscriptionId { get; set; }
        public string CurrentStorageAccount { get; set; }
        public PSCredential Credentials { get; set; }
    }
}