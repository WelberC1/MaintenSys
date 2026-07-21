using System.Net;
using System.Text.Json;
using FluentValidation;
using MaintenSys.Application.Common;
using MaintenSys.Domain.Common;

namespace MaintenSys.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await TratarExcecaoAsync(context, ex);
        }
    }

    private async Task TratarExcecaoAsync(HttpContext context, Exception ex)
    {
        var (statusCode, mensagem, erros) = ex switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                "Um ou mais campos são inválidos.",
                validationEx.Errors.Select(e => e.ErrorMessage).ToArray()),

            NotFoundException notFoundEx => (HttpStatusCode.NotFound, notFoundEx.Message, Array.Empty<string>()),
            ConflictException conflictEx => (HttpStatusCode.Conflict, conflictEx.Message, Array.Empty<string>()),
            AutenticacaoInvalidaException authEx => (HttpStatusCode.Unauthorized, authEx.Message, Array.Empty<string>()),
            DomainException domainEx => (HttpStatusCode.BadRequest, domainEx.Message, Array.Empty<string>()),
            _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado ao processar a requisição.", Array.Empty<string>())
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            logger.LogError(ex, "Erro não tratado ao processar a requisição {Path}", context.Request.Path);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var corpo = JsonSerializer.Serialize(new { mensagem, erros });
        await context.Response.WriteAsync(corpo);
    }
}
