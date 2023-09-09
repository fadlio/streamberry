using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streamberry.Context;
using Streamberry.Context.Models;
using Streamberry.Rest.Models;

namespace Streamberry.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewsController
{
    private readonly ILogger<ReviewsController> _logger;
    private readonly EfContext _dbCtx;

    public ReviewsController(ILogger<ReviewsController> logger, EfContext dbCtx)
    {
        _logger = logger;
        _dbCtx = dbCtx;
    }

    [HttpPost]
    public async Task<Results<NotFound, Created<ReviewResponse>>> CreateReviewAsync(
        [FromBody] CreateReviewRequest request)
    {
        var user = await _dbCtx.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
        if (user == null)
        {
            return TypedResults.NotFound();
        }

        var movie = await _dbCtx.Movies.FirstOrDefaultAsync(m => m.Id == request.MovieId);
        if (movie == null)
        {
            return TypedResults.NotFound();
        }

        var review = _dbCtx.Reviews.Add(new Review
        {
            User = user,
            Rating = request.Rating,
            Comment = request.Comment,
        });
        movie.Reviews.Add(review.Entity);

        await _dbCtx.SaveChangesAsync();

        return TypedResults.Created($"/review/{review.Entity.Id}", new ReviewResponse(review.Entity));
    }

    [HttpGet("{id:int}")]
    public async Task<Results<NotFound, Ok<ReviewResponse>>> GetReviewById(int id)
    {
        var review = await _dbCtx.Reviews.FirstOrDefaultAsync(u => u.Id == id);
        if (review == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(new ReviewResponse(review));
    }
}