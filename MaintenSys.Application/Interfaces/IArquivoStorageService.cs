namespace MaintenSys.Application.Interfaces;

public interface IArquivoStorageService
{
    Task<string> SalvarAsync(Stream conteudo, string nomeArquivoOriginal, CancellationToken cancellationToken = default);
}
