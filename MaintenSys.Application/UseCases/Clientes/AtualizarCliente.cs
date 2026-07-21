using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.Clientes;

public record AtualizarClienteCommand(Guid Id, string Nome, string Telefone, string? Email, string? Endereco) : IRequest<ClienteDto>;

public class AtualizarClienteCommandValidator : AbstractValidator<AtualizarClienteCommand>
{
    public AtualizarClienteCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Telefone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}

public class AtualizarClienteCommandHandler(IClienteRepository clienteRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<AtualizarClienteCommand, ClienteDto>
{
    public async Task<ClienteDto> Handle(AtualizarClienteCommand request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.ObterPorIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Cliente", request.Id);

        cliente.AtualizarDados(request.Nome, request.Telefone, request.Email, request.Endereco);

        clienteRepository.Atualizar(cliente);
        await unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return ClienteDto.DeEntidade(cliente);
    }
}
