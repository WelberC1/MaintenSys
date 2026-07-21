using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Enums;
using MaintenSys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaintenSys.Infra.Persistence.Repositories;

public class OrdemServicoRepository(MaintenSysDbContext context) : IOrdemServicoRepository
{
    public Task<OrdemServico?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => context.OrdensServico
            .Include(o => o.Itens)
            .Include(o => o.Historico)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public Task<OrdemServico?> ObterCompletaPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => context.OrdensServico
            .Include(o => o.Cliente)
            .Include(o => o.Aparelho)
            .Include(o => o.Tecnico)
            .Include(o => o.Itens)
            .Include(o => o.Historico)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task<(IReadOnlyList<OrdemServico> Itens, int Total)> BuscarAsync(
        StatusOrdemServico? status, Guid? tecnicoId, int pagina, int tamanhoPagina, CancellationToken cancellationToken = default)
    {
        var query = context.OrdensServico
            .Include(o => o.Cliente)
            .Include(o => o.Aparelho)
            .Include(o => o.Tecnico)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        if (tecnicoId.HasValue)
            query = query.Where(o => o.TecnicoId == tecnicoId.Value);

        var total = await query.CountAsync(cancellationToken);

        var itens = await query
            .OrderByDescending(o => o.DataEntrada)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(cancellationToken);

        return (itens, total);
    }

    public async Task AdicionarAsync(OrdemServico ordemServico, CancellationToken cancellationToken = default)
        => await context.OrdensServico.AddAsync(ordemServico, cancellationToken);

    public void Atualizar(OrdemServico ordemServico)
    {
        context.Entry(ordemServico).State = EntityState.Modified;

        // Entidades filhas usam Guid gerado no client, então o EF não consegue
        // distinguir "nova" de "existente" apenas pela chave: marcamos explicitamente
        // como Added as que ainda não estão rastreadas pelo ChangeTracker.
        foreach (var item in ordemServico.Itens)
        {
            if (context.Entry(item).State == EntityState.Detached)
                context.Entry(item).State = EntityState.Added;
        }

        foreach (var historico in ordemServico.Historico)
        {
            if (context.Entry(historico).State == EntityState.Detached)
                context.Entry(historico).State = EntityState.Added;
        }
    }
}
