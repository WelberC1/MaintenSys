using MaintenSys.Domain.Common;

namespace MaintenSys.Domain.Entities;

public class Aparelho : BaseEntity
{
    public Guid ClienteId { get; private set; }
    public Cliente? Cliente { get; private set; }

    public string Tipo { get; private set; } = null!;
    public string Marca { get; private set; } = null!;
    public string Modelo { get; private set; } = null!;
    public string? NumeroSerie { get; private set; }
    public string? Cor { get; private set; }
    public int? Ano { get; private set; }
    public string? Acessorios { get; private set; }

    private readonly List<FotoAparelho> _fotos = new();
    public IReadOnlyCollection<FotoAparelho> Fotos => _fotos.AsReadOnly();

    private readonly List<OrdemServico> _ordensServico = new();
    public IReadOnlyCollection<OrdemServico> OrdensServico => _ordensServico.AsReadOnly();

    protected Aparelho() { }

    public Aparelho(
        Guid clienteId,
        string tipo,
        string marca,
        string modelo,
        string? numeroSerie = null,
        string? cor = null,
        int? ano = null,
        string? acessorios = null)
    {
        if (clienteId == Guid.Empty)
            throw new DomainException("O aparelho precisa estar vinculado a um cliente.");
        if (string.IsNullOrWhiteSpace(tipo))
            throw new DomainException("O tipo do aparelho é obrigatório.");
        if (string.IsNullOrWhiteSpace(marca))
            throw new DomainException("A marca do aparelho é obrigatória.");
        if (string.IsNullOrWhiteSpace(modelo))
            throw new DomainException("O modelo do aparelho é obrigatório.");
        if (ano.HasValue && (ano < 1980 || ano > DateTime.UtcNow.Year + 1))
            throw new DomainException("O ano informado para o aparelho é inválido.");

        ClienteId = clienteId;
        Tipo = tipo;
        Marca = marca;
        Modelo = modelo;
        NumeroSerie = numeroSerie;
        Cor = cor;
        Ano = ano;
        Acessorios = acessorios;
    }

    public void AtualizarDados(string tipo, string marca, string modelo, string? numeroSerie, string? cor, int? ano, string? acessorios)
    {
        if (string.IsNullOrWhiteSpace(tipo))
            throw new DomainException("O tipo do aparelho é obrigatório.");
        if (string.IsNullOrWhiteSpace(marca))
            throw new DomainException("A marca do aparelho é obrigatória.");
        if (string.IsNullOrWhiteSpace(modelo))
            throw new DomainException("O modelo do aparelho é obrigatório.");

        Tipo = tipo;
        Marca = marca;
        Modelo = modelo;
        NumeroSerie = numeroSerie;
        Cor = cor;
        Ano = ano;
        Acessorios = acessorios;
        Touch();
    }

    public FotoAparelho AdicionarFoto(string caminhoArquivo, string? descricao = null)
    {
        var foto = new FotoAparelho(Id, caminhoArquivo, descricao);
        _fotos.Add(foto);
        Touch();
        return foto;
    }

    public void RemoverFoto(Guid fotoId)
    {
        var foto = _fotos.FirstOrDefault(f => f.Id == fotoId);
        if (foto is null)
            throw new DomainException("Foto não encontrada para este aparelho.");

        _fotos.Remove(foto);
        Touch();
    }
}
