using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.Aparelhos;

public record ObterAparelhosPorClienteQuery(Guid ClienteId) : IRequest<IReadOnlyList<AparelhoDto>>;

public class ObterAparelhosPorClienteQueryHandler(IAparelhoRepository aparelhoRepository)
    : IRequestHandler<ObterAparelhosPorClienteQuery, IReadOnlyList<AparelhoDto>>
{
    public async Task<IReadOnlyList<AparelhoDto>> Handle(ObterAparelhosPorClienteQuery request, CancellationToken cancellationToken)
    {
        var aparelhos = await aparelhoRepository.ObterPorClienteAsync(request.ClienteId, cancellationToken);
        return aparelhos.Select(AparelhoDto.DeEntidade).ToList();
    }
}
