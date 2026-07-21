using MaintenSys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenSys.Infra.Persistence.Configurations;

public class AparelhoConfiguration : IEntityTypeConfiguration<Aparelho>
{
    public void Configure(EntityTypeBuilder<Aparelho> builder)
    {
        builder.ToTable("Aparelhos");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Tipo).IsRequired().HasMaxLength(50);
        builder.Property(a => a.Marca).IsRequired().HasMaxLength(50);
        builder.Property(a => a.Modelo).IsRequired().HasMaxLength(50);
        builder.Property(a => a.NumeroSerie).HasMaxLength(100);
        builder.Property(a => a.Cor).HasMaxLength(30);
        builder.Property(a => a.Acessorios).HasMaxLength(500);

        builder.HasMany(a => a.Fotos)
            .WithOne()
            .HasForeignKey(f => f.AparelhoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.OrdensServico)
            .WithOne(o => o.Aparelho)
            .HasForeignKey(o => o.AparelhoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
