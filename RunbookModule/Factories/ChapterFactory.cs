using RunbookModule.Constants;
using RunbookModule.Loggers;
using RunbookModule.Providers;
using RunbookModule.RetriesStrategies;
using RunbookModule.Wrappers;
using System.Management.Automation;

namespace RunbookModule.Factories
{
    public class ChapterFactory : IChapterFactory
    {
        public IChapter Create(string name, ScriptBlock action, bool ignoreErrorStream = false)
        {
            var logger = ContainerProvider.Resolve<ILogger>(ContainerConstants.CompositeLogger);
            var psWrapperFactory = ContainerProvider.Resolve<IPsWrapperFactory>();
            return new Chapter(name, action, psWrapperFactory, logger, ignoreErrorStream);
        }

        public IChapter Create(string name, ScriptBlock action, IRetryStrategy retryStrategy, bool ignoreErrorStream = false)
        {
            var logger = ContainerProvider.Resolve<ILogger>(ContainerConstants.CompositeLogger);
            var psWrapperFactory = ContainerProvider.Resolve<IPsWrapperFactory>();
            return new Chapter(name, action, psWrapperFactory, retryStrategy, logger, ignoreErrorStream);
        }

        public IChapter Create(string name, object[] arguments, ScriptBlock action, bool ignoreErrorStream = false)
        {
            var logger = ContainerProvider.Resolve<ILogger>(ContainerConstants.CompositeLogger);
            var psWrapperFactory = ContainerProvider.Resolve<IPsWrapperFactory>();
            return new Chapter(name, arguments, action, psWrapperFactory, logger, ignoreErrorStream);
        }

        public IChapter Create(string name, object[] arguments, ScriptBlock action, IRetryStrategy retryStrategy, bool ignoreErrorStream = false)
        {
            var logger = ContainerProvider.Resolve<ILogger>(ContainerConstants.CompositeLogger);
            var psWrapperFactory = ContainerProvider.Resolve<IPsWrapperFactory>();
            return new Chapter(name, arguments, action, psWrapperFactory, retryStrategy, logger, ignoreErrorStream);
        }
    }
}
