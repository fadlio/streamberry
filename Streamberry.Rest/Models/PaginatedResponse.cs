namespace Streamberry.Rest.Models;

public class PaginatedResponse<T>
{
    public PaginatedResponse(IEnumerable<T> result, Pagination pagination)
    {
        Pagination = pagination;
        Result = result;
    }
    
    public Pagination Pagination { get; set; }
    public IEnumerable<T> Result { get; set; }
}