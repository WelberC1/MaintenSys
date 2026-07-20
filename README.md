# MaintenSys

API para gestão de assistências técnicas (celulares, notebooks, eletrônicos em geral), construída em **.NET 10** seguindo os princípios de **Clean Architecture**.

O objetivo do sistema é digitalizar o fluxo de uma assistência técnica: cadastro de clientes, cadastro dos aparelhos recebidos e controle completo da Ordem de Serviço (OS), da abertura até a entrega — incluindo a emissão da OS em PDF.

## Domínio do problema

O sistema tem dois perfis de usuário:

- **Técnico**: cadastra e consulta clientes, cadastra aparelhos (com fotos, defeito relatado, etc.), abre e conduz Ordens de Serviço.
- **Administrador**: tudo que o técnico faz, além de gerenciar usuários e acompanhar relatórios/faturamento.

### Fluxo principal

1. Técnico cadastra o **Cliente** (ou reutiliza um já existente).
2. Técnico cadastra o **Aparelho** entregue por esse cliente (tipo, marca, modelo, nº de série, ano, cor, acessórios entregues, fotos do estado atual).
3. Técnico abre uma **Ordem de Serviço** vinculando cliente + aparelho, registrando o defeito relatado.
4. A OS percorre um fluxo de status controlado:

   ```
   Aberta → Em Análise → Aguardando Aprovação do Orçamento → Em Conserto → Aguardando Peça → Concluída → Entregue
                                                                                 ↳ Cancelada (em qualquer etapa não finalizada)
   ```

5. Ao longo do processo são registrados: diagnóstico técnico, itens de orçamento (peças e serviços), prazo de garantia e um **histórico de auditoria** de cada mudança de status (quem alterou, quando, de qual status para qual).
6. Ao final, é possível **emitir a Ordem de Serviço em PDF**, pronta para impressão/entrega ao cliente.

## Arquitetura

O projeto é dividido em 4 camadas, cada uma em seu próprio projeto .NET:

```
MaintenSys.Domain        Entidades, enums, regras de negócio e contratos de repositório.
                          Não depende de nenhuma outra camada.

MaintenSys.Application    Casos de uso, DTOs e validações. Depende apenas do Domain.
                          (em desenvolvimento)

MaintenSys.Infra          Implementações concretas: EF Core, geração de PDF, Identity/JWT.
                          Depende de Domain e Application.
                          (em desenvolvimento)

MaintenSys.API             Controllers, autenticação e configuração da aplicação web.
                          Depende de Application e Infra.
                          (em desenvolvimento)
```

A regra de dependência segue o padrão de Clean Architecture: as camadas internas (Domain) não conhecem as externas, e toda comunicação com infraestrutura acontece através de interfaces definidas no Domain e implementadas na Infra.

### Modelo de domínio

- **Usuario** — Técnico ou Administrador, com autenticação.
- **Cliente** — dados de contato e documento (CPF/CNPJ).
- **Aparelho** — vinculado a um Cliente; guarda características do equipamento e fotos.
- **OrdemServico** — vincula Cliente + Aparelho + Técnico responsável; controla status, itens de orçamento e histórico de alterações.
- **ItemOrdemServico** — peça ou serviço lançado no orçamento de uma OS.
- **HistoricoStatusOrdemServico** — trilha de auditoria das mudanças de status da OS.

As regras de transição de status e as validações de cada entidade vivem na própria entidade (modelo rico), não em serviços externos.

## Stack

| Camada | Tecnologia |
|---|---|
| Runtime | .NET 10 |
| ORM | Entity Framework Core (SQL Server) |
| Autenticação | ASP.NET Core Identity + JWT |
| Validação | FluentValidation |
| Orquestração de casos de uso | MediatR |
| Geração de PDF | QuestPDF |
| Documentação da API | Swagger / OpenAPI |

## Funcionalidades planejadas

- [x] Modelagem do domínio (Cliente, Aparelho, Ordem de Serviço)
- [ ] Autenticação e autorização por perfil (Técnico / Administrador)
- [ ] CRUD de clientes
- [ ] Cadastro de aparelhos com upload de fotos
- [ ] Abertura e acompanhamento de Ordem de Serviço
- [ ] Orçamento (itens de peça/serviço) e cálculo automático do total
- [ ] Emissão da Ordem de Serviço em PDF
- [ ] Histórico/timeline de status da OS
- [ ] Relatórios administrativos (OS por status, faturamento por período, produtividade por técnico)

## Como rodar

> Em breve — assim que a camada de API e a configuração do banco de dados forem finalizadas.

## Status do projeto

🚧 Em desenvolvimento ativo. Este é um projeto pessoal de portfólio.
