using MaintenSys.Application.UseCases.Clientes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenSys.API.Controllers;

[ApiController]
[Route("api/clientes")]
[Authorize]
public class ClientesController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Criar(CriarClienteCommand command, CancellationToken cancellationToken)
    {
        var cliente = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, AtualizarClienteRequest request, CancellationToken cancellationToken)
    {
        var cliente = await sender.Send(new AtualizarClienteCommand(id, request.Nome, request.Telefone, request.Email, request.Endereco), cancellationToken);
        return Ok(cliente);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var cliente = await sender.Send(new ObterClientePorIdQuery(id), cancellationToken);
        return Ok(cliente);
    }

    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] string? termo, [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 20, CancellationToken cancellationToken = default)
    {
        var resultado = await sender.Send(new BuscarClientesQuery(termo, pagina, tamanhoPagina), cancellationToken);
        return Ok(resultado);
    }
}

public record AtualizarClienteRequest(string Nome, string Telefone, string? Email, string? Endereco);
