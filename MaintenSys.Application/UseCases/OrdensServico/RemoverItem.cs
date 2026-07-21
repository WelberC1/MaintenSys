using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.OrdensServico;

public record RemoverItemCommand(Guid OrdemServicoId, Guid ItemId) : IRequest<OrdemServicoDto>;

public class RemoverItemCommandValidator : AbstractValidator<RemoverItemCommand>
{
    public RemoverItemCommandValidator()
    {
        RuleFor(x => x.OrdemServicoId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
    }
}

public class RemoverItemCommandHandler(IOrdemServicoRepository ordemServicoRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<RemoverItemCommand, OrdemServicoDto>
{
    public async Task<OrdemServicoDto> Handle(RemoverItemCommand request, CancellationToken cancellationToken)
    {
        var ordemServico = await ordemServicoRepository.ObterPorIdAsync(request.OrdemServicoId, cancellationToken)
            ?? throw new NotFoundException("Ordem de Serviço", request.OrdemServicoId);

        ordemServico.RemoverItem(request.ItemId);

        ordemServicoRepository.Atualizar(ordemServico);
        await unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return OrdemServicoDto.DeEntidade(ordemServico);
    }
}
