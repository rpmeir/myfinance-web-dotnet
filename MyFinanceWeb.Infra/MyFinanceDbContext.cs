using Microsoft.EntityFrameworkCore;
using MyFinanceWeb.Domain.Entities;

namespace MyFinanceWeb.Infra;

public class MyFinanceDbContext(DbContextOptions<MyFinanceDbContext> options) : DbContext(options)
{
    public DbSet<PlanoConta> PlanoContas { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=myfinance;Username=postgres;Password=123456");
    }
}
