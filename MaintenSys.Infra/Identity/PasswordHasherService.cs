using MaintenSys.Application.Interfaces;
using MaintenSys.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MaintenSys.Infra.Identity;

public class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<Usuario> _hasher = new();

    public string Hash(string senha) => _hasher.HashPassword(null!, senha);

    public bool Verificar(string senhaHash, string senhaFornecida)
        => _hasher.VerifyHashedPassword(null!, senhaHash, senhaFornecida) != PasswordVerificationResult.Failed;
}
