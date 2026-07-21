using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.OrdensServico;

public record AbrirOrdemServicoCommand(
    Guid ClienteId,
    Guid AparelhoId,
    Guid TecnicoId,
    string DefeitoRelatado,
    DateTime? DataPrevisaoEntrega) : IRequest<OrdemServicoDto>;

public class AbrirOrdemServicoCommandValidator : AbstractValidator<AbrirOrdemServicoCommand>
{
    public AbrirOrdemServicoCommandValidator()
    {
        RuleFor(x => x.ClienteId).NotEmpty();
        RuleFor(x => x.AparelhoId).NotEmpty();
        RuleFor(x => x.TecnicoId).NotEmpty();
        RuleFor(x => x.DefeitoRelatado).NotEmpty().MaximumLength(1000);
    }
}

public class AbrirOrdemServicoCommandHandler(
    IOrdemServicoRepository ordemServicoRepository,
    IClienteRepository clienteRepository,
    IAparelhoRepository aparelhoRepository,
    IUsuarioRepository usuarioRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AbrirOrdemServicoCommand, OrdemServicoDto>
{
    public async Task<OrdemServicoDto> Handle(AbrirOrdemServicoCommand request, CancellationToken cancellationToken)
    {
        _ = await clienteRepository.ObterPorIdAsync(request.ClienteId, cancellationToken)
            ?? throw new NotFoundException("Cliente", request.ClienteId);

        var aparelho = await aparelhoRepository.ObterPorIdAsync(request.AparelhoId, cancellationToken)
            ?? throw new NotFoundException("Aparelho", request.AparelhoId);

        if (aparelho.ClienteId != request.ClienteId)
            throw new ConflictException("O aparelho informado não pertence ao cliente informado.");

        _ = await usuarioRepository.ObterPorIdAsync(request.TecnicoId, cancellationToken)
            ?? throw new NotFoundException("Técnico", request.TecnicoId);

        var ordemServico = new OrdemServico(request.ClienteId, request.AparelhoId, request.TecnicoId, request.DefeitoRelatado, request.DataPrevisaoEntrega);

        await ordemServicoRepository.AdicionarAsync(ordemServico, cancellationToken);
        await unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return OrdemServicoDto.DeEntidade(ordemServico);
    }
}
