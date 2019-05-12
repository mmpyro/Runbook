using RunbookModule.Wrappers;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace RunbookModule.Dtos
{
    public class PsStatusDto
    {
        private readonly IEnumerable<object> _errors;

        public PsStatusDto(IPsWrapper ps)
        {
            State = ps.State;
            HadErrors = ps.HadErrors;
            _errors = ps.Error;
        }

        public PSInvocationState State { get; set; }
        public bool HadErrors { get; set; }

        public string Error { get; set; }
        public string ErrorMessage => ReadError();

        private string ReadError()
        {
            return string.IsNullOrEmpty(Error) ? ReadOutput(_errors) : Error;
        }

        private static string ReadOutput<T>(IEnumerable<T> stream)
        {
            var sb = new StringBuilder();
            foreach (var info in stream)
                sb.Append(info);
            return sb.ToString();
        }
    }
}