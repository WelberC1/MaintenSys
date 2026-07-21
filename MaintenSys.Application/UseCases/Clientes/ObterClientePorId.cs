using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.Clientes;

public record ObterClientePorIdQuery(Guid Id) : IRequest<ClienteDto>;

public class ObterClientePorIdQueryHandler(IClienteRepository clienteRepository) : IRequestHandler<ObterClientePorIdQuery, ClienteDto>
{
    public async Task<ClienteDto> Handle(ObterClientePorIdQuery request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.ObterPorIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Cliente", request.Id);

        return ClienteDto.DeEntidade(cliente);
    }
}
