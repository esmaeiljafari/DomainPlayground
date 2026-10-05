using DomainPlayground.Core.Application.Common.Interfaces.Persistence;
using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Infrastructure.Persistence.Commands.Authorization.Repositories;
using DomainPlayground.Infrastructure.Persistence.Commands.Authorization.Services;
using DomainPlayground.Infrastructure.Persistence.Commands.Common.Contexts;
using DomainPlayground.Infrastructure.Persistence.Commands.Common.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DomainPlayground.Infrastructure.Persistence.Commands;

public static class DependencyInjection
{
    public static IServiceCollection AddCommandPersistence(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<AuditableEntityInterceptor>();

        services.AddDbContext<ApplicationCommandDbContext>((sp, options) =>
            options
                .UseSqlServer(configuration.GetConnectionString("CommandDatabase"))
                .AddInterceptors(sp.GetRequiredService<AuditableEntityInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationCommandDbContext>());

        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IUserAccessRepository, UserAccessRepository>();

        services.AddScoped<PermissionSynchronizer>();
        services.AddScoped<AuthorizationSeeder>();

        return services;
    }
}