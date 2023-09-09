using Microsoft.EntityFrameworkCore;

namespace Streamberry.Context.Models;

[Index(nameof(Title), IsUnique = true)]
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateOnly ReleaseDate { get; set; }
    
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Provider> Providers { get; set; } = new List<Provider>();
}