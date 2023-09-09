using Microsoft.EntityFrameworkCore;
using Streamberry.Context;
using Streamberry.Context.Models;

namespace Streamberry.Gql.Resolvers;

public class Query
{
    public async Task<List<Movie>> GetMoviesAsync(EfContext dbContext, CancellationToken ct) =>
        await dbContext.Movies.ToListAsync(ct);
}