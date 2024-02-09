using Domain.Entities;
using Domain.Repositories;

namespace UseServiceScopeAndPaging.GraphQL.Resolvers;

[ExtendObjectType("Category")]
public class CategoryResolver
{
    public Task<Category> GetCategoryAsync([Parent] Product product, [Service] ICategoryRepository categoryRepository) =>
        categoryRepository.GetByIdAsync(product.CategoryId);
}
