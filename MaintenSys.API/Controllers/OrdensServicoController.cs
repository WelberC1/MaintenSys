using MaintenSys.Application.Interfaces;
using MaintenSys.Application.UseCases.OrdensServico;
using MaintenSys.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MaintenSys.API.Controllers;

[ApiController]
[Route("api/ordens-servico")]
[Authorize]
public class OrdensServicoController(ISender sender, ICurrentUserService currentUser) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Abrir(AbrirOrdemServicoRequest request, CancellationToken cancellationToken)
    {
        var command = new AbrirOrdemServicoCommand(request.ClienteId, request.AparelhoId, currentUser.UsuarioId, request.DefeitoRelatado, request.DataPrevisaoEntrega);
        var ordemServico = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = ordemServico.Id }, ordemServico);
    }

    [HttpPost("{id:guid}/diagnostico")]
    public async Task<IActionResult> RegistrarDiagnostico(Guid id, RegistrarDiagnosticoRequest request, CancellationToken cancellationToken)
    {
        var ordemServico = await sender.Send(new RegistrarDiagnosticoCommand(id, request.Diagnostico), cancellationToken);
        return Ok(ordemServico);
    }

    [HttpPost("{id:guid}/itens")]
    public async Task<IActionResult> AdicionarItem(Guid id, AdicionarItemRequest request, CancellationToken cancellationToken)
    {
        var command = new AdicionarItemCommand(id, request.Descricao, request.Tipo, request.Quantidade, request.ValorUnitario);
        var ordemServico = await sender.Send(command, cancellationToken);
        return Ok(ordemServico);
    }

    [HttpDelete("{id:guid}/itens/{itemId:guid}")]
    public async Task<IActionResult> RemoverItem(Guid id, Guid itemId, CancellationToken cancellationToken)
    {
        var ordemServico = await sender.Send(new RemoverItemCommand(id, itemId), cancellationToken);
        return Ok(ordemServico);
    }

    [HttpPost("{id:guid}/status")]
    public async Task<IActionResult> AlterarStatus(Guid id, AlterarStatusRequest request, CancellationToken cancellationToken)
    {
        var command = new AlterarStatusCommand(id, request.NovoStatus, currentUser.UsuarioId, request.Observacao);
        var ordemServico = await sender.Send(command, cancellationToken);
        return Ok(ordemServico);
    }

    [HttpPost("{id:guid}/garantia")]
    public async Task<IActionResult> DefinirGarantia(Guid id, DefinirGarantiaRequest request, CancellationToken cancellationToken)
    {
        var ordemServico = await sender.Send(new DefinirGarantiaCommand(id, request.PrazoGarantiaDias), cancellationToken);
        return Ok(ordemServico);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        var ordemServico = await sender.Send(new ObterOrdemServicoPorIdQuery(id), cancellationToken);
        return Ok(ordemServico);
    }

    [HttpGet]
    public async Task<IActionResult> Buscar(
        [FromQuery] StatusOrdemServico? status,
        [FromQuery] Guid? tecnicoId,
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 20,
        CancellationToken cancellationToken = default)
    {
        var resultado = await sender.Send(new BuscarOrdensServicoQuery(status, tecnicoId, pagina, tamanhoPagina), cancellationToken);
        return Ok(resultado);
    }

    [HttpGet("{id:guid}/pdf")]
    public async Task<IActionResult> GerarPdf(Guid id, CancellationToken cancellationToken)
    {
        var pdf = await sender.Send(new GerarPdfOrdemServicoQuery(id), cancellationToken);
        return File(pdf, "application/pdf", $"OS-{id.ToString()[..8]}.pdf");
    }
}

public record AbrirOrdemServicoRequest(Guid ClienteId, Guid AparelhoId, string DefeitoRelatado, DateTime? DataPrevisaoEntrega);
public record RegistrarDiagnosticoRequest(string Diagnostico);
public record AdicionarItemRequest(string Descricao, TipoItemOrdemServico Tipo, int Quantidade, decimal ValorUnitario);
public record AlterarStatusRequest(StatusOrdemServico NovoStatus, string? Observacao);
public record DefinirGarantiaRequest(int PrazoGarantiaDias);
