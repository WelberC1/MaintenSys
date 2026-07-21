using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.Clientes;

public record BuscarClientesQuery(string? Termo, int Pagina = 1, int TamanhoPagina = 20) : IRequest<PagedResult<ClienteDto>>;

public class BuscarClientesQueryHandler(IClienteRepository clienteRepository) : IRequestHandler<BuscarClientesQuery, PagedResult<ClienteDto>>
{
    public async Task<PagedResult<ClienteDto>> Handle(BuscarClientesQuery request, CancellationToken cancellationToken)
    {
        var pagina = request.Pagina < 1 ? 1 : request.Pagina;
        var tamanhoPagina = request.TamanhoPagina is < 1 or > 100 ? 20 : request.TamanhoPagina;

        var (clientes, total) = await clienteRepository.BuscarAsync(request.Termo, pagina, tamanhoPagina, cancellationToken);

        return new PagedResult<ClienteDto>
        {
            Itens = clientes.Select(ClienteDto.DeEntidade).ToList(),
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            TotalRegistros = total
        };
    }
}
