using HotChocolate.Types.Descriptors;
using System.Reflection;
using System.Runtime.CompilerServices;
using UseServiceScopeAndPaging.GraphQL.Middleware;

namespace UseServiceScopeAndPaging.GraphQL.Attributes;

public class UseMongoTransactionMiddlewareAttribute : ObjectFieldDescriptorAttribute
{
    public UseMongoTransactionMiddlewareAttribute([CallerLineNumber] int order = 0)
    {
        Order = order;
    }

    protected override void OnConfigure(IDescriptorContext context,
        IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.Use<UseMongoTransactionMiddleware>();
    }
}
