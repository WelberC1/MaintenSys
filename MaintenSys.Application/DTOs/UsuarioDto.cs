using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Enums;

namespace MaintenSys.Application.DTOs;

public record UsuarioDto(Guid Id, string Nome, string Email, TipoUsuario Tipo, bool Ativo)
{
    public static UsuarioDto DeEntidade(Usuario usuario) => new(usuario.Id, usuario.Nome, usuario.Email, usuario.Tipo, usuario.Ativo);
}

public record LoginResultDto(string Token, UsuarioDto Usuario);
