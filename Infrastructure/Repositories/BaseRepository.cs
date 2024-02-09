using Domain.Entities;
using Domain.Repositories;
using HotChocolate;
using HotChocolate.Data;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    private readonly IMongoCollection<T> collection;

    private readonly IClientSessionHandle _sessionHandle;

    public BaseRepository(IClientSessionHandle sessionHandle, ICatalogContext catalogContext)
    {
        _sessionHandle = sessionHandle;

        if (catalogContext == null)
        {
            throw new ArgumentNullException(nameof(catalogContext));
        }

        this.collection = catalogContext.GetCollection<T>(typeof(T).Name);
    }

    public IExecutable<T> GetAllAsync()
    {
        return this.collection.Find(_sessionHandle, _ => true, default).AsExecutable();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        var filter = Builders<T>.Filter.Eq(_ => _.Id, id);

        return await this.collection.Find(_sessionHandle, filter, default).FirstOrDefaultAsync();
    }

    public async Task<T> InsertAsync(T entity)
    {
        await this.collection.InsertOneAsync(_sessionHandle, entity, default);

        return entity;
    }

    public async Task<bool> RemoveAsync(string id)
    {
        var result = await this.collection.DeleteOneAsync(_sessionHandle, Builders<T>.Filter.Eq(_ => _.Id, id), default);

        return result.DeletedCount > 0;
    }
}
