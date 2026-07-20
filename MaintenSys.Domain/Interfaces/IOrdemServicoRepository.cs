using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Enums;

namespace MaintenSys.Domain.Interfaces;

public interface IOrdemServicoRepository
{
    Task<OrdemServico?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OrdemServico?> ObterCompletaPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrdemServico>> BuscarAsync(StatusOrdemServico? status, Guid? tecnicoId, int pagina, int tamanhoPagina, CancellationToken cancellationToken = default);
    Task AdicionarAsync(OrdemServico ordemServico, CancellationToken cancellationToken = default);
    void Atualizar(OrdemServico ordemServico);
}
