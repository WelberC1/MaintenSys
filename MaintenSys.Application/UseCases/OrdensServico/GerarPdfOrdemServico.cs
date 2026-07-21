using MaintenSys.Application.Common;
using MaintenSys.Application.Interfaces;
using MaintenSys.Domain.Interfaces;
using MediatR;

namespace MaintenSys.Application.UseCases.OrdensServico;

public record GerarPdfOrdemServicoQuery(Guid Id) : IRequest<byte[]>;

public class GerarPdfOrdemServicoQueryHandler(IOrdemServicoRepository ordemServicoRepository, IPdfService pdfService)
    : IRequestHandler<GerarPdfOrdemServicoQuery, byte[]>
{
    public async Task<byte[]> Handle(GerarPdfOrdemServicoQuery request, CancellationToken cancellationToken)
    {
        var ordemServico = await ordemServicoRepository.ObterCompletaPorIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Ordem de Serviço", request.Id);

        return pdfService.GerarPdfOrdemServico(ordemServico);
    }
}
