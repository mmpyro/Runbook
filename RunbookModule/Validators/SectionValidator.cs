using System;
using System.Collections.Generic;
using System.Linq;
using RunbookModule.Constants;
using RunbookModule.Sections;

namespace RunbookModule.Validators
{
    public interface ISectionValidator
    {
        void Validate(IEnumerable<ISection> runbookSections, ISection section);
        void Validate(IEnumerable<ISection> runbookSections, IEnumerable<ISection> sections);
    }

    public class SectionValidator : ISectionValidator
    {
        public void Validate(IEnumerable<ISection> runbookSections, ISection section)
        {
            if (IsInValidSection(section))
            {
                throw new ArgumentException(ErrorMessages.SectionNullErrorMessage);
            }
            if (runbookSections.Any(t => t.SectionName.Equals(section.SectionName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"Section: {section.SectionName} already exists in that runbook.");
            }
        }

        public void Validate(IEnumerable<ISection> runbookSections, IEnumerable<ISection> sections)
        {
            if (sections.Any(IsInValidSection))
            {
                throw new ArgumentException(ErrorMessages.SectionNullErrorMessage);
            }
            if(sections.GroupBy(t => t.SectionName).Any(g => g.Count() > 1))
            {
                var collisionName = sections.GroupBy(t => t.SectionName).Where(g => g.Count() > 1).Select(g => g.Key);
                throw new ArgumentException($"Sections {collisionName} are not unique inside runbook.");
            }
            if (runbookSections.Any(t => sections.Any(section => section.SectionName.Equals(t.SectionName, StringComparison.OrdinalIgnoreCase))))
            {
                string collisionNames = string.Join(",", sections.Select(t => t.SectionName).Intersect(runbookSections.Select(t => t.SectionName)).ToArray());
                throw new ArgumentException($"Sections {collisionNames} are not unique inside runbook.");
            }
        }

        private static bool IsInValidSection(ISection section)
        {
            return section == null || section.Size == 0;
        }
    }
}