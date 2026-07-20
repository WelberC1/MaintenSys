using MaintenSys.Domain.Common;
using MaintenSys.Domain.Enums;

namespace MaintenSys.Domain.Entities;

public class HistoricoStatusOrdemServico : BaseEntity
{
    public Guid OrdemServicoId { get; private set; }
    public StatusOrdemServico? StatusAnterior { get; private set; }
    public StatusOrdemServico StatusNovo { get; private set; }
    public Guid UsuarioId { get; private set; }
    public string? Observacao { get; private set; }
    public DateTime DataAlteracao { get; private set; } = DateTime.UtcNow;

    protected HistoricoStatusOrdemServico() { }

    public HistoricoStatusOrdemServico(Guid ordemServicoId, StatusOrdemServico? statusAnterior, StatusOrdemServico statusNovo, Guid usuarioId, string? observacao)
    {
        if (ordemServicoId == Guid.Empty)
            throw new DomainException("O histórico precisa estar vinculado a uma ordem de serviço.");
        if (usuarioId == Guid.Empty)
            throw new DomainException("O histórico precisa registrar o usuário responsável pela alteração.");

        OrdemServicoId = ordemServicoId;
        StatusAnterior = statusAnterior;
        StatusNovo = statusNovo;
        UsuarioId = usuarioId;
        Observacao = observacao;
    }
}
