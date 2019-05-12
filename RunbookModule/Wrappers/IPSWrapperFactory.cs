namespace RunbookModule.Wrappers
{
    public interface IPsWrapperFactory
    {
        IPsWrapper Create();
    }

    public class PsWrapperFactory : IPsWrapperFactory
    {
        public IPsWrapper Create()
        {
            return new IpsWrapper();
        }
    }

}