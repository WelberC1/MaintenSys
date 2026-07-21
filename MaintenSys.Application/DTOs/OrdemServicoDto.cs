using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Enums;

namespace MaintenSys.Application.DTOs;

public record ItemOrdemServicoDto(Guid Id, string Descricao, TipoItemOrdemServico Tipo, int Quantidade, decimal ValorUnitario, decimal ValorTotal)
{
    public static ItemOrdemServicoDto DeEntidade(ItemOrdemServico item) => new(
        item.Id, item.Descricao, item.Tipo, item.Quantidade, item.ValorUnitario, item.ValorTotal);
}

public record HistoricoStatusDto(StatusOrdemServico? StatusAnterior, StatusOrdemServico StatusNovo, Guid UsuarioId, string? Observacao, DateTime DataAlteracao)
{
    public static HistoricoStatusDto DeEntidade(HistoricoStatusOrdemServico historico) => new(
        historico.StatusAnterior, historico.StatusNovo, historico.UsuarioId, historico.Observacao, historico.DataAlteracao);
}

public record OrdemServicoDto(
    Guid Id,
    Guid ClienteId,
    string? NomeCliente,
    Guid AparelhoId,
    string? DescricaoAparelho,
    Guid TecnicoId,
    string? NomeTecnico,
    string DefeitoRelatado,
    string? DiagnosticoTecnico,
    StatusOrdemServico Status,
    DateTime DataEntrada,
    DateTime? DataPrevisaoEntrega,
    DateTime? DataConclusao,
    DateTime? DataEntrega,
    int? PrazoGarantiaDias,
    string? Observacoes,
    decimal ValorTotal,
    IReadOnlyList<ItemOrdemServicoDto> Itens,
    IReadOnlyList<HistoricoStatusDto> Historico)
{
    public static OrdemServicoDto DeEntidade(OrdemServico os) => new(
        os.Id,
        os.ClienteId,
        os.Cliente?.Nome,
        os.AparelhoId,
        os.Aparelho is null ? null : $"{os.Aparelho.Marca} {os.Aparelho.Modelo}",
        os.TecnicoId,
        os.Tecnico?.Nome,
        os.DefeitoRelatado,
        os.DiagnosticoTecnico,
        os.Status,
        os.DataEntrada,
        os.DataPrevisaoEntrega,
        os.DataConclusao,
        os.DataEntrega,
        os.PrazoGarantiaDias,
        os.Observacoes,
        os.ValorTotal,
        os.Itens.Select(ItemOrdemServicoDto.DeEntidade).ToList(),
        os.Historico.OrderBy(h => h.DataAlteracao).Select(HistoricoStatusDto.DeEntidade).ToList());
}
