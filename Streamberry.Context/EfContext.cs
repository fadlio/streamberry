using Microsoft.EntityFrameworkCore;
using Streamberry.Context.Models;

namespace Streamberry.Context;

public class EfContext : DbContext
{
    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Provider> Providers { get; set; } = null!;

    public EfContext(DbContextOptions<EfContext> options)
        : base(options)
    {
    }

    public static void GenerateMockData(EfContext context)
    {
        var rng = new Random(0);
        
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        var providers = new List<Provider>();
        for (var i = 0; i < 3; i++)
        {
            providers.Add(new Provider { Name = $"Provedor{i + 1}" });
        }

        var genres = new List<Genre>();
        for (var i = 0; i < 10; i++)
        {
            genres.Add(new Genre { Name = $"Genero{i + 1}" });
        }

        var users = new List<User>();
        for (var i = 0; i < 15; i++)
        {
            users.Add(new User { Name = $"Usuario{i + 1}" });
        }

        for (var i = 0; i < 100; i++)
        {
            var reviews = users
                .OrderBy(_ => rng.Next())
                .Take(rng.Next(3))
                .Select(u => new Review
                {
                    User = u,
                    Rating = rng.Next(1, 6),
                });
            
            var movie = new Movie
            {
                ReleaseDate = new DateOnly(rng.Next(1960, 2024), rng.Next(1, 13), rng.Next(1, 28)),
                Title = $"Filme{rng.Next()}",
                Genres = genres
                    .OrderBy(_ => rng.Next())
                    .Take(rng.Next(1, genres.Count))
                    .ToList(),
                Providers = providers
                    .OrderBy(_ => rng.Next())
                    .Take(rng.Next(1, providers.Count))
                    .ToList(),
                Reviews = reviews.ToList()
            };
            context.Add(movie);
        }

        context.SaveChanges();
    }
}