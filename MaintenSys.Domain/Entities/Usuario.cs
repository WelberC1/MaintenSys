using MaintenSys.Domain.Common;
using MaintenSys.Domain.Enums;

namespace MaintenSys.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string SenhaHash { get; private set; } = null!;
    public TipoUsuario Tipo { get; private set; }
    public bool Ativo { get; private set; } = true;

    protected Usuario() { }

    public Usuario(string nome, string email, string senhaHash, TipoUsuario tipo)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do usuário é obrigatório.");
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("O e-mail do usuário é obrigatório.");
        if (string.IsNullOrWhiteSpace(senhaHash))
            throw new DomainException("A senha do usuário é obrigatória.");

        Nome = nome;
        Email = email.Trim().ToLowerInvariant();
        SenhaHash = senhaHash;
        Tipo = tipo;
    }

    public void AtualizarDados(string nome, string email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do usuário é obrigatório.");
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("O e-mail do usuário é obrigatório.");

        Nome = nome;
        Email = email.Trim().ToLowerInvariant();
        Touch();
    }

    public void AlterarSenha(string novaSenhaHash)
    {
        if (string.IsNullOrWhiteSpace(novaSenhaHash))
            throw new DomainException("A senha do usuário é obrigatória.");

        SenhaHash = novaSenhaHash;
        Touch();
    }

    public void Desativar()
    {
        Ativo = false;
        Touch();
    }

    public void Ativar()
    {
        Ativo = true;
        Touch();
    }

    public bool EhAdministrador() => Tipo == TipoUsuario.Administrador;
}
