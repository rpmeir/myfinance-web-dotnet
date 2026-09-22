using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyFinanceWeb.Domain.Entities;

namespace MyFinanceWeb.Infra.Configurations;

public class PlanoContaConfiguration : IEntityTypeConfiguration<PlanoConta>
{
    public void Configure(EntityTypeBuilder<PlanoConta> builder)
    {
        builder.ToTable("planoconta");
        builder.Property(item => item.Id).HasColumnName("id");
        builder.Property(item => item.Descricao).HasColumnName("descricao");
        builder.Property(item => item.Tipo).HasColumnName("tipo");
    }
}