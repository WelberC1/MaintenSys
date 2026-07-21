using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaintenSys.Infra.Persistence.Repositories;

public class AparelhoRepository(MaintenSysDbContext context) : IAparelhoRepository
{
    public Task<Aparelho?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => context.Aparelhos
            .Include(a => a.Fotos)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Aparelho>> ObterPorClienteAsync(Guid clienteId, CancellationToken cancellationToken = default)
        => await context.Aparelhos
            .Include(a => a.Fotos)
            .Where(a => a.ClienteId == clienteId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task AdicionarAsync(Aparelho aparelho, CancellationToken cancellationToken = default)
        => await context.Aparelhos.AddAsync(aparelho, cancellationToken);

    public void Atualizar(Aparelho aparelho)
    {
        context.Entry(aparelho).State = EntityState.Modified;

        // Fotos usam Guid gerado no client: o EF não distingue "nova" de "existente"
        // apenas pela chave, então marcamos explicitamente as ainda não rastreadas.
        foreach (var foto in aparelho.Fotos)
        {
            if (context.Entry(foto).State == EntityState.Detached)
                context.Entry(foto).State = EntityState.Added;
        }
    }
}
