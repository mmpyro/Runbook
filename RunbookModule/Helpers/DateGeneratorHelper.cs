using System;

namespace RunbookModule.Helpers
{
    public static class DateGeneratorHelper
    {
        public static string CurrentDateString()
        {
            var dt = DateTime.Now;
            return $"{dt.Day}_{dt.Month}_{dt.Year}_{dt.Hour}_{dt.Minute}_{dt.Second}";
        }
    }
}