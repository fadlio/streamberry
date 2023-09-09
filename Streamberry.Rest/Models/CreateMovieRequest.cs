namespace Streamberry.Rest.Models;

public class CreateMovieRequest
{
    public string Title { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    
    public IEnumerable<string> Genres { get; set; } = new List<string>();
    public IEnumerable<string> Providers { get; set; } = new List<string>();
}