using Microsoft.EntityFrameworkCore;

namespace MyFinanceWeb.Infra.Database;

public sealed class MyFinanceDbContextFactory
{
    private MyFinanceDbContextFactory() { }

    public static MyFinanceDbContext Create(IMyFinanceDatabaseStrategy databaseStrategy)
    {
        ArgumentNullException.ThrowIfNull(databaseStrategy);

        var optionsBuilder = new DbContextOptionsBuilder<MyFinanceDbContext>();
        databaseStrategy.Configure(optionsBuilder);

        return new MyFinanceDbContext(optionsBuilder.Options);
    }
}