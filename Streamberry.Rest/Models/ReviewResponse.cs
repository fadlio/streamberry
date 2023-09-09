using Streamberry.Context.Models;

namespace Streamberry.Rest.Models;

public class ReviewResponse
{
    public string User { get; set; } = null!;
    public int Rating { get; set; }
    public string? Comment { get; set; }

    public ReviewResponse(Review review)
    {
        User = review.User.Name;
        Rating = review.Rating;
        Comment = review.Comment;
    }

    public ReviewResponse()
    {
        
    }
}