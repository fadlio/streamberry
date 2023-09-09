using Streamberry.Context.Models;

namespace Streamberry.Rest.Models;

public class MovieResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    public double Rating { get; set; }

    public IEnumerable<string> Genres { get; set; } = new List<string>();
    public IEnumerable<string> Providers { get; set; } = new List<string>();
    public IEnumerable<ReviewResponse> Reviews { get; set; } = new List<ReviewResponse>();

    public MovieResponse(Movie m)
    {
        Id = m.Id;
        Title = m.Title;
        ReleaseDate = m.ReleaseDate;
        Genres = m.Genres.Select(g => g.Name);
        Providers = m.Providers.Select(p => p.Name);
        Reviews = m.Reviews.Select(r => new ReviewResponse
        {
            User = r.User.Name,
            Comment = r.Comment,
            Rating = r.Rating
        });
        
        if (m.Reviews.Any())
            Rating = m.Reviews.Select(r => r.Rating).Average();
    }

    public MovieResponse()
    {
    }
}