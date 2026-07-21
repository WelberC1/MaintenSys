using MaintenSys.Domain.Entities;

namespace MaintenSys.Application.Interfaces;

public interface IJwtTokenService
{
    string GerarToken(Usuario usuario);
}
