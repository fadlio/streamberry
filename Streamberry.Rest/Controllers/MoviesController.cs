using System.Net.Mime;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streamberry.Context;
using Streamberry.Context.Models;
using Streamberry.Rest.Models;

namespace Streamberry.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController
{
    private readonly ILogger<MoviesController> _logger;
    private readonly EfContext _dbCtx;

    public MoviesController(ILogger<MoviesController> logger, EfContext dbCtx)
    {
        _logger = logger;
        _dbCtx = dbCtx;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<MovieResponse>>> GetMoviesAsync([FromQuery] Pagination pagination)
    {
        var result = await _dbCtx.Movies
            .Select(m => new MovieResponse
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseDate = m.ReleaseDate,
                Genres = m.Genres.Select(g => g.Name),
                Providers = m.Providers.Select(p => p.Name),
                // https://stackoverflow.com/questions/1287146/handling-null-results-with-the-linq-average-method
                Rating = m.Reviews.Select(r => (int?)r.Rating).Average() ?? 0
            })
            .OrderBy(m => m.Id)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
        return new PaginatedResponse<MovieResponse>(result, pagination);
    }

    // TODO: Make pagination work on the sql server
    [HttpGet("genre/{id:int}")]
    public async Task<Results<NotFound, Ok<PaginatedResponse<MovieResponse>>>> GetMoviesByGenderAsync(int id,
        [FromQuery] Pagination pagination)
    {
        var genre = await _dbCtx.Genres
            .Include(g => g.Movies)
            .ThenInclude(m => m.Providers)
            .Include(g => g.Movies)
            .ThenInclude(m => m.Genres)
            .Include(g => g.Movies)
            .ThenInclude(m => m.Reviews)
            .ThenInclude(r => r.User)
            .FirstOrDefaultAsync(g => g.Id == id);
        if (genre == null)
        {
            return TypedResults.NotFound();
        }

        var result = genre.Movies
            .Select(m => new MovieResponse(m))
            .OrderBy(m => m.Id)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();
        return TypedResults.Ok(new PaginatedResponse<MovieResponse>(result, pagination));
    }

    [HttpGet("year/{year:int}")]
    public async Task<ActionResult<PaginatedResponse<MovieResponse>>> GetMoviesByYear(int year,
        [FromQuery] Pagination pagination)
    {
        var response = await _dbCtx.Movies
            .Where(m => m.ReleaseDate.Year == year)
            .Select(m => new MovieResponse
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseDate = m.ReleaseDate,
                Genres = m.Genres.Select(g => g.Name),
                Providers = m.Providers.Select(p => p.Name),
                // https://stackoverflow.com/questions/1287146/handling-null-results-with-the-linq-average-method
                Rating = m.Reviews.Select(r => (int?)r.Rating).Average() ?? 0
            })
            .OrderBy(m => m.Id)
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PaginatedResponse<MovieResponse>(response, pagination);
    }

    [HttpGet("{id:int}")]
    public async Task<Results<NotFound, Ok<MovieResponse>>> GetMovieByIdAsync(int id)
    {
        var response = await _dbCtx.Movies.Select(m => new MovieResponse
        {
            Id = m.Id,
            Title = m.Title,
            ReleaseDate = m.ReleaseDate,
            Genres = m.Genres.Select(g => g.Name),
            Providers = m.Providers.Select(p => p.Name),
            // https://stackoverflow.com/questions/1287146/handling-null-results-with-the-linq-average-method
            Rating = m.Reviews.Select(r => (int?)r.Rating).Average() ?? 0
        }).FirstOrDefaultAsync(m => m.Id == id);
        if (response == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(response);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<Results<BadRequest, Created<MovieResponse>>> CreateMovieAsync(
        [FromBody] CreateMovieRequest request)
    {
        // https://stackoverflow.com/questions/35011656/async-await-in-linq-select
        var genres = request.Genres
            .Select(async gn =>
                await _dbCtx.Genres.FirstOrDefaultAsync(g => g.Name == gn) ?? new Genre { Name = gn })
            .Select(t => t.Result)
            .ToList();

        var providers = request.Providers
            .Select(async pn =>
                await _dbCtx.Providers.FirstOrDefaultAsync(p => p.Name == pn) ?? new Provider { Name = pn })
            .Select(t => t.Result)
            .ToList();

        var movie = new Movie
        {
            Title = request.Title,
            ReleaseDate = request.ReleaseDate,
            Genres = genres,
            Providers = providers,
        };

        var res = _dbCtx.Movies.Add(movie);
        await _dbCtx.SaveChangesAsync();
        return TypedResults.Created($"/movies/{res.Entity.Id}", new MovieResponse(res.Entity));
    }

    [HttpDelete("{id:int}")]
    public async Task<Results<NotFound, Ok>> DeleteMovieAsync(int id)
    {
        var movie = await _dbCtx.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return TypedResults.NotFound();
        }

        _dbCtx.Movies.Remove(movie);
        await _dbCtx.SaveChangesAsync();

        return TypedResults.Ok();
    }

    [HttpPatch("{id:int}")]
    [Consumes(MediaTypeNames.Application.Json)]
    public async Task<Results<BadRequest, Created<MovieResponse>>> UpdateMovieAsync(int id,
        [FromBody] UpdateMovieRequest request)
    {
        var movie = await _dbCtx.Movies
            .Include(m => m.Genres)
            .Include(m => m.Providers)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return TypedResults.BadRequest();
        }

        // Explicitly call Clear and SaveChanges to properly update many-to-many relations
        if (request.Genres != null || request.Providers != null)
        {
            if (request.Genres != null) movie.Genres.Clear();
            if (request.Providers != null) movie.Providers.Clear();
            await _dbCtx.SaveChangesAsync();
        }

        // https://stackoverflow.com/questions/35011656/async-await-in-linq-select
        var genres = (request.Genres ?? new List<string>())
            .Select(async gn =>
                await _dbCtx.Genres.FirstOrDefaultAsync(g => g.Name == gn) ?? new Genre { Name = gn })
            .Select(t => t.Result)
            .ToList();

        var providers = (request.Providers ?? new List<string>())
            .Select(async pn =>
                await _dbCtx.Providers.FirstOrDefaultAsync(p => p.Name == pn) ?? new Provider { Name = pn })
            .Select(t => t.Result)
            .ToList();

        movie.Title = request.Title;
        movie.ReleaseDate = request.ReleaseDate;
        movie.Genres = genres;
        movie.Providers = providers;

        await _dbCtx.SaveChangesAsync();
        return TypedResults.Created("asd", new MovieResponse(movie));
    }
}