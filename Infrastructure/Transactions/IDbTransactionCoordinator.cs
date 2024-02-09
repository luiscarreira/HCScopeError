namespace Infrastructure.Transactions;

public interface IDbTransactionCoordinator
{
    bool IsInTransaction();
    void BeginTransaction();
    Task CommitAsync();
    Task CommitAsync(CancellationToken cancellationToken);
    Task RollbackAsync();
    Task RollbackAsync(CancellationToken cancellationToken);
}
