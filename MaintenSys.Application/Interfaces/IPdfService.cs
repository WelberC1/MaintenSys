using MaintenSys.Domain.Entities;

namespace MaintenSys.Application.Interfaces;

public interface IPdfService
{
    byte[] GerarPdfOrdemServico(OrdemServico ordemServico);
}
