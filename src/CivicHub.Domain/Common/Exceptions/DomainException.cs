namespace CivicHub.Domain.Common.Exceptions;

public abstract class DomainException : Exception
{
    public object[] LocalizationArguments { get; set; }

    public DomainException(string messageKey, params object[] localizationArguments) : base(messageKey)
    {
        LocalizationArguments = localizationArguments;
    }
}