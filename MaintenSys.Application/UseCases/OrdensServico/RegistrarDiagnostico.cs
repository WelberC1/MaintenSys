using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.OrdensServico;

public record RegistrarDiagnosticoCommand(Guid OrdemServicoId, string Diagnostico) : IRequest<OrdemServicoDto>;

public class RegistrarDiagnosticoCommandValidator : AbstractValidator<RegistrarDiagnosticoCommand>
{
    public RegistrarDiagnosticoCommandValidator()
    {
        RuleFor(x => x.OrdemServicoId).NotEmpty();
        RuleFor(x => x.Diagnostico).NotEmpty().MaximumLength(2000);
    }
}

public class RegistrarDiagnosticoCommandHandler(IOrdemServicoRepository ordemServicoRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<RegistrarDiagnosticoCommand, OrdemServicoDto>
{
    public async Task<OrdemServicoDto> Handle(RegistrarDiagnosticoCommand request, CancellationToken cancellationToken)
    {
        var ordemServico = await ordemServicoRepository.ObterPorIdAsync(request.OrdemServicoId, cancellationToken)
            ?? throw new NotFoundException("Ordem de Serviço", request.OrdemServicoId);

        ordemServico.RegistrarDiagnostico(request.Diagnostico);

        ordemServicoRepository.Atualizar(ordemServico);
        await unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return OrdemServicoDto.DeEntidade(ordemServico);
    }
}
