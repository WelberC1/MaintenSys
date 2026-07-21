using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Enums;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.OrdensServico;

public record AdicionarItemCommand(Guid OrdemServicoId, string Descricao, TipoItemOrdemServico Tipo, int Quantidade, decimal ValorUnitario)
    : IRequest<OrdemServicoDto>;

public class AdicionarItemCommandValidator : AbstractValidator<AdicionarItemCommand>
{
    public AdicionarItemCommandValidator()
    {
        RuleFor(x => x.OrdemServicoId).NotEmpty();
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Tipo).IsInEnum();
        RuleFor(x => x.Quantidade).GreaterThan(0);
        RuleFor(x => x.ValorUnitario).GreaterThanOrEqualTo(0);
    }
}

public class AdicionarItemCommandHandler(IOrdemServicoRepository ordemServicoRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<AdicionarItemCommand, OrdemServicoDto>
{
    public async Task<OrdemServicoDto> Handle(AdicionarItemCommand request, CancellationToken cancellationToken)
    {
        var ordemServico = await ordemServicoRepository.ObterPorIdAsync(request.OrdemServicoId, cancellationToken)
            ?? throw new NotFoundException("Ordem de Serviço", request.OrdemServicoId);

        ordemServico.AdicionarItem(request.Descricao, request.Tipo, request.Quantidade, request.ValorUnitario);

        ordemServicoRepository.Atualizar(ordemServico);
        await unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return OrdemServicoDto.DeEntidade(ordemServico);
    }
}
