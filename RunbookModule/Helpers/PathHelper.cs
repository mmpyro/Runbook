using System.IO;
using System.Text;

namespace RunbookModule.Helpers
{
    public static class PathHelper
    {
        public static string RemoveInvalidChars(string fileName)
        {
            StringBuilder sb = new StringBuilder(fileName);
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                sb.Replace(c.ToString(), "");
            }
            return sb.ToString();
        }
    }
}