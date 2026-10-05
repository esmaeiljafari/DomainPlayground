using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Core.Application.Features.Users.Interfaces;
using DomainPlayground.Infrastructure.Persistence.Queries.Authorization.Repositories;
using DomainPlayground.Infrastructure.Persistence.Queries.Contexts;
using DomainPlayground.Infrastructure.Persistence.Queries.Identity.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DomainPlayground.Infrastructure.Persistence.Queries;

public static class DependencyInjection
{
    public static IServiceCollection AddQueryPersistence(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationQueryDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("QueryDatabase")));

        services.AddScoped<IAuthorizationQueries, AuthorizationQueriesRepository>();
        services.AddScoped<IUsersQueries, UsersQueriesRepository>();

        return services;
    }
}