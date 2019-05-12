using RunbookModule.RetriesStrategies;
using System.Management.Automation;

namespace RunbookModule.Factories
{
    public interface IChapterFactory
    {
        IChapter Create(string name, object[] arguments, ScriptBlock action, bool ignoreErrorStream = false);
        IChapter Create(string name, ScriptBlock action, bool ignoreErrorStream = false);
        IChapter Create(string name, ScriptBlock action, IRetryStrategy retryStrategy, bool ignoreErrorStream = false);
        IChapter Create(string name, object[] arguments, ScriptBlock action, IRetryStrategy retryStrategy, bool ignoreErrorStream = false);
    }
}