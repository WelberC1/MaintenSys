using MaintenSys.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MaintenSys.Infra.Storage;

public class LocalFileStorageService : IArquivoStorageService
{
    private readonly string _diretorioBase;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _diretorioBase = configuration["Storage:AparelhosFotosPath"]
            ?? Path.Combine(AppContext.BaseDirectory, "wwwroot", "uploads", "aparelhos");

        Directory.CreateDirectory(_diretorioBase);
    }

    public async Task<string> SalvarAsync(Stream conteudo, string nomeArquivoOriginal, CancellationToken cancellationToken = default)
    {
        var extensao = Path.GetExtension(nomeArquivoOriginal);
        var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
        var caminhoCompleto = Path.Combine(_diretorioBase, nomeArquivo);

        await using var arquivo = File.Create(caminhoCompleto);
        await conteudo.CopyToAsync(arquivo, cancellationToken);

        return Path.Combine("uploads", "aparelhos", nomeArquivo).Replace('\\', '/');
    }
}
