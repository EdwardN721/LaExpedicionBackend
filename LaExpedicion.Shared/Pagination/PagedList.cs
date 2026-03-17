namespace LaExpedicion.Shared.Pagination;

public class PagedList<T> : List<T>
{
    public Metadata Metadata { get; set; }

    public PagedList(List<T> items, int count,
        int pageNumber, int pageSize)
    {
        Metadata = new Metadata
        {
            TotalCount = count,
            PageSize = pageSize,
            CurrentPage = pageNumber,
            TotalPages = (int)Math.Ceiling(count / (double)pageSize)
        };
        AddRange(items);
    }
}