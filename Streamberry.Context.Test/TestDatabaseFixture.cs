namespace Streamberry.Context.Test;

public class TestDatabaseFixture
{
    private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True";

    private static readonly object _lock = new();
    private static bool _databaseInitialized;
}