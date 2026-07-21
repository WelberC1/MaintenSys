using MaintenSys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenSys.Infra.Persistence.Configurations;

public class HistoricoStatusOrdemServicoConfiguration : IEntityTypeConfiguration<HistoricoStatusOrdemServico>
{
    public void Configure(EntityTypeBuilder<HistoricoStatusOrdemServico> builder)
    {
        builder.ToTable("HistoricoStatusOrdemServico");
        builder.HasKey(h => h.Id);

        builder.Property(h => h.StatusAnterior).HasConversion<string>().HasMaxLength(40);
        builder.Property(h => h.StatusNovo).HasConversion<string>().HasMaxLength(40);
        builder.Property(h => h.Observacao).HasMaxLength(500);
    }
}
