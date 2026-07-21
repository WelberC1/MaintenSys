using MaintenSys.Application.UseCases.Aparelhos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenSys.API.Controllers;

[ApiController]
[Route("api/aparelhos")]
[Authorize]
public class AparelhosController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Criar(CriarAparelhoCommand command, CancellationToken cancellationToken)
    {
        var aparelho = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = aparelho.Id }, aparelho);
    }

    [HttpPost("{id:guid}/fotos")]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> AdicionarFoto(Guid id, IFormFile arquivo, [FromForm] string? descricao, CancellationToken cancellationToken)
    {
        if (arquivo.Length == 0)
            return BadRequest(new { mensagem = "O arquivo enviado está vazio." });

        await using var stream = arquivo.OpenReadStream();
        var aparelho = await sender.Send(new AdicionarFotoAparelhoCommand(id, stream, arquivo.FileName, descricao), cancellationToken);
        return Ok(aparelho);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var aparelho = await sender.Send(new ObterAparelhoPorIdQuery(id), cancellationToken);
        return Ok(aparelho);
    }

    [HttpGet("por-cliente/{clienteId:guid}")]
    public async Task<IActionResult> ObterPorCliente(Guid clienteId, CancellationToken cancellationToken)
    {
        var aparelhos = await sender.Send(new ObterAparelhosPorClienteQuery(clienteId), cancellationToken);
        return Ok(aparelhos);
    }
}
