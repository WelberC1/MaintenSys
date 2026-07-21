using System.Security.Claims;
using MaintenSys.Application.Interfaces;
using MaintenSys.Domain.Enums;

namespace MaintenSys.API.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UsuarioId
    {
        get
        {
            var valor = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(valor, out var id) ? id : Guid.Empty;
        }
    }

    public TipoUsuario Tipo
    {
        get
        {
            var valor = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            return Enum.TryParse<TipoUsuario>(valor, out var tipo) ? tipo : default;
        }
    }
}
