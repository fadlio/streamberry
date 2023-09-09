namespace Streamberry.Context.Models;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }

    public int UserId { get; set; }
    public int MovieId { get; set; }
    
    public User User { get; set; } = null!;
    public Movie Movie { get; set; } = null!;
}