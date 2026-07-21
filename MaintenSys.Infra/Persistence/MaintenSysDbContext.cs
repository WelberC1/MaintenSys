using MaintenSys.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MaintenSys.Infra.Persistence;

public class MaintenSysDbContext(DbContextOptions<MaintenSysDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Aparelho> Aparelhos => Set<Aparelho>();
    public DbSet<FotoAparelho> FotosAparelho => Set<FotoAparelho>();
    public DbSet<OrdemServico> OrdensServico => Set<OrdemServico>();
    public DbSet<ItemOrdemServico> ItensOrdemServico => Set<ItemOrdemServico>();
    public DbSet<HistoricoStatusOrdemServico> HistoricoStatusOrdemServico => Set<HistoricoStatusOrdemServico>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UsePropertyAccessMode(Microsoft.EntityFrameworkCore.PropertyAccessMode.Field);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MaintenSysDbContext).Assembly);
    }
}
