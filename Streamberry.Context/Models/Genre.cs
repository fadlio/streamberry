using Microsoft.EntityFrameworkCore;

namespace Streamberry.Context.Models;

[Index(nameof(Name), IsUnique = true)]
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Movie> Movies { get; } = new List<Movie>();
}