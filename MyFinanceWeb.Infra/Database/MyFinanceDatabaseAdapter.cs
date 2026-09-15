using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyFinanceWeb.Infra.Database;

public static class MyFinanceDatabaseAdapter
{
    public static IServiceCollection AddMyFinanceDbContext(
        this IServiceCollection services,
        IMyFinanceDatabaseStrategy databaseStrategy)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(databaseStrategy);

        return services.AddDbContext<MyFinanceDbContext>(
            options => databaseStrategy.Configure(options)
        );
    }
}