using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Identities.Core.Extensions;
using Modules.Identities.Infrastructure.Extensions;

namespace Modules.Identities.Extensions;

public static class ModuleExtensions
{
    public static IServiceCollection AddIdentitiesModule(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentitiesCore()
            .AddIdentityInfrastructure(configuration);
        return services;
    }
}