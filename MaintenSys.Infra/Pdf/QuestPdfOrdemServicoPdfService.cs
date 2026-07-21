using MaintenSys.Application.Interfaces;
using MaintenSys.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MaintenSys.Infra.Pdf;

public class QuestPdfOrdemServicoPdfService : IPdfService
{
    public byte[] GerarPdfOrdemServico(OrdemServico os)
    {
        var documento = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(coluna =>
                {
                    coluna.Item().Text("MaintenSys - Ordem de Serviço").FontSize(18).Bold();
                    coluna.Item().Text($"OS Nº {os.Id.ToString()[..8].ToUpperInvariant()}").FontSize(10).FontColor(Colors.Grey.Darken1);
                    coluna.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                });

                page.Content().PaddingVertical(15).Column(coluna =>
                {
                    coluna.Spacing(12);

                    coluna.Item().Element(c => SecaoCliente(c, os));
                    coluna.Item().Element(c => SecaoAparelho(c, os));
                    coluna.Item().Element(c => SecaoDefeitoDiagnostico(c, os));
                    coluna.Item().Element(c => SecaoItens(c, os));
                    coluna.Item().Element(c => SecaoDatasGarantia(c, os));
                    coluna.Item().Element(c => SecaoAssinatura(c));
                });

                page.Footer().AlignCenter().Text(texto =>
                {
                    texto.Span("Página ");
                    texto.CurrentPageNumber();
                    texto.Span(" de ");
                    texto.TotalPages();
                });
            });
        });

        return documento.GeneratePdf();
    }

    private static void SecaoCliente(IContainer container, OrdemServico os)
    {
        container.Column(coluna =>
        {
            coluna.Item().Text("Cliente").Bold().FontSize(12);
            coluna.Item().Text($"Nome: {os.Cliente?.Nome}");
            coluna.Item().Text($"CPF/CNPJ: {os.Cliente?.CpfCnpj}    Telefone: {os.Cliente?.Telefone}");
            if (!string.IsNullOrWhiteSpace(os.Cliente?.Email))
                coluna.Item().Text($"E-mail: {os.Cliente.Email}");
        });
    }

    private static void SecaoAparelho(IContainer container, OrdemServico os)
    {
        container.Column(coluna =>
        {
            coluna.Item().Text("Aparelho").Bold().FontSize(12);
            coluna.Item().Text($"Tipo: {os.Aparelho?.Tipo}    Marca: {os.Aparelho?.Marca}    Modelo: {os.Aparelho?.Modelo}");
            coluna.Item().Text($"Nº de série: {os.Aparelho?.NumeroSerie ?? "-"}    Cor: {os.Aparelho?.Cor ?? "-"}    Ano: {os.Aparelho?.Ano?.ToString() ?? "-"}");
            if (!string.IsNullOrWhiteSpace(os.Aparelho?.Acessorios))
                coluna.Item().Text($"Acessórios entregues: {os.Aparelho.Acessorios}");
        });
    }

    private static void SecaoDefeitoDiagnostico(IContainer container, OrdemServico os)
    {
        container.Column(coluna =>
        {
            coluna.Item().Text("Defeito relatado / Diagnóstico").Bold().FontSize(12);
            coluna.Item().Text($"Defeito relatado pelo cliente: {os.DefeitoRelatado}");
            coluna.Item().Text($"Diagnóstico técnico: {os.DiagnosticoTecnico ?? "Ainda não informado"}");
            coluna.Item().Text($"Status atual: {os.Status}    Técnico responsável: {os.Tecnico?.Nome ?? "-"}");
        });
    }

    private static void SecaoItens(IContainer container, OrdemServico os)
    {
        container.Column(coluna =>
        {
            coluna.Item().Text("Orçamento").Bold().FontSize(12);

            coluna.Item().Table(tabela =>
            {
                tabela.ColumnsDefinition(colunas =>
                {
                    colunas.RelativeColumn(4);
                    colunas.RelativeColumn(2);
                    colunas.RelativeColumn(1);
                    colunas.RelativeColumn(2);
                    colunas.RelativeColumn(2);
                });

                tabela.Header(cabecalho =>
                {
                    cabecalho.Cell().Text("Descrição").Bold();
                    cabecalho.Cell().Text("Tipo").Bold();
                    cabecalho.Cell().Text("Qtd").Bold();
                    cabecalho.Cell().Text("Vlr. Unit.").Bold();
                    cabecalho.Cell().Text("Total").Bold();
                    cabecalho.Cell().ColumnSpan(5).PaddingTop(2).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                });

                foreach (var item in os.Itens)
                {
                    tabela.Cell().Text(item.Descricao);
                    tabela.Cell().Text(item.Tipo.ToString());
                    tabela.Cell().Text(item.Quantidade.ToString());
                    tabela.Cell().Text(item.ValorUnitario.ToString("C2"));
                    tabela.Cell().Text(item.ValorTotal.ToString("C2"));
                }
            });

            coluna.Item().AlignRight().Text($"Valor total: {os.ValorTotal:C2}").Bold().FontSize(12);
        });
    }

    private static void SecaoDatasGarantia(IContainer container, OrdemServico os)
    {
        container.Column(coluna =>
        {
            coluna.Item().Text("Datas e garantia").Bold().FontSize(12);
            coluna.Item().Text($"Entrada: {os.DataEntrada:dd/MM/yyyy HH:mm}    Previsão de entrega: {os.DataPrevisaoEntrega?.ToString("dd/MM/yyyy") ?? "-"}");
            coluna.Item().Text($"Conclusão: {os.DataConclusao?.ToString("dd/MM/yyyy HH:mm") ?? "-"}    Entrega: {os.DataEntrega?.ToString("dd/MM/yyyy HH:mm") ?? "-"}");
            coluna.Item().Text($"Garantia: {(os.PrazoGarantiaDias.HasValue ? $"{os.PrazoGarantiaDias} dias" : "Não definida")}");
        });
    }

    private static void SecaoAssinatura(IContainer container)
    {
        container.PaddingTop(30).Row(linha =>
        {
            linha.RelativeItem().Column(coluna =>
            {
                coluna.Item().LineHorizontal(1).LineColor(Colors.Grey.Darken1);
                coluna.Item().AlignCenter().Text("Assinatura do Técnico");
            });

            linha.ConstantItem(30);

            linha.RelativeItem().Column(coluna =>
            {
                coluna.Item().LineHorizontal(1).LineColor(Colors.Grey.Darken1);
                coluna.Item().AlignCenter().Text("Assinatura do Cliente");
            });
        });
    }
}
