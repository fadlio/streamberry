using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace Streamberry.Context;

public class DapperContext
{
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration["Data:ConnectionString"]!;
    }

    public DapperContext()
    {
        _connectionString = @"Data Source=test.db";
    }

    public IDbConnection CreateConnection()
        => new SqliteConnection(_connectionString);
}