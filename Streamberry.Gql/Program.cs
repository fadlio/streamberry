using HotChocolate.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Streamberry.Context;
using Streamberry.Gql.Resolvers;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<EfContext>(
    options => options.UseSqlite(builder.Configuration["Data:ConnectionString"]));

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<EfContext>()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL().WithOptions(new GraphQLServerOptions
{
    Tool = { Enable = app.Environment.IsDevelopment() }
});

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var ctx = scope.ServiceProvider.GetRequiredService<EfContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<EfContext>>();

    EfContext.GenerateMockData(ctx);
    logger.LogInformation("Mock data generated");
}

app.Run();