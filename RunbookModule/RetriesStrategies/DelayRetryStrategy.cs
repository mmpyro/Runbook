using System;
using System.Threading;

namespace RunbookModule.RetriesStrategies
{
    public class DelayRetryStrategy : IRetryStrategy
    {
        private readonly TimeSpan delay;

        public DelayRetryStrategy(TimeSpan delay)
        {
            this.delay = delay;
        }

        public void Invoke()
        {
            Thread.Sleep(delay);
        }
    }
}
