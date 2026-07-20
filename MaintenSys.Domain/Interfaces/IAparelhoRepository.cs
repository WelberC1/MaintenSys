using MaintenSys.Domain.Entities;

namespace MaintenSys.Domain.Interfaces;

public interface IAparelhoRepository
{
    Task<Aparelho?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Aparelho>> ObterPorClienteAsync(Guid clienteId, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Aparelho aparelho, CancellationToken cancellationToken = default);
    void Atualizar(Aparelho aparelho);
}
