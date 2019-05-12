using System.Management.Automation;

namespace RunbookModuleTests.Helpers
{
    public static class ScriptBlockHelper
    {
        public static ScriptBlock CreateScriptBlock()
        {
            return ScriptBlock.Create("Chapter cannot be null");
        }
    }
}