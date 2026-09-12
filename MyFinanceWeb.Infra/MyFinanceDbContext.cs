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
        modelBuilder.Entity<PlanoConta>(entity =>
        {
            entity.ToTable("planoconta");
            entity.Property(item => item.Id).HasColumnName("id");
            entity.Property(item => item.Descricao).HasColumnName("descricao");
            entity.Property(item => item.Tipo).HasColumnName("tipo");
        });

        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.ToTable("transacao");
            entity.HasOne(item => item.PlanoConta)
                .WithMany()
                .HasForeignKey(item => item.PlanoContaId);
            entity.Property(item => item.Id).HasColumnName("id");
            entity.Property(item => item.Historico).HasColumnName("historico");
            entity.Property(item => item.Data)
                .HasColumnName("data")
                .HasColumnType("date");
            entity.Property(item => item.Valor).HasColumnName("valor");
            entity.Property(item => item.PlanoContaId).HasColumnName("planocontaid");
        });
    }
}
