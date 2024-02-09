using HotChocolate.Resolvers;
using MongoDB.Driver;

namespace UseServiceScopeAndPaging.GraphQL.Middleware;

public class UseMongoTransactionMiddleware
{
    private readonly FieldDelegate _next;

    public UseMongoTransactionMiddleware(FieldDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(IMiddlewareContext context, IClientSessionHandle clientSessionHandle)
    {
        clientSessionHandle.StartTransaction(new MongoDB.Driver.TransactionOptions());

        try
        {
            await _next(context);
            clientSessionHandle.CommitTransaction(context.RequestAborted);
        }
        catch
        {
            clientSessionHandle.AbortTransaction(context.RequestAborted);
            throw; // let existing error handling / error logging take over
        }
    }
}
