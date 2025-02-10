namespace CivicHub.Application.Common.Localization;

public interface IValidationLocalizer
{
    string Translate(string key, params object[] arguments);
}