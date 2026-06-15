using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Identities.Core.Contracts;
using Modules.Identities.Infrastructure.Persistence;
using Shared.Infrastructure.Extensions;

namespace Modules.Identities.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddDatabaseContext<IdentitiesDbContext>(config)
            .AddScoped<IIdentitiesDbContext>(provider => provider.GetService<IdentitiesDbContext>());
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        return services;
    }
}