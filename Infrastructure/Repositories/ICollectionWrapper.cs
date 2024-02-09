using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public interface ICollectionWrapper<T>
{
    public IFindFluent<T, T> Find(FilterDefinition<T> filter, FindOptions? findOptions = default);

    public IFindFluent<T, T> Find(Expression<Func<T, bool>> predicate, FindOptions? findOptions = default);

    public Task<T?> FindFirstOrDefaultAsync(FilterDefinition<T> filter, FindOptions? findOptions = default, CancellationToken cancellationToken = default);

    public Task<T?> FindFirstOrDefaultAsync(FilterDefinition<T> filter, SortDefinition<T> sort, FindOptions? findOptions = default, CancellationToken cancellationToken = default);

    public Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, FindOptions? findOptions = default, CancellationToken cancellationToken = default);

    public IFindFluent<T, TDest> Project<TDest>(IFindFluent<T, T> findFluent, Expression<Func<T, TDest>> projection);

    public Task<List<TDest>> ToListAsync<TDest>(IFindFluent<T, TDest> findFluent, CancellationToken cancellationToken = default);

    public Task<List<TDest>> ToListAsync<TDest>(IAggregateFluent<TDest> aggregateFluent, CancellationToken cancellationToken = default);

    public Task InsertOneAsync(T entity, CancellationToken cancellationToken = default);

    public Task InsertManyAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    public Task<string> CreateCompoundIndexAsync(string ascending, string descending, CancellationToken cancellationToken);

    public Task<string> CreateIndexAsync(IndexKeysDefinition<T> indexKeysDefinition, CreateIndexOptions? indexOptions = default, CancellationToken cancellationToken = default);

    public Task<UpdateResult> UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, CancellationToken cancellationToken = default);

    public Task<UpdateResult> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, CancellationToken cancellationToken = default);

    public Task<ReplaceOneResult> ReplaceOneAsync(FilterDefinition<T> filter, T updatedEntity, ReplaceOptions? replaceOptions = default, CancellationToken cancellationToken = default);

    public Task<ReplaceOneResult> ReplaceOneAsync(Expression<Func<T, bool>> predicate, T entity, ReplaceOptions? replaceOptions = default, CancellationToken cancellationToken = default);

    public Task<DeleteResult> DeleteOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    public Task<DeleteResult> DeleteOneAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

    public Task<DeleteResult> DeleteManyAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

    public IAggregateFluent<T> Aggregate(AggregateOptions? options = null);

    public IMongoQueryable<T> AsQueryable(AggregateOptions? options = null);

    public Task<long> CountAsync(FilterDefinition<T> filter);

    public Task<IAsyncCursor<TField>> DistinctAsync<TField>(Expression<Func<T, TField>> field, Expression<Func<T, bool>> filter, DistinctOptions? options = null, CancellationToken cancellationToken = default);

    public Task<IAsyncCursor<TField>> DistinctAsync<TField>(FieldDefinition<T, TField> field, Expression<Func<T, bool>> filter, DistinctOptions? options = null, CancellationToken cancellationToken = default);

    public Task<bool> AnyAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);
}
