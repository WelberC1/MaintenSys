using MaintenSys.Domain.Interfaces;

namespace MaintenSys.Infra.Persistence;

public class UnitOfWork(MaintenSysDbContext context) : IUnitOfWork
{
    public Task<int> SalvarAlteracoesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
