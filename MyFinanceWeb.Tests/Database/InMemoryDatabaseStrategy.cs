using Microsoft.EntityFrameworkCore;
using MyFinanceWeb.Infra;
using MyFinanceWeb.Infra.Database;

namespace MyFinanceWeb.Tests.Database;

public sealed class InMemoryDatabaseStrategy(string databaseName) : IMyFinanceDatabaseStrategy
{
    public void Configure(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
        optionsBuilder.UseInMemoryDatabase(databaseName);
    }
}