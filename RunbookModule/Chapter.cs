using RunbookModule.Dtos;
using RunbookModule.Loggers;
using RunbookModule.Report;
using RunbookModule.RetriesStrategies;
using RunbookModule.Wrappers;
using System;
using System.Diagnostics;
using System.Management.Automation;
using static RunbookModule.Helpers.FunctionHelper;


namespace RunbookModule
{
    public interface IChapter
    {
        void SetNumberOfRetries(int numberOfRetries);
        void SetRetryStrategy(IRetryStrategy retryStrategy);
        ChapterExecutionInfo Invoke(string sectionName, ILogger logger);
        string Name { get; }
    }

    public class Chapter : IChapter
    {
        private readonly IPsWrapperFactory _factory;
        private int iteration = 0;
        private int _numberOfRetries;
        private readonly bool _ignoreErrorStream;
        private IRetryStrategy _retryStrategy;
        private readonly object[] _arguments = new object[0];
        private string _name;
        private ScriptBlock _action;

        public string Name => _name;

        public Chapter(string name, object[] arguments, ScriptBlock action, IPsWrapperFactory factory, bool ignoreErrorStream = false) 
            :this(name, action, factory, ignoreErrorStream)
        {
            _arguments = arguments;
        }

        public Chapter(string name, object[] arguments, ScriptBlock action, IPsWrapperFactory factory, IRetryStrategy retryStrategy, bool ignoreErrorStream = false) 
            :this(name, arguments, action, factory, ignoreErrorStream)
        {
            _retryStrategy = retryStrategy;
        }

        public Chapter(string name, ScriptBlock action, IPsWrapperFactory factory, IRetryStrategy retryStrategy, bool ignoreErrorStream = false)
             :this(name, action, factory, ignoreErrorStream)
        {
            _retryStrategy = retryStrategy;
        }

        public Chapter(string name, ScriptBlock action, IPsWrapperFactory factory, bool ignoreErrorStream = false)
        {
            _name = name;
            _action = action;
            _numberOfRetries = 0;
            _ignoreErrorStream = ignoreErrorStream;
            _factory = factory;
            if(_retryStrategy == null)
            {
                _retryStrategy = RetryStrategy.Immediate;
            }
        }

        public void SetNumberOfRetries(int numberOfRetries)
        {
            _numberOfRetries = numberOfRetries;
        }

        public override bool Equals(object obj)
        {
            var chapter = obj as Chapter;
            return chapter != null && _name.Equals(chapter.Name);
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public ChapterExecutionInfo Invoke(string sectionName, ILogger logger)
        {
            var chapetrStopWatch = new Stopwatch();
            chapetrStopWatch.Start();
            PsStatusDto psStatus;
            do
            {
                using (var ps = _factory.Create())
                {
                    string errorMessage = null;
                    try
                    {
                        SetupCommonFunctions(ps);
                        ps.AddScript(_action.ToString(), false);
                        ps.AddParameters(_arguments);
                        ps.ClearStreams();
                        ps.SetLogger(logger, sectionName, _name);
                        ps.Invoke();
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                    psStatus = new PsStatusDto(ps) { Error = errorMessage };
                }
            } while (IsRetryNecessary(psStatus));
            chapetrStopWatch.Stop();
            var status = GetStatus(psStatus);
            return new ChapterExecutionInfo
            {
                StatusCode = status,
                ErrorMessage = psStatus.Error,
                ExecutionTime = chapetrStopWatch.Elapsed,
                Retries = iteration,
                Name = _name
            };
        }

        public void SetRetryStrategy(IRetryStrategy retryStrategy)
        {
            _retryStrategy = retryStrategy;
        }

        private static void SetupCommonFunctions(IPsWrapper ps)
        {
            ps.AddScript(CheckOperationStatus(), false);
        }

        private StatusCode GetStatus(PsStatusDto psStatus)
        {
            if (_ignoreErrorStream)
                return psStatus.State == PSInvocationState.Completed
                    ? StatusCode.Success
                    : StatusCode.Fail;
            return psStatus.HadErrors ? StatusCode.Fail : StatusCode.Success;
        }

        private bool IsRetryNecessary(PsStatusDto psStatus)
        {
            bool retry;
            if (_ignoreErrorStream)
            {
                retry = (psStatus.State != PSInvocationState.Completed) && (iteration++ < _numberOfRetries);
            }
            else
            {
                retry = (iteration++ < _numberOfRetries) && psStatus.HadErrors;
            }

            if(retry)
            {
                _retryStrategy.Invoke();
            }
            return retry;
        }
    }
}
