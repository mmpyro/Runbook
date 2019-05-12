namespace RunbookModule.RetriesStrategies
{
    public interface IRetryStrategy
    {
        void Invoke();
    }
}