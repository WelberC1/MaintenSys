using MaintenSys.Domain.Entities;

namespace MaintenSys.Application.DTOs;

public record FotoAparelhoDto(Guid Id, string CaminhoArquivo, string? Descricao)
{
    public static FotoAparelhoDto DeEntidade(FotoAparelho foto) => new(foto.Id, foto.CaminhoArquivo, foto.Descricao);
}

public record AparelhoDto(
    Guid Id,
    Guid ClienteId,
    string Tipo,
    string Marca,
    string Modelo,
    string? NumeroSerie,
    string? Cor,
    int? Ano,
    string? Acessorios,
    IReadOnlyList<FotoAparelhoDto> Fotos,
    DateTime CreatedAt)
{
    public static AparelhoDto DeEntidade(Aparelho aparelho) => new(
        aparelho.Id,
        aparelho.ClienteId,
        aparelho.Tipo,
        aparelho.Marca,
        aparelho.Modelo,
        aparelho.NumeroSerie,
        aparelho.Cor,
        aparelho.Ano,
        aparelho.Acessorios,
        aparelho.Fotos.Select(FotoAparelhoDto.DeEntidade).ToList(),
        aparelho.CreatedAt);
}
