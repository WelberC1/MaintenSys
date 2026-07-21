using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MaintenSys.Infra.Persistence.Repositories;

public class UsuarioRepository(MaintenSysDbContext context) : IUsuarioRepository
{
    public Task<Usuario?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
        => context.Usuarios.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken cancellationToken = default)
        => context.Usuarios.FirstOrDefaultAsync(u => u.Email == email.Trim().ToLower(), cancellationToken);

    public async Task AdicionarAsync(Usuario usuario, CancellationToken cancellationToken = default)
        => await context.Usuarios.AddAsync(usuario, cancellationToken);

    public void Atualizar(Usuario usuario) => context.Entry(usuario).State = EntityState.Modified;
}
