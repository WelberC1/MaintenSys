using MaintenSys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenSys.Infra.Persistence.Configurations;

public class ItemOrdemServicoConfiguration : IEntityTypeConfiguration<ItemOrdemServico>
{
    public void Configure(EntityTypeBuilder<ItemOrdemServico> builder)
    {
        builder.ToTable("ItensOrdemServico");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Descricao).IsRequired().HasMaxLength(200);
        builder.Property(i => i.Tipo).HasConversion<string>().HasMaxLength(20);
        builder.Property(i => i.ValorUnitario).HasColumnType("decimal(18,2)");

        builder.Ignore(i => i.ValorTotal);
    }
}
