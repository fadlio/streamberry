using Dapper;
using Microsoft.AspNetCore.Mvc;
using Streamberry.Context;
using Streamberry.Rest.Models;

namespace Streamberry.Rest.Controllers;

public class GenresController
{
    private readonly ILogger<GenresController> _logger;
    private readonly EfContext _efCtx;
    private readonly DapperContext _dpCtx;

    public GenresController(ILogger<GenresController> logger, EfContext efCtx, DapperContext dpCtx)
    {
        _efCtx = efCtx;
        _dpCtx = dpCtx;
        _logger = logger;
    }

    [HttpGet("avgRatingByYear")]
    public async Task<ActionResult<PaginatedResponse<GenreAvgRatingByYearResponse>>> GetAvgRatingByYearAsync(
        [FromQuery] Pagination pagination)
    {
        const string query = """
                             select
                             	g.Name as "Genre",
                             	cast(strftime('%Y', m.ReleaseDate) as integer) AS "Year",
                             	coalesce(avg(r.Rating), 0.0) as "Rating"
                             from
                             	Genres g
                             inner join GenreMovie gm on
                             	gm.GenresId = g.Id
                             inner join Movies m on
                             	m.Id = gm.MoviesId
                             left join Reviews r on
                             	r.MovieId = gm.MoviesId
                             group by
                             	g.Name,
                             	strftime('%Y', m.ReleaseDate)
                             limit @PageSize
                             offset @Skip
                             """;
        using var conn = _dpCtx.CreateConnection();
        var res = await conn.QueryAsync<GenreAvgRatingByYearResponse>(query, new
        {
	        pagination.PageSize,
	        Skip = pagination.PageSize * (pagination.PageNumber - 1)
        });
        return new PaginatedResponse<GenreAvgRatingByYearResponse>(res, pagination);
    }
}