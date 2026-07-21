using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.Clientes;

public record CriarClienteCommand(string Nome, string CpfCnpj, string Telefone, string? Email, string? Endereco) : IRequest<ClienteDto>;

public class CriarClienteCommandValidator : AbstractValidator<CriarClienteCommand>
{
    public CriarClienteCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(150);
        RuleFor(x => x.CpfCnpj).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Telefone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}

public class CriarClienteCommandHandler(IClienteRepository clienteRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CriarClienteCommand, ClienteDto>
{
    public async Task<ClienteDto> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        var existente = await clienteRepository.ObterPorCpfCnpjAsync(request.CpfCnpj, cancellationToken);
        if (existente is not null)
            throw new ConflictException($"Já existe um cliente cadastrado com o CPF/CNPJ '{request.CpfCnpj}'.");

        var cliente = new Cliente(request.Nome, request.CpfCnpj, request.Telefone, request.Email, request.Endereco);

        await clienteRepository.AdicionarAsync(cliente, cancellationToken);
        await unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return ClienteDto.DeEntidade(cliente);
    }
}
