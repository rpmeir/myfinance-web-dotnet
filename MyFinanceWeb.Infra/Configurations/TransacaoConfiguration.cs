using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinanceWeb.Domain.Entities;

namespace MyFinanceWeb.Infra.Configurations;

public class TransacaoConfiguration : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.ToTable("transacao");
        builder.HasOne(item => item.PlanoConta)
            .WithMany()
            .HasForeignKey(item => item.PlanoContaId);
        builder.Property(item => item.Id).HasColumnName("id");
        builder.Property(item => item.Historico).HasColumnName("historico");
        builder.Property(item => item.Data)
            .HasColumnName("data")
            .HasColumnType("date");
        builder.Property(item => item.Valor).HasColumnName("valor");
        builder.Property(item => item.PlanoContaId).HasColumnName("planocontaid");
    }
}