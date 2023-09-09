namespace Streamberry.Rest.Models;

public class GenreAvgRatingByYearResponse
{
    public string Genre { get; set; } = null!;
    public int Year { get; set; }
    public double Rating { get; set; }
}