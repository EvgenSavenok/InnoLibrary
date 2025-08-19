namespace Application.RequestFeatures;

public static class PagingExtensions
{
    public static IQueryable<T> Paging<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }

        return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    public static IEnumerable<T> Paging<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
        {
            pageNumber = 1;
        }

        if (pageSize < 1)
        {
            pageSize = 10;
        }

        return source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}