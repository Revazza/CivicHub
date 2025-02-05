namespace CivicHub.Application.Common.Extensions;

public static class StringExtensions
{
    public static string Capitalize(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return str;

        return char.ToUpper(str[0]) + str[1..];
    }

    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);
    
    public static bool IsNotNullOrEmpty(this string str) => !string.IsNullOrEmpty(str);
}