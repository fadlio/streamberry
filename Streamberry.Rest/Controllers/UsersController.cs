using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streamberry.Context;
using Streamberry.Rest.Models;

namespace Streamberry.Rest.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController
{
    private readonly ILogger<UsersController> _logger;
    private readonly EfContext _dbCtx;

    public UsersController(ILogger<UsersController> logger, EfContext dbCtx)
    {
        _logger = logger;
        _dbCtx = dbCtx;
    }
}