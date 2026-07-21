using MaintenSys.Domain.Enums;

namespace MaintenSys.Application.Interfaces;

public interface ICurrentUserService
{
    Guid UsuarioId { get; }
    TipoUsuario Tipo { get; }
}
