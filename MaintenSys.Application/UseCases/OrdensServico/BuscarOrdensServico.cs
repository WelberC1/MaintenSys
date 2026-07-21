using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Enums;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.OrdensServico;

public record BuscarOrdensServicoQuery(StatusOrdemServico? Status, Guid? TecnicoId, int Pagina = 1, int TamanhoPagina = 20)
    : IRequest<PagedResult<OrdemServicoDto>>;

public class BuscarOrdensServicoQueryHandler(IOrdemServicoRepository ordemServicoRepository)
    : IRequestHandler<BuscarOrdensServicoQuery, PagedResult<OrdemServicoDto>>
{
    public async Task<PagedResult<OrdemServicoDto>> Handle(BuscarOrdensServicoQuery request, CancellationToken cancellationToken)
    {
        var pagina = request.Pagina < 1 ? 1 : request.Pagina;
        var tamanhoPagina = request.TamanhoPagina is < 1 or > 100 ? 20 : request.TamanhoPagina;

        var (ordens, total) = await ordemServicoRepository.BuscarAsync(request.Status, request.TecnicoId, pagina, tamanhoPagina, cancellationToken);

        return new PagedResult<OrdemServicoDto>
        {
            Itens = ordens.Select(OrdemServicoDto.DeEntidade).ToList(),
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            TotalRegistros = total
        };
    }
}
