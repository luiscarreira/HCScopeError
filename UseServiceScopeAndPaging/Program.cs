using Domain.Repositories;
using Infrastructure.Configurations;
using Infrastructure.Data;
using Infrastructure.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using UseServiceScopeAndPaging.Configurations;
using UseServiceScopeAndPaging.GraphQL.Queries;
using UseServiceScopeAndPaging.GraphQL.Resolvers;
using UseServiceScopeAndPaging.GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddSingleton(configuration.Get<ApiConfiguration>().MongoDbConfiguration);
builder.Services.AddSingleton<ICatalogContext, CatalogContext>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddSingleton<IMongoClient>(provider =>
{
    var configuration = provider.GetRequiredService<MongoDbConfiguration>();
    var mongoClientSettings = MongoClientSettings.FromConnectionString(configuration.ConnectionString);
    mongoClientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());

    return new MongoClient(mongoClientSettings);
});
builder.Services.AddScoped<IClientSessionHandle>(provider => 
{
    return provider.GetRequiredService<IMongoClient>().StartSession();
});

// GraphQL
builder.Services
    .AddGraphQLServer()
    .AddMongoDbFiltering()
    .AddMongoDbSorting()
    .AddMongoDbPagingProviders()
    .AddQueryType(d => d.Name("Query"))
        .AddTypeExtension<ProductQuery>()
    .AddType<ProductType>()
    .AddType<CategoryResolver>();

var app = builder.Build();

app.UseWebSockets();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL("/api/graphql");
});

app.Run();
