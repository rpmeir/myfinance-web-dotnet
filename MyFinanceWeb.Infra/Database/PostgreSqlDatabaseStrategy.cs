using Microsoft.EntityFrameworkCore;

namespace MyFinanceWeb.Infra.Database;

public sealed class PostgreSqlDatabaseStrategy(string connectionString) : IMyFinanceDatabaseStrategy
{
    public void Configure(DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);
        optionsBuilder.UseNpgsql(connectionString);
    }
}