using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Transactions;

namespace Infrastructure.Transactions;

public class MongoDBTransactionCoordinator : IDbTransactionCoordinator
{
    private readonly ILogger<MongoDBTransactionCoordinator> _logger;
    private readonly IClientSessionHandle _clientSessionHandle;
    private readonly object _lockObject = new();

    private int _scopeTransactions;

    public MongoDBTransactionCoordinator(IClientSessionHandle clientSessionHandle, ILoggerFactory loggerFactory)
    {
        _clientSessionHandle = clientSessionHandle;
        _logger = loggerFactory.CreateLogger<MongoDBTransactionCoordinator>();
    }

    public bool IsInTransaction() => _clientSessionHandle.IsInTransaction;

    public void BeginTransaction()
    {
        lock (_lockObject)
        {
            if (_scopeTransactions == 0)
            {
                _logger.LogDebug("Starting transaction");
                _clientSessionHandle.StartTransaction(new MongoDB.Driver.TransactionOptions());
            }
            else
            {
                if (!IsInTransaction())
                {
                    throw new TransactionAbortedException(
                        "Attempt to start a transaction on already aborted transaction not allowed");
                }

                _logger.LogDebug("Transaction already in progress");
            }

            _scopeTransactions++;
        }
    }

    public Task CommitAsync() => CommitAsync(default);
    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        lock (_lockObject)
        {
            _scopeTransactions--;

            if (_scopeTransactions != 0)
            {
                if (!IsInTransaction())
                {
                    throw new TransactionAbortedException(
                        "Attempt to commit a transaction on already aborted or not started transaction not allowed");
                }

                _logger.LogDebug(
                    "{RemainingTransactions} operations active. Waiting for the final operation to complete before committing the transaction",
                    _scopeTransactions);

                return Task.CompletedTask;
            }

            _logger.LogDebug("Committing transaction");
            return _clientSessionHandle.CommitTransactionAsync(cancellationToken);
        }
    }

    public Task RollbackAsync() => RollbackAsync(default);
    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (!IsInTransaction())
        {
            _logger.LogDebug("Transaction already aborted");

            return Task.CompletedTask;
        }

        _logger.LogDebug("Aborting transaction");
        return _clientSessionHandle.AbortTransactionAsync(cancellationToken);
    }
}
