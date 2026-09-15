using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MyFinanceWeb.Infra.Database;
using MyFinanceWeb.Tests.Database;

namespace MyFinanceWeb.Tests.Integration;

public sealed class MyFinanceWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            services.AddMyFinanceDbContext(
                new InMemoryDatabaseStrategy($"myfinance-integration-{Guid.NewGuid():N}")
            );
        });
    }
}