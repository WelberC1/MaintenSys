using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.Aparelhos;

public record ObterAparelhoPorIdQuery(Guid Id) : IRequest<AparelhoDto>;

public class ObterAparelhoPorIdQueryHandler(IAparelhoRepository aparelhoRepository) : IRequestHandler<ObterAparelhoPorIdQuery, AparelhoDto>
{
    public async Task<AparelhoDto> Handle(ObterAparelhoPorIdQuery request, CancellationToken cancellationToken)
    {
        var aparelho = await aparelhoRepository.ObterPorIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Aparelho", request.Id);

        return AparelhoDto.DeEntidade(aparelho);
    }
}
