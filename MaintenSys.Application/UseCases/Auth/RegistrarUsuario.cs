using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Application.Interfaces;
using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Enums;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.Auth;

public record RegistrarUsuarioCommand(string Nome, string Email, string Senha, TipoUsuario Tipo) : IRequest<UsuarioDto>;

public class RegistrarUsuarioCommandValidator : AbstractValidator<RegistrarUsuarioCommand>
{
    public RegistrarUsuarioCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Senha).NotEmpty().MinimumLength(6);
        RuleFor(x => x.Tipo).IsInEnum();
    }
}

public class RegistrarUsuarioCommandHandler(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    : IRequestHandler<RegistrarUsuarioCommand, UsuarioDto>
{
    public async Task<UsuarioDto> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        var existente = await usuarioRepository.ObterPorEmailAsync(request.Email, cancellationToken);
        if (existente is not null)
            throw new ConflictException($"Já existe um usuário cadastrado com o e-mail '{request.Email}'.");

        var senhaHash = passwordHasher.Hash(request.Senha);
        var usuario = new Usuario(request.Nome, request.Email, senhaHash, request.Tipo);

        await usuarioRepository.AdicionarAsync(usuario, cancellationToken);
        await unitOfWork.SalvarAlteracoesAsync(cancellationToken);

        return UsuarioDto.DeEntidade(usuario);
    }
}
