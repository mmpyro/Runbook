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
            var psWrapperFactory = ContainerProvider.Resolve<IPsWrapperFactory>();
            return new Chapter(name, action, psWrapperFactory, ignoreErrorStream);
        }

        public IChapter Create(string name, ScriptBlock action, IRetryStrategy retryStrategy, bool ignoreErrorStream = false)
        {
            var psWrapperFactory = ContainerProvider.Resolve<IPsWrapperFactory>();
            return new Chapter(name, action, psWrapperFactory, retryStrategy, ignoreErrorStream);
        }

        public IChapter Create(string name, object[] arguments, ScriptBlock action, bool ignoreErrorStream = false)
        {
            var psWrapperFactory = ContainerProvider.Resolve<IPsWrapperFactory>();
            return new Chapter(name, arguments, action, psWrapperFactory, ignoreErrorStream);
        }

        public IChapter Create(string name, object[] arguments, ScriptBlock action, IRetryStrategy retryStrategy, bool ignoreErrorStream = false)
        {
            var psWrapperFactory = ContainerProvider.Resolve<IPsWrapperFactory>();
            return new Chapter(name, arguments, action, psWrapperFactory, retryStrategy, ignoreErrorStream);
        }
    }
}
