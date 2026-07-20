using MaintenSys.Domain.Common;

namespace MaintenSys.Domain.Entities;

public class Cliente : BaseEntity
{
    public string Nome { get; private set; } = null!;
    public string CpfCnpj { get; private set; } = null!;
    public string Telefone { get; private set; } = null!;
    public string? Email { get; private set; }
    public string? Endereco { get; private set; }

    private readonly List<Aparelho> _aparelhos = new();
    public IReadOnlyCollection<Aparelho> Aparelhos => _aparelhos.AsReadOnly();

    protected Cliente() { }

    public Cliente(string nome, string cpfCnpj, string telefone, string? email = null, string? endereco = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do cliente é obrigatório.");
        if (string.IsNullOrWhiteSpace(cpfCnpj))
            throw new DomainException("O CPF/CNPJ do cliente é obrigatório.");
        if (string.IsNullOrWhiteSpace(telefone))
            throw new DomainException("O telefone do cliente é obrigatório.");

        Nome = nome;
        CpfCnpj = cpfCnpj;
        Telefone = telefone;
        Email = email;
        Endereco = endereco;
    }

    public void AtualizarDados(string nome, string telefone, string? email, string? endereco)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do cliente é obrigatório.");
        if (string.IsNullOrWhiteSpace(telefone))
            throw new DomainException("O telefone do cliente é obrigatório.");

        Nome = nome;
        Telefone = telefone;
        Email = email;
        Endereco = endereco;
        Touch();
    }
}
