using System;

namespace RunbookModule.Validators
{
    public class PropertyValidator : IPropertyValidator
    {
        public PropertyValidator NotNullOrEmpty(string property, string errorMessage)
        {
            if(string.IsNullOrEmpty(property) || string.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentException(errorMessage);
            }
            return this;
        }

        public PropertyValidator GreaterThanZero(int value, string errorMessage)
        {
            if(value < 1)
            {
                throw new ArgumentException(errorMessage);
            }
            return this;
        }

        public PropertyValidator GreaterThanOne(int value, string errorMessage)
        {
            if (value < 2)
            {
                throw new ArgumentException(errorMessage);
            }
            return this;
        }

        public PropertyValidator GreaterOrEqualZero(int value, string errorMessage)
        {
            if (value < 0)
            {
                throw new ArgumentException(errorMessage);
            }
            return this;
        }

        public PropertyValidator NotNull(object property, string errorMessage)
        {
            if (property == null)
            {
                throw new ArgumentException(errorMessage);
            }
            return this;
        }
    }
}
