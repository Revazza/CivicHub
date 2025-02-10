namespace CivicHub.Application.Common.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> enumerable) => enumerable is not null && enumerable.Any();

    public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => enumerable is null || !enumerable.Any();
}