using MaintenSys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenSys.Infra.Persistence.Configurations;

public class FotoAparelhoConfiguration : IEntityTypeConfiguration<FotoAparelho>
{
    public void Configure(EntityTypeBuilder<FotoAparelho> builder)
    {
        builder.ToTable("FotosAparelho");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.CaminhoArquivo).IsRequired().HasMaxLength(400);
        builder.Property(f => f.Descricao).HasMaxLength(200);
    }
}
