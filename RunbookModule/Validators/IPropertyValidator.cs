namespace RunbookModule.Validators
{
    public interface IPropertyValidator
    {
        PropertyValidator GreaterThanOne(int value, string errorMessage);
        PropertyValidator GreaterThanZero(int value, string errorMessage);
        PropertyValidator GreaterOrEqualZero(int value, string errorMessage);
        PropertyValidator NotNullOrEmpty(string property, string errorMessage);
        PropertyValidator NotNull(object property, string errorMessage);
    }
}