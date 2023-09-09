using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using Streamberry.Context;
using Streamberry.Rest.Controllers;
using Streamberry.Rest.Models;

namespace Streamberry.Rest.Test;

public class MoviesControllerTests
{
    private MoviesController _controller = null!;
    private EfContext _ctx = null!;

    [SetUp]
    public void Setup()
    {
        var logger = Mock.Of<ILogger<MoviesController>>();
        _ctx = new TestContextFactory().CreateDbContext(new[] { "" });
        EfContext.GenerateMockData(_ctx);
        _controller = new MoviesController(logger, _ctx);
    }

    [Test]
    public async Task GetMoviesTest()
    {
        var res = await _controller.GetMoviesAsync(new Pagination
        {
            PageSize = 25
        });
        Assert.That(res.Value!.Result.Count(), Is.EqualTo(25));
    }

    [TestCase(1, ExpectedResult = 1)]
    [TestCase(2, ExpectedResult = 2)]
    public async Task<int> GetMovieByIdTest(int id)
    {
        var res = await _controller.GetMovieByIdAsync(id);
        return res.Result switch
        {
            Ok<MovieResponse> ok => ok.Value!.Id,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [Test]
    public async Task CreateMovie()
    {
        var genres = new[] { "Genero1", "Genero2", "Bar" };
        var providers = new[] { "Provedor1", "Foo" };

        var res = await _controller.CreateMovieAsync(new CreateMovieRequest
        {
            Title = "NovoFilme",
            ReleaseDate = DateOnly.FromDateTime(DateTime.Now),
            Providers = providers,
            Genres = genres,
        });
        switch (res.Result)
        {
            case Created<MovieResponse> created:
                Assert.Multiple(() =>
                {
                    Assert.That(created.Value!.Id, Is.EqualTo(101));
                    Assert.That(created.Value!.Genres, Is.EqualTo(genres));
                    Assert.That(created.Value!.Providers, Is.EqualTo(providers));
                });
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [Test]
    public async Task UpdateMovie()
    {
        var genres = new[] { "Genero1", "Genero2", "Bar" };
        var providers = new[] { "Provedor1", "Foo" };

        var res = await _controller.UpdateMovieAsync(1, new UpdateMovieRequest
        {
            Title = "NovoFilme",
            ReleaseDate = DateOnly.FromDateTime(DateTime.Now),
            Providers = providers,
            Genres = genres,
        });
        switch (res.Result)
        {
            case Created<MovieResponse> created:
                Assert.Multiple(() =>
                {
                    Assert.That(created.Value!.Id, Is.EqualTo(1));
                    Assert.That(created.Value!.Genres, Is.EqualTo(genres));
                    Assert.That(created.Value!.Providers, Is.EqualTo(providers));
                });
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    [Test]
    public async Task DeleteMovie()
    {
        await _controller.DeleteMovieAsync(1);
        Assert.That(_ctx.Movies.Count(), Is.EqualTo(99));
    }

    [Test]
    public async Task GetMoviesByGenre()
    {
        var res = await _controller.GetMoviesByGenderAsync(1, new Pagination
        {
            PageSize = 25
        });
        switch (res.Result)
        {
            case Ok<PaginatedResponse<MovieResponse>> ok:
                Assert.That(ok.Value!.Result.Count(), Is.EqualTo(25));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
}