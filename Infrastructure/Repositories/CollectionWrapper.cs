using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class CollectionWrapper<T> : ICollectionWrapper<T>
{
    private readonly IClientSessionHandle _sessionHandle;
    private readonly IMongoCollection<T> _collection;

    protected CollectionWrapper(IClientSessionHandle sessionHandle, IMongoCollection<T> collection)
    {
        _sessionHandle = sessionHandle ?? throw new ArgumentNullException(nameof(sessionHandle));
        _collection = collection ?? throw new ArgumentNullException(nameof(collection));
    }

    public virtual IFindFluent<T, T> Find(FilterDefinition<T> filter, FindOptions? findOptions = default)
        => _collection.Find(_sessionHandle, filter, findOptions);

    public virtual IFindFluent<T, T> Find(Expression<Func<T, bool>> predicate, FindOptions? findOptions = default)
        => _collection.Find(_sessionHandle, predicate, findOptions);

    public virtual Task<T?> FindFirstOrDefaultAsync(FilterDefinition<T> filter, FindOptions? findOptions = default, CancellationToken cancellationToken = default)
         => _collection.Find(_sessionHandle, filter, findOptions).FirstOrDefaultAsync(cancellationToken)!;

    public virtual Task<T?> FindFirstOrDefaultAsync(FilterDefinition<T> filter, SortDefinition<T> sort, FindOptions? findOptions = default, CancellationToken cancellationToken = default)
        => _collection.Find(_sessionHandle, filter, findOptions).Sort(sort).FirstOrDefaultAsync(cancellationToken)!;

    public virtual Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, FindOptions? findOptions = default, CancellationToken cancellationToken = default)
        => _collection.Find(_sessionHandle, predicate, findOptions).FirstOrDefaultAsync(cancellationToken)!;

    public virtual IFindFluent<T, TDest> Project<TDest>(IFindFluent<T, T> findFluent, Expression<Func<T, TDest>> projection)
        => findFluent.Project(projection);

    public virtual Task<List<TDest>> ToListAsync<TDest>(IFindFluent<T, TDest> findFluent, CancellationToken cancellationToken = default)
        => findFluent.ToListAsync(cancellationToken);

    public virtual Task<List<TDest>> ToListAsync<TDest>(IAggregateFluent<TDest> aggregateFluent, CancellationToken cancellationToken = default)
        => aggregateFluent.ToListAsync(cancellationToken);

    public virtual Task InsertOneAsync(T entity, CancellationToken cancellationToken = default)
        => _collection.InsertOneAsync(_sessionHandle, entity, null, cancellationToken);

    public virtual Task InsertManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        => _collection.InsertManyAsync(_sessionHandle, entities, new InsertManyOptions(), cancellationToken);

    public virtual async Task<string> CreateCompoundIndexAsync(string ascending, string descending, CancellationToken cancellationToken)
    {
        var builder = Builders<T>.IndexKeys
            .Ascending(ascending)
            .Descending(descending);
        var indexOptions = new CreateIndexOptions { Unique = true };

        return await _collection.Indexes.CreateOneAsync(new CreateIndexModel<T>(builder, indexOptions), cancellationToken: cancellationToken);
    }

    public virtual Task<string> CreateIndexAsync(IndexKeysDefinition<T> indexKeysDefinition, CreateIndexOptions? indexOptions = default, CancellationToken cancellationToken = default) => _collection.Indexes.CreateOneAsync(new CreateIndexModel<T>(indexKeysDefinition, indexOptions), cancellationToken: cancellationToken);

    public virtual Task<UpdateResult> UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, CancellationToken cancellationToken = default)
        => _collection.UpdateOneAsync(_sessionHandle, filter, update, cancellationToken: cancellationToken);

    public virtual Task<UpdateResult> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, CancellationToken cancellationToken = default)
        => _collection.UpdateManyAsync(_sessionHandle, filter, update, cancellationToken: cancellationToken);

    public virtual Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<T> filter, T updatedEntity, ReplaceOptions? replaceOptions = default, CancellationToken cancellationToken = default)
        => _collection.ReplaceOneAsync(_sessionHandle, filter, updatedEntity, replaceOptions, cancellationToken: cancellationToken);

    public virtual Task<ReplaceOneResult> ReplaceOneAsync(Expression<Func<T, bool>> predicate, T entity, ReplaceOptions? replaceOptions = default, CancellationToken cancellationToken = default)
        => _collection.ReplaceOneAsync(_sessionHandle, predicate, entity, replaceOptions, cancellationToken: cancellationToken);

    public virtual Task<DeleteResult> DeleteOneAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        => _collection.DeleteOneAsync(session: _sessionHandle, filter, cancellationToken: cancellationToken);

    public virtual Task<DeleteResult> DeleteOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
        => _collection.DeleteOneAsync(session: _sessionHandle, filter: filter, cancellationToken: cancellationToken);

    public virtual Task<DeleteResult> DeleteManyAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
        => _collection.DeleteManyAsync(session: _sessionHandle, filter: filter, cancellationToken: cancellationToken);

    public virtual IAggregateFluent<T> Aggregate(AggregateOptions? options = null)
        => _collection.Aggregate(_sessionHandle, options);

    public virtual IMongoQueryable<T> AsQueryable(AggregateOptions? options = null)
        => _collection.AsQueryable(options);

    public virtual Task<long> CountAsync(FilterDefinition<T> filter) => _collection.CountDocumentsAsync(filter);

    public virtual Task<IAsyncCursor<TField>> DistinctAsync<TField>(Expression<Func<T, TField>> field, Expression<Func<T, bool>> filter, DistinctOptions? options = null, CancellationToken cancellationToken = default)
        => _collection.DistinctAsync(_sessionHandle, field, filter, options, cancellationToken);

    public virtual Task<IAsyncCursor<TField>> DistinctAsync<TField>(FieldDefinition<T, TField> field, Expression<Func<T, bool>> filter, DistinctOptions? options = null, CancellationToken cancellationToken = default)
        => _collection.DistinctAsync<T, TField>(_sessionHandle, field, filter, options, cancellationToken);

    public virtual Task<bool> AnyAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
        => _collection.Find(filter).AnyAsync(cancellationToken);
}
