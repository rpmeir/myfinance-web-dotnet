using Microsoft.EntityFrameworkCore;

namespace MyFinanceWeb.Infra.Database;

public interface IMyFinanceDatabaseStrategy
{
    void Configure(DbContextOptionsBuilder optionsBuilder);
}