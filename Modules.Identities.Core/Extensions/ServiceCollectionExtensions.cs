
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Dispatcher;

namespace Modules.Identities.Core.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentitiesCore(this IServiceCollection services)
    {
        services.AddDispatcher(Assembly.GetExecutingAssembly());
        return services;
    }
}