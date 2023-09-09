namespace Streamberry.Rest.Models;

public class PaginatedResponse<T>
{
    public PaginatedResponse(IEnumerable<T> result, Pagination pagination)
    {
        PageNumber = pagination.PageNumber;
        PageSize = pagination.PageSize;
        Result = result;
        TotalFetched = Result.Count();
    }
    
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalFetched { get; set; }
    public IEnumerable<T> Result { get; set; }
}