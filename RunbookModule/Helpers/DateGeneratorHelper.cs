using System;

namespace RunbookModule.Helpers
{
    public static class DateGeneratorHelper
    {
        public static string CurrentDateString()
        {
            var dt = DateTime.Now;
            return $"[{dt.Day}-{dt.Month}-{dt.Year}_{dt.Hour}:{dt.Minute}:{dt.Second}]";
        }
    }
}