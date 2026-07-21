using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Enums;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.OrdensServico;

public record AlterarStatusCommand(Guid OrdemServicoId, StatusOrdemServico NovoStatus, Guid UsuarioId, string? Observacao)
    : IRequest<OrdemServicoDto>;

public class AlterarStatusCommandValidator : AbstractValidator<AlterarStatusCommand>
{
    public AlterarStatusCommandValidator()
    {
        RuleFor(x => x.OrdemServicoId).NotEmpty();
        RuleFor(x => x.NovoStatus).IsInEnum();
        RuleFor(x => x.UsuarioId).NotEmpty();
    }
}

public class AlterarStatusCommandHandler(IOrdemServicoRepository ordemServicoRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<AlterarStatusCommand, OrdemServicoDto>
{
    public async Task<OrdemServicoDto> Handle(AlterarStatusCommand request, CancellationToken cancellationToken)
    {
        var ordemServico = await ordemServicoRepository.ObterPorIdAsync(request.OrdemServicoId, cancellationToken)
            ?? throw new NotFoundException("Ordem de Serviço", request.OrdemServicoId);

        ordemServico.AlterarStatus(request.NovoStatus, request.UsuarioId, request.Observacao);

        ordemServicoRepository.Atualizar(ordemServico);
        await unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return OrdemServicoDto.DeEntidade(ordemServico);
    }
}
