namespace MaintenSys.Application.Common;

public class NotFoundException(string entidade, object chave)
    : Exception($"{entidade} '{chave}' não foi encontrado(a).");

public class ConflictException(string mensagem) : Exception(mensagem);

public class AutenticacaoInvalidaException(string mensagem = "E-mail ou senha inválidos.") : Exception(mensagem);
