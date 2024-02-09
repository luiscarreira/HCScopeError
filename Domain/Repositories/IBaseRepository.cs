using Domain.Entities;
using HotChocolate;

namespace Domain.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    IExecutable<T> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task<T> InsertAsync(T entity);
    Task<bool> RemoveAsync(string id);
}
