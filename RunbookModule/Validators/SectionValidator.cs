using System;
using System.Collections.Generic;
using System.Linq;
using RunbookModule.Sections;

namespace RunbookModule.Validators
{
    public interface ISectionValidator
    {
        void Validate(ISection section);
        void Validate(IEnumerable<ISection> sections);
    }

    public class SectionValidator : ISectionValidator
    {
        private const string ErrorMessage = "Section cannot be null or empty";

        public void Validate(ISection section)
        {
            if (IsInValidSection(section))
            {
                throw new ArgumentException(ErrorMessage);
            }
        }

        public void Validate(IEnumerable<ISection> sections)
        {
            if (sections.Any(IsInValidSection))
            {
                throw new ArgumentException(ErrorMessage);
            }
        }

        private static bool IsInValidSection(ISection section)
        {
            return section == null || section.Size == 0;
        }
    }
}