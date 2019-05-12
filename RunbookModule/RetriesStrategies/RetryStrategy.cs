namespace RunbookModule.RetriesStrategies
{
    public static class RetryStrategy
    {
        public static IRetryStrategy Immediate => new ImmediateRetryStrategy();
    }
}
