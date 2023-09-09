using Microsoft.EntityFrameworkCore;

namespace Streamberry.Context.Models;

[Index(nameof(Name), IsUnique = true)]
public class Provider
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}