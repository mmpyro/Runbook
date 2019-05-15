﻿using System.Collections.Generic;
using RunbookModule.Report;

namespace RunbookModule.Sections
{
    public interface ISection
    {
        void Add(IChapter chapter);
        void AddRange(IEnumerable<IChapter> chapters);
        void Remove(IChapter chapter);
        int Size { get; }
        string SectionName { get; }
        StatusCode StatusCode { get; }
        StatusCode Invoke();
        double OverallExecutionSeconds { get; }
        List<ChapterExecutionInfo> ChaptersExecutionInfos { get; }
    }
}