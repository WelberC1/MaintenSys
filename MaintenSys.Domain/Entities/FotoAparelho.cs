using MaintenSys.Domain.Common;

namespace MaintenSys.Domain.Entities;

public class FotoAparelho : BaseEntity
{
    public Guid AparelhoId { get; private set; }
    public string CaminhoArquivo { get; private set; } = null!;
    public string? Descricao { get; private set; }

    protected FotoAparelho() { }

    public FotoAparelho(Guid aparelhoId, string caminhoArquivo, string? descricao = null)
    {
        if (aparelhoId == Guid.Empty)
            throw new DomainException("A foto precisa estar vinculada a um aparelho.");
        if (string.IsNullOrWhiteSpace(caminhoArquivo))
            throw new DomainException("O caminho do arquivo da foto é obrigatório.");

        AparelhoId = aparelhoId;
        CaminhoArquivo = caminhoArquivo;
        Descricao = descricao;
    }
}
