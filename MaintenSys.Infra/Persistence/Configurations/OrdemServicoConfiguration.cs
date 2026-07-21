using MaintenSys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenSys.Infra.Persistence.Configurations;

public class OrdemServicoConfiguration : IEntityTypeConfiguration<OrdemServico>
{
    public void Configure(EntityTypeBuilder<OrdemServico> builder)
    {
        builder.ToTable("OrdensServico");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.DefeitoRelatado).IsRequired().HasMaxLength(1000);
        builder.Property(o => o.DiagnosticoTecnico).HasMaxLength(2000);
        builder.Property(o => o.Observacoes).HasMaxLength(1000);
        builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(40);

        builder.Ignore(o => o.ValorTotal);

        builder.HasOne(o => o.Cliente)
            .WithMany()
            .HasForeignKey(o => o.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Tecnico)
            .WithMany()
            .HasForeignKey(o => o.TecnicoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(o => o.Itens)
            .WithOne()
            .HasForeignKey(i => i.OrdemServicoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.Historico)
            .WithOne()
            .HasForeignKey(h => h.OrdemServicoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
