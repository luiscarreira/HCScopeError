using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(IClientSessionHandle sessionHandle, ICatalogContext catalogContext) : base(sessionHandle, catalogContext)
    {
    }
}
