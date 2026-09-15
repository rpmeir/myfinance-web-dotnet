using MyFinanceWeb.Infra;
using MyFinanceWeb.Infra.Database;

namespace MyFinanceWeb.Tests.Database;

public sealed class TestDatabaseBuilder
{
    private readonly string _databaseName = $"myfinance-tests-{Guid.NewGuid():N}";

    public MyFinanceDbContext Build()
    {
        var databaseStrategy = new InMemoryDatabaseStrategy(_databaseName);
        var dbContext = MyFinanceDbContextFactory.Create(databaseStrategy);
        dbContext.Database.EnsureCreated();

        return dbContext;
    }
}