using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Application.Interfaces;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.Aparelhos;

public record AdicionarFotoAparelhoCommand(Guid AparelhoId, Stream ConteudoArquivo, string NomeArquivoOriginal, string? Descricao)
    : IRequest<AparelhoDto>;

public class AdicionarFotoAparelhoCommandValidator : AbstractValidator<AdicionarFotoAparelhoCommand>
{
    public AdicionarFotoAparelhoCommandValidator()
    {
        RuleFor(x => x.AparelhoId).NotEmpty();
        RuleFor(x => x.NomeArquivoOriginal).NotEmpty();
    }
}

public class AdicionarFotoAparelhoCommandHandler(
    IAparelhoRepository aparelhoRepository,
    IArquivoStorageService arquivoStorageService,
    IUnitOfWork unitOfWork) : IRequestHandler<AdicionarFotoAparelhoCommand, AparelhoDto>
{
    public async Task<AparelhoDto> Handle(AdicionarFotoAparelhoCommand request, CancellationToken cancellationToken)
    {
        var aparelho = await aparelhoRepository.ObterPorIdAsync(request.AparelhoId, cancellationToken)
            ?? throw new NotFoundException("Aparelho", request.AparelhoId);

        var caminhoArquivo = await arquivoStorageService.SalvarAsync(request.ConteudoArquivo, request.NomeArquivoOriginal, cancellationToken);
        aparelho.AdicionarFoto(caminhoArquivo, request.Descricao);

        aparelhoRepository.Atualizar(aparelho);
        await unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return AparelhoDto.DeEntidade(aparelho);
    }
}
