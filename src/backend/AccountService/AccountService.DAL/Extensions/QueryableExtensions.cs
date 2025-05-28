namespace AccountService.DAL.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (pageNumber < 1)
        {
            throw new ArgumentException("Page number must be greater than or equal to 1.", nameof(pageNumber));
        }

        if (pageSize < 1)
        {
            throw new ArgumentException("Page size must be greater than or equal to 1.", nameof(pageSize));
        }

        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}