using Domain.Entities;
using Domain.Repositories;
using UseServiceScopeAndPaging.GraphQL.Attributes;

namespace UseServiceScopeAndPaging.GraphQL.Queries;

[ExtendObjectType("Query")]
public class ProductQuery
{
    [UseServiceScope(Order = 0)]
    [UseMongoTransactionMiddleware(Order = 1)]
    [UsePaging(IncludeTotalCount = true, Order = 2)]
    public IExecutable<Product> GetProductsAsync([Service] IProductRepository productRepository) =>
        productRepository.GetAllAsync();

    public Task<Product> GetProductAsync(string id, [Service] IProductRepository productRepository) =>
        productRepository.GetByIdAsync(id);
}
