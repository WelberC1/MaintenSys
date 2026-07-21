using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.OrdensServico;

public record DefinirGarantiaCommand(Guid OrdemServicoId, int PrazoGarantiaDias) : IRequest<OrdemServicoDto>;

public class DefinirGarantiaCommandValidator : AbstractValidator<DefinirGarantiaCommand>
{
    public DefinirGarantiaCommandValidator()
    {
        RuleFor(x => x.OrdemServicoId).NotEmpty();
        RuleFor(x => x.PrazoGarantiaDias).GreaterThanOrEqualTo(0);
    }
}

public class DefinirGarantiaCommandHandler(IOrdemServicoRepository ordemServicoRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DefinirGarantiaCommand, OrdemServicoDto>
{
    public async Task<OrdemServicoDto> Handle(DefinirGarantiaCommand request, CancellationToken cancellationToken)
    {
        var ordemServico = await ordemServicoRepository.ObterPorIdAsync(request.OrdemServicoId, cancellationToken)
            ?? throw new NotFoundException("Ordem de Serviço", request.OrdemServicoId);

        ordemServico.DefinirGarantia(request.PrazoGarantiaDias);

        ordemServicoRepository.Atualizar(ordemServico);
        await unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return OrdemServicoDto.DeEntidade(ordemServico);
    }
}
