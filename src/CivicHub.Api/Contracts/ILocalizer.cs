namespace CivicHub.Api.Contracts;

public interface ILocalizer<T>
{
    string Translate(string key, params object[] arguments);
}