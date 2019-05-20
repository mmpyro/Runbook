using System;
using System.Collections.Generic;
using System.Management.Automation;
using RunbookModule.Helpers;
using RunbookModule.Loggers;

namespace RunbookModule.Wrappers
{
    public interface IPsWrapper : IDisposable
    {
        void AddScript(string script, bool useLocalScope);
        void AddParameters(object[] chapterArguments);
        void ClearStreams();
        void Invoke();
        PSInvocationState State { get; }
        IEnumerable<object> Error { get; }
        IEnumerable<object> Information { get; }
        bool HadErrors { get; }
        void SetLogger(ILogger logger, string sectionName, string chapterName);
    }

    public class IpsWrapper : IPsWrapper
    {
        private readonly PowerShell _ps;

        public IpsWrapper()
        {
            _ps = PowerShell.Create(RunspaceMode.NewRunspace);
        }

        public IEnumerable<object> Error => _ps?.Streams?.Error;
        public IEnumerable<object> Information => _ps?.Streams?.Information;
        public bool HadErrors => _ps.HadErrors;

        public void SetLogger(ILogger logger, string sectionName, string chapterName)
        {
            _ps.Streams.Information.DataAdding += (s, e) =>
            {
                StreamDataAdding(logger, sectionName, chapterName, e);
            };

            _ps.Streams.Error.DataAdding += (s, e) =>
            {
                StreamDataAdding(logger, sectionName, chapterName, e);
            };

            _ps.Streams.Debug.DataAdding += (s, e) =>
            {
                StreamDataAdding(logger, sectionName, chapterName, e);
            };

            _ps.Streams.Warning.DataAdding += (s, e) =>
            {
                StreamDataAdding(logger, sectionName, chapterName, e);
            };

            _ps.Streams.Verbose.DataAdding += (s, e) =>
            {
                StreamDataAdding(logger, sectionName, chapterName, e);
            };

            _ps.Streams.Progress.DataAdding += (s, e) =>
            {
                StreamDataAdding(logger, sectionName, chapterName, e);
            };
        }

        private static void StreamDataAdding(ILogger logger, string sectionName, string chapterName, DataAddingEventArgs e)
        {
            string message = e.ItemAdded.ToString();
            string taskName = PathHelper.RemoveInvalidChars($"{sectionName}_{chapterName}");
            logger?.Log(taskName, message);
        }

        public void Dispose()
        {
            _ps?.Dispose();
        }

        public void AddScript(string script, bool useLocalScope)
        {
            _ps?.AddScript(script, useLocalScope);
        }

        public void AddParameters(object[] chapterArguments)
        {
            _ps?.AddParameters(chapterArguments);
        }

        public void ClearStreams()
        {
            _ps?.Streams?.ClearStreams();
        }

        public void Invoke()
        {
            _ps?.Invoke();
        }

        public PSInvocationState State => _ps.InvocationStateInfo.State;
    }
}