using System;
using System.Threading;

namespace RunbookModule.RetriesStrategies
{
    public class IncrementalDelayRetryStrategy : IRetryStrategy
    {
        private int _iteration = 1;
        private double _miliseconds;

        public IncrementalDelayRetryStrategy(TimeSpan delay)
        {
            _miliseconds = delay.TotalMilliseconds;
        }

        public void Invoke()
        {
            var delay = TimeSpan.FromMilliseconds(_miliseconds * _iteration);
            Thread.Sleep(delay);
            _iteration++;
        }
    }
}
