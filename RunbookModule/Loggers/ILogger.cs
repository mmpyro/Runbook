﻿namespace RunbookModule.Loggers
{
    public interface ILogger
    {
        void Log(string msg);
        void Log(string sectionName, string msg);
    }
}