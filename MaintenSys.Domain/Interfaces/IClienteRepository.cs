using MaintenSys.Domain.Entities;

namespace MaintenSys.Domain.Interfaces;

public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Cliente?> ObterPorCpfCnpjAsync(string cpfCnpj, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Cliente>> BuscarAsync(string? termo, int pagina, int tamanhoPagina, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Cliente cliente, CancellationToken cancellationToken = default);
    void Atualizar(Cliente cliente);
}
