using MaintenSys.Domain.Common;
using MaintenSys.Domain.Enums;

namespace MaintenSys.Domain.Entities;

public class ItemOrdemServico : BaseEntity
{
    public Guid OrdemServicoId { get; private set; }
    public string Descricao { get; private set; } = null!;
    public TipoItemOrdemServico Tipo { get; private set; }
    public int Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }

    public decimal ValorTotal => Quantidade * ValorUnitario;

    protected ItemOrdemServico() { }

    public ItemOrdemServico(Guid ordemServicoId, string descricao, TipoItemOrdemServico tipo, int quantidade, decimal valorUnitario)
    {
        if (ordemServicoId == Guid.Empty)
            throw new DomainException("O item precisa estar vinculado a uma ordem de serviço.");
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("A descrição do item é obrigatória.");
        if (quantidade <= 0)
            throw new DomainException("A quantidade do item deve ser maior que zero.");
        if (valorUnitario < 0)
            throw new DomainException("O valor unitário do item não pode ser negativo.");

        OrdemServicoId = ordemServicoId;
        Descricao = descricao;
        Tipo = tipo;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
    }
}
