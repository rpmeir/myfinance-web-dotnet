using Microsoft.EntityFrameworkCore;
using MyFinanceWeb.Domain.Entities;

namespace MyFinanceWeb.Infra;

public class MyFinanceDbContext : DbContext
{
    public DbSet<PlanoConta> PlanoContas { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }

    public MyFinanceDbContext(DbContextOptions<MyFinanceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyFinanceDbContext).Assembly);
    }
}
