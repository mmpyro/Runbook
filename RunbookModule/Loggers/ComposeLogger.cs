using Ninject;
using RunbookModule.Constants;
using System.Collections.Generic;

namespace RunbookModule.Loggers
{
    public class ComposeLogger : ILogger
    {
        private readonly List<ILogger> _loggers = new List<ILogger>();

        public ComposeLogger([Named(ContainerConstants.LiveLogger)]ILogger liveLogger, [Named(ContainerConstants.FileLogger)]ILogger fileLogger)
        {
            _loggers.Add(liveLogger);
            _loggers.Add(fileLogger);
        }

        public void Log(string msg)
        {
            _loggers.ForEach(l => l.Log(msg));
        }

        public void Log(string sectionName, string msg)
        {
            _loggers.ForEach(l => l.Log(sectionName, msg));
        }
    }
}
