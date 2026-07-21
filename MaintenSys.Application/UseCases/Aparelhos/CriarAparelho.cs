using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.Aparelhos;

public record CriarAparelhoCommand(
    Guid ClienteId,
    string Tipo,
    string Marca,
    string Modelo,
    string? NumeroSerie,
    string? Cor,
    int? Ano,
    string? Acessorios) : IRequest<AparelhoDto>;

public class CriarAparelhoCommandValidator : AbstractValidator<CriarAparelhoCommand>
{
    public CriarAparelhoCommandValidator()
    {
        RuleFor(x => x.ClienteId).NotEmpty();
        RuleFor(x => x.Tipo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Marca).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Modelo).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Ano).InclusiveBetween(1980, DateTime.UtcNow.Year + 1).When(x => x.Ano.HasValue);
    }
}

public class CriarAparelhoCommandHandler(IAparelhoRepository aparelhoRepository, IClienteRepository clienteRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CriarAparelhoCommand, AparelhoDto>
{
    public async Task<AparelhoDto> Handle(CriarAparelhoCommand request, CancellationToken cancellationToken)
    {
        _ = await clienteRepository.ObterPorIdAsync(request.ClienteId, cancellationToken)
            ?? throw new NotFoundException("Cliente", request.ClienteId);

        var aparelho = new Aparelho(
            request.ClienteId, request.Tipo, request.Marca, request.Modelo,
            request.NumeroSerie, request.Cor, request.Ano, request.Acessorios);

        await aparelhoRepository.AdicionarAsync(aparelho, cancellationToken);
        await unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return AparelhoDto.DeEntidade(aparelho);
    }
}
