using System.Text.RegularExpressions;

namespace CivicHub.Infrastructure.Helpers;

public static class FileHelper
{
    private const string InvalidFileNameRegexPattern = @"([^\w\-\.]*\.+$)|([^\w\-\.]+)";

    public static string SanitizeFileName(string name)
        => Regex.Replace(name, InvalidFileNameRegexPattern, "_");
}