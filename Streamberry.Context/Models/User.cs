namespace Streamberry.Context.Models;


public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Review> Reviews { get; } = new List<Review>();
}