namespace Streamberry.Context.Test;

public class Tests
{
    [Test]
    public void MockDataTest()
    {
        var ctx = new TestContextFactory().CreateDbContext(new[] { "" });
        Assert.That(ctx.Movies.Count(), Is.GreaterThan(0));
    }
}