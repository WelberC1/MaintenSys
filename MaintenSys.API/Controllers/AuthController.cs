using MaintenSys.Application.UseCases.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenSys.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginCommand command, CancellationToken cancellationToken)
    {
        var resultado = await sender.Send(command, cancellationToken);
        return Ok(resultado);
    }

    [HttpPost("registrar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Registrar(RegistrarUsuarioCommand command, CancellationToken cancellationToken)
    {
        var resultado = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Login), new { id = resultado.Id }, resultado);
    }
}
