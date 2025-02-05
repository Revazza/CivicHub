namespace CivicHub.Application.Common.Extensions;

public static class DatetimeExtensions
{
    public static bool IsNotDefault(this DateTime dateTime) => dateTime != default;
    
    public static bool IsDefault(this DateTime dateTime) => dateTime == default;
}