namespace Streamberry.Rest.Models;

public class ReviewResponse
{
    public string User { get; set; } = null!;
    public int Rating { get; set; }
    public string? Comment { get; set; }
}