using MaintenSys.Domain.Entities;

namespace MaintenSys.Application.DTOs;

public record ClienteDto(
    Guid Id,
    string Nome,
    string CpfCnpj,
    string Telefone,
    string? Email,
    string? Endereco,
    DateTime CreatedAt)
{
    public static ClienteDto DeEntidade(Cliente cliente) => new(
        cliente.Id,
        cliente.Nome,
        cliente.CpfCnpj,
        cliente.Telefone,
        cliente.Email,
        cliente.Endereco,
        cliente.CreatedAt);
}
