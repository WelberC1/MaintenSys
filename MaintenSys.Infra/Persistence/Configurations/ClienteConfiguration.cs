using MaintenSys.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MaintenSys.Infra.Persistence.Configurations;

public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nome).IsRequired().HasMaxLength(150);
        builder.Property(c => c.CpfCnpj).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Telefone).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Email).HasMaxLength(200);
        builder.Property(c => c.Endereco).HasMaxLength(300);

        builder.HasIndex(c => c.CpfCnpj).IsUnique();

        builder.HasMany(c => c.Aparelhos)
            .WithOne(a => a.Cliente)
            .HasForeignKey(a => a.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
