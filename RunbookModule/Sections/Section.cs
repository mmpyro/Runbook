using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RunbookModule.Loggers;
using RunbookModule.Report;

namespace RunbookModule.Sections
{
    public abstract class Section : ISection
    {
        private const string ErrorMessage = "Chapter cannot be null";
        protected readonly List<IChapter> Chapters = new List<IChapter>();
        protected readonly Stopwatch Sw = new Stopwatch();

        protected Section(string sectionName)
        {
            SectionName = sectionName;
            ChaptersExecutionInfos = new List<ChapterExecutionInfo>();
        }

        public List<ChapterExecutionInfo> ChaptersExecutionInfos { get; }

        public void Add(IChapter chapter)
        {
            if (chapter == null)
                throw new ArgumentException($"{ErrorMessage}. Section: {SectionName}.");
            if(Chapters.Any(t => t.Name.Equals(chapter.Name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"Section: {SectionName}, Chapter {chapter.Name} already exists in that section.");
            Chapters.Add(chapter);
        }

        public void AddRange(IEnumerable<IChapter> chapters)
        {
            if (chapters.Any(t => t == null))
                throw new ArgumentException($"{ErrorMessage}. Section: {SectionName}.");
            if (chapters.GroupBy(t => t.Name.ToLower()).Count() != chapters.Count())
            {
                string collisionNames = string.Join(",", chapters.Select(t => t.Name.ToLower()).GroupBy(t => t).Where(g => g.Count() > 1).Select(t => t.Key));
                throw new ArgumentException($"Section: {SectionName}, Chapters {collisionNames} are not unique inside section.");
            }
            if (chapters.Any(t => Chapters.Any(ch => ch.Name.Equals(t.Name, StringComparison.OrdinalIgnoreCase))))
            {
                string collisionNames = string.Join(",", chapters.Select(t => t.Name).Intersect(Chapters.Select(t => t.Name)).ToArray());
                throw new ArgumentException($"Section: {SectionName}, Chapters {collisionNames} are not unique inside section.");
            }
            Chapters.AddRange(chapters);
        }

        public void Remove(IChapter chapter)
        {
            if (chapter == null)
                throw new ArgumentException(ErrorMessage);
            Chapters.Remove(chapter);
        }

        public int Size => Chapters.Count;

        public string SectionName { get;  protected set; }

        public abstract StatusCode Invoke(ILogger logger);

        public double OverallExecutionSeconds => Sw.Elapsed.TotalSeconds;

        public virtual StatusCode StatusCode
            => ChaptersExecutionInfos.Any(t => t.StatusCode == StatusCode.Fail) ? StatusCode.Fail : StatusCode.Success;
    }
}