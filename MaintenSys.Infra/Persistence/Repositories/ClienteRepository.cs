using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaintenSys.Infra.Persistence.Repositories;

public class ClienteRepository(MaintenSysDbContext context) : IClienteRepository
{
    public Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => context.Clientes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<Cliente?> ObterPorCpfCnpjAsync(string cpfCnpj, CancellationToken cancellationToken = default)
        => context.Clientes.FirstOrDefaultAsync(c => c.CpfCnpj == cpfCnpj, cancellationToken);

    public async Task<(IReadOnlyList<Cliente> Itens, int Total)> BuscarAsync(string? termo, int pagina, int tamanhoPagina, CancellationToken cancellationToken = default)
    {
        var query = context.Clientes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(termo))
            query = query.Where(c => c.Nome.Contains(termo) || c.CpfCnpj.Contains(termo) || c.Telefone.Contains(termo));

        var total = await query.CountAsync(cancellationToken);

        var itens = await query
            .OrderBy(c => c.Nome)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync(cancellationToken);

        return (itens, total);
    }

    public async Task AdicionarAsync(Cliente cliente, CancellationToken cancellationToken = default)
        => await context.Clientes.AddAsync(cliente, cancellationToken);

    public void Atualizar(Cliente cliente) => context.Entry(cliente).State = EntityState.Modified;
}
