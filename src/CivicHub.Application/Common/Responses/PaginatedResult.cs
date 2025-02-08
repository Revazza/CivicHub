namespace CivicHub.Application.Common.Responses;

public record PaginatedResult<T>(
    int TotalCount,
    int PageNumber,
    int PageSize,
    List<T> Items) where T : class
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}