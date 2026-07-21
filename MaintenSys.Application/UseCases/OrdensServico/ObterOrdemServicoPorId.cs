using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.OrdensServico;

public record ObterOrdemServicoPorIdQuery(Guid Id) : IRequest<OrdemServicoDto>;

public class ObterOrdemServicoPorIdQueryHandler(IOrdemServicoRepository ordemServicoRepository)
    : IRequestHandler<ObterOrdemServicoPorIdQuery, OrdemServicoDto>
{
    public async Task<OrdemServicoDto> Handle(ObterOrdemServicoPorIdQuery request, CancellationToken cancellationToken)
    {
        var ordemServico = await ordemServicoRepository.ObterCompletaPorIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Ordem de Serviço", request.Id);

        return OrdemServicoDto.DeEntidade(ordemServico);
    }
}
