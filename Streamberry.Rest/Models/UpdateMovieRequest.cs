namespace Streamberry.Rest.Models;

public class UpdateMovieRequest
{
    public string Title { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    
    public IEnumerable<string>? Genres { get; set; }
    public IEnumerable<string>? Providers { get; set; }
}