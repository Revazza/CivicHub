namespace CivicHub.Application.Common.Extensions;

public static class StringExtensions
{
    public static string Capitalize(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        return char.ToUpper(str[0]) + str[1..];
    }
}