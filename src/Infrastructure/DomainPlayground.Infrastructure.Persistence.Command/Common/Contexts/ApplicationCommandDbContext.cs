using DomainPlayground.Core.Application.Common.Interfaces.Persistence;
using DomainPlayground.Core.Domain.Authorization.Permissions;
using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.Roles;
using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.UserAccesses;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.Infrastructure.Persistence.Commands.Common.Converters;
using Microsoft.EntityFrameworkCore;

namespace DomainPlayground.Infrastructure.Persistence.Commands.Common.Contexts;

public sealed class ApplicationCommandDbContext(DbContextOptions<ApplicationCommandDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserAccess> UserAccesses => Set<UserAccess>();

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<UserId>().HaveConversion<UserIdConverter>();
        builder.Properties<UserAccessId>().HaveConversion<UserAccessIdConverter>();
        builder.Properties<RoleId>().HaveConversion<RoleIdConverter>();
        builder.Properties<PermissionId>().HaveConversion<PermissionIdConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationCommandDbContext).Assembly);
    }
}