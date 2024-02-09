using MongoDB.Driver;

namespace Infrastructure.Data;

public interface ICatalogContext
{
    IMongoCollection<T> GetCollection<T>(string name);
}
