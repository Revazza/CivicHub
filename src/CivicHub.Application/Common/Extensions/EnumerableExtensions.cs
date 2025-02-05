namespace CivicHub.Application.Common.Extensions;

public static class EnumerableExtensions
{
    public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable) => enumerable.Any();
    
    public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => !enumerable.Any();
}