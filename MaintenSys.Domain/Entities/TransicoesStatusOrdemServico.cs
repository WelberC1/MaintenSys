using MaintenSys.Domain.Enums;

namespace MaintenSys.Domain.Entities;

internal static class TransicoesStatusOrdemServico
{
    private static readonly Dictionary<StatusOrdemServico, StatusOrdemServico[]> Permitidas = new()
    {
        [StatusOrdemServico.Aberta] = [StatusOrdemServico.EmAnalise, StatusOrdemServico.Cancelada],
        [StatusOrdemServico.EmAnalise] = [StatusOrdemServico.AguardandoAprovacaoOrcamento, StatusOrdemServico.EmConserto, StatusOrdemServico.Cancelada],
        [StatusOrdemServico.AguardandoAprovacaoOrcamento] = [StatusOrdemServico.EmConserto, StatusOrdemServico.Cancelada],
        [StatusOrdemServico.EmConserto] = [StatusOrdemServico.AguardandoPeca, StatusOrdemServico.Concluida, StatusOrdemServico.Cancelada],
        [StatusOrdemServico.AguardandoPeca] = [StatusOrdemServico.EmConserto, StatusOrdemServico.Cancelada],
        [StatusOrdemServico.Concluida] = [StatusOrdemServico.Entregue],
        [StatusOrdemServico.Entregue] = [],
        [StatusOrdemServico.Cancelada] = []
    };

    public static bool PodeTransicionar(StatusOrdemServico atual, StatusOrdemServico novo)
        => Permitidas.TryGetValue(atual, out var permitidos) && permitidos.Contains(novo);
}
