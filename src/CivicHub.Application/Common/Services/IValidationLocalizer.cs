namespace CivicHub.Application.Common.Services;

public interface IValidationLocalizer
{
    string Translate(string key, params object[] arguments);
}