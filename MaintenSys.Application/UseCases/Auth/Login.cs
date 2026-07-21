using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Application.DTOs;
using MaintenSys.Application.Interfaces;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.Auth;

public record LoginCommand(string Email, string Senha) : IRequest<LoginResultDto>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Senha).NotEmpty();
    }
}

public class LoginCommandHandler(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, LoginResultDto>
{
    public async Task<LoginResultDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.ObterPorEmailAsync(request.Email, cancellationToken);
        if (usuario is null || !usuario.Ativo || !passwordHasher.Verificar(usuario.SenhaHash, request.Senha))
            throw new AutenticacaoInvalidaException();

        var token = jwtTokenService.GerarToken(usuario);
        return new LoginResultDto(token, UsuarioDto.DeEntidade(usuario));
    }
}
