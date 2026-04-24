namespace RBC_consulting.Domain.Common.Paginations;

public sealed class PagedList<TItem>
{
    public const int DefaultPageSize = 8;
    public const int MaxPageSize = 128;

    public IReadOnlyList<TItem> Items { get; private set; } = [];
    public int PageNumber { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalCount { get; private set; }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PagedList() { }

    public PagedList(IEnumerable<TItem> items, int count, int pageNumber, int pageSize = DefaultPageSize)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than 0.");
        if (pageSize > MaxPageSize)
            throw new ArgumentOutOfRangeException(nameof(pageSize), $"Page size maximum {MaxPageSize}.");
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Total count cannot be negative.");

        Items = items.ToList();
        TotalCount = count;
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    public static PagedList<TItem> Empty(int pageNumber = 1, int pageSize = DefaultPageSize) =>
        new([], 0, pageNumber, pageSize);
}
