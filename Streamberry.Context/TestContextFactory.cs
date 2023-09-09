using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Streamberry.Context;


// https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation
// Used for migrations and tests
public class TestContextFactory : IDesignTimeDbContextFactory<EfContext>
{
   public EfContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EfContext>();
        optionsBuilder.UseSqlite(@"Data Source=test.db");

        return new EfContext(optionsBuilder.Options);
    }
}