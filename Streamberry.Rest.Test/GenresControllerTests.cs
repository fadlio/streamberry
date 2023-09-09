using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using Streamberry.Context;
using Streamberry.Rest.Controllers;
using Streamberry.Rest.Models;

namespace Streamberry.Rest.Test;

public class GenresControllerTests
{
    private GenresController _controller = null!;
    private EfContext _efCtx = null!;
    private DapperContext _dpCtx = null!;

    [SetUp]
    public void Setup()
    {
        var logger = Mock.Of<ILogger<GenresController>>();
        _efCtx = new TestContextFactory().CreateDbContext(new[] { "" });
        EfContext.GenerateMockData(_efCtx);

        _dpCtx = new DapperContext();
        
        _controller = new GenresController(logger, _efCtx, _dpCtx);
    }

    [Test]
    public async Task GetAvgRatingByYearTest()
    {
        var res = await _controller.GetAvgRatingByYearAsync(new Pagination
        {
            PageSize = 25
        });
        Assert.That(res.Value!.TotalFetched, Is.EqualTo(25));
    }
}