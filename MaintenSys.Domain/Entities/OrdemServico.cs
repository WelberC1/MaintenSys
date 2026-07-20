using MaintenSys.Domain.Common;
using MaintenSys.Domain.Enums;

namespace MaintenSys.Domain.Entities;

public class OrdemServico : BaseEntity
{
    public Guid ClienteId { get; private set; }
    public Cliente? Cliente { get; private set; }

    public Guid AparelhoId { get; private set; }
    public Aparelho? Aparelho { get; private set; }

    public Guid TecnicoId { get; private set; }
    public Usuario? Tecnico { get; private set; }

    public string DefeitoRelatado { get; private set; } = null!;
    public string? DiagnosticoTecnico { get; private set; }
    public StatusOrdemServico Status { get; private set; }

    public DateTime DataEntrada { get; private set; } = DateTime.UtcNow;
    public DateTime? DataPrevisaoEntrega { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public DateTime? DataEntrega { get; private set; }

    public int? PrazoGarantiaDias { get; private set; }
    public string? Observacoes { get; private set; }

    private readonly List<ItemOrdemServico> _itens = new();
    public IReadOnlyCollection<ItemOrdemServico> Itens => _itens.AsReadOnly();

    private readonly List<HistoricoStatusOrdemServico> _historico = new();
    public IReadOnlyCollection<HistoricoStatusOrdemServico> Historico => _historico.AsReadOnly();

    public decimal ValorTotal => _itens.Sum(i => i.ValorTotal);

    protected OrdemServico() { }

    public OrdemServico(Guid clienteId, Guid aparelhoId, Guid tecnicoId, string defeitoRelatado, DateTime? dataPrevisaoEntrega = null)
    {
        if (clienteId == Guid.Empty)
            throw new DomainException("A ordem de serviço precisa estar vinculada a um cliente.");
        if (aparelhoId == Guid.Empty)
            throw new DomainException("A ordem de serviço precisa estar vinculada a um aparelho.");
        if (tecnicoId == Guid.Empty)
            throw new DomainException("A ordem de serviço precisa ter um técnico responsável.");
        if (string.IsNullOrWhiteSpace(defeitoRelatado))
            throw new DomainException("O defeito relatado pelo cliente é obrigatório.");

        ClienteId = clienteId;
        AparelhoId = aparelhoId;
        TecnicoId = tecnicoId;
        DefeitoRelatado = defeitoRelatado;
        DataPrevisaoEntrega = dataPrevisaoEntrega;
        Status = StatusOrdemServico.Aberta;

        _historico.Add(new HistoricoStatusOrdemServico(Id, null, StatusOrdemServico.Aberta, tecnicoId, "Ordem de serviço aberta."));
    }

    public void RegistrarDiagnostico(string diagnostico)
    {
        if (string.IsNullOrWhiteSpace(diagnostico))
            throw new DomainException("O diagnóstico técnico é obrigatório.");

        DiagnosticoTecnico = diagnostico;
        Touch();
    }

    public ItemOrdemServico AdicionarItem(string descricao, TipoItemOrdemServico tipo, int quantidade, decimal valorUnitario)
    {
        if (Status is StatusOrdemServico.Entregue or StatusOrdemServico.Cancelada)
            throw new DomainException("Não é possível adicionar itens a uma ordem de serviço finalizada.");

        var item = new ItemOrdemServico(Id, descricao, tipo, quantidade, valorUnitario);
        _itens.Add(item);
        Touch();
        return item;
    }

    public void RemoverItem(Guid itemId)
    {
        if (Status is StatusOrdemServico.Entregue or StatusOrdemServico.Cancelada)
            throw new DomainException("Não é possível remover itens de uma ordem de serviço finalizada.");

        var item = _itens.FirstOrDefault(i => i.Id == itemId);
        if (item is null)
            throw new DomainException("Item não encontrado nesta ordem de serviço.");

        _itens.Remove(item);
        Touch();
    }

    public void AlterarStatus(StatusOrdemServico novoStatus, Guid usuarioId, string? observacao = null)
    {
        if (usuarioId == Guid.Empty)
            throw new DomainException("É necessário informar o usuário responsável pela alteração de status.");

        if (!TransicoesStatusOrdemServico.PodeTransicionar(Status, novoStatus))
            throw new DomainException($"Não é possível mudar o status de '{Status}' para '{novoStatus}'.");

        var statusAnterior = Status;
        Status = novoStatus;

        if (novoStatus == StatusOrdemServico.Concluida)
            DataConclusao = DateTime.UtcNow;

        if (novoStatus == StatusOrdemServico.Entregue)
            DataEntrega = DateTime.UtcNow;

        _historico.Add(new HistoricoStatusOrdemServico(Id, statusAnterior, novoStatus, usuarioId, observacao));
        Touch();
    }

    public void DefinirGarantia(int prazoGarantiaDias)
    {
        if (prazoGarantiaDias < 0)
            throw new DomainException("O prazo de garantia não pode ser negativo.");

        PrazoGarantiaDias = prazoGarantiaDias;
        Touch();
    }

    public void AtualizarObservacoes(string? observacoes)
    {
        Observacoes = observacoes;
        Touch();
    }

    public void AtualizarPrevisaoEntrega(DateTime? dataPrevisaoEntrega)
    {
        DataPrevisaoEntrega = dataPrevisaoEntrega;
        Touch();
    }
}
