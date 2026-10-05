using DomainPlayground.Infrastructure.Persistence.Queries.Authorization.ReadModels;
using DomainPlayground.Infrastructure.Persistence.Queries.Identity.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace DomainPlayground.Infrastructure.Persistence.Queries.Contexts;

public sealed class ApplicationQueryDbContext(DbContextOptions<ApplicationQueryDbContext> options)
    : DbContext(options)
{
    public DbSet<RoleReadModel> Roles => Set<RoleReadModel>();
    public DbSet<RolePermissionReadModel> RolePermissions => Set<RolePermissionReadModel>();
    public DbSet<PermissionReadModel> Permissions => Set<PermissionReadModel>();
    public DbSet<UserAccessReadModel> UserAccesses => Set<UserAccessReadModel>();
    public DbSet<UserAccessRoleReadModel> UserAccessRoles => Set<UserAccessRoleReadModel>();
    public DbSet<UserPermissionOverrideReadModel> UserAccessOverrides => Set<UserPermissionOverrideReadModel>();
    public DbSet<UserReadModel> Users => Set<UserReadModel>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoleReadModel>(b =>
        {
            b.ToTable("Roles");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<RolePermissionReadModel>(b =>
        {
            b.ToTable("RolePermissions");
            b.HasKey(x => new { x.RoleId, x.PermissionId });
        });

        modelBuilder.Entity<PermissionReadModel>(b =>
        {
            b.ToTable("Permissions");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<UserAccessReadModel>(b =>
        {
            b.ToTable("UserAccesses");
            b.HasKey(x => x.Id);
        });

        modelBuilder.Entity<UserAccessRoleReadModel>(b =>
        {
            b.ToTable("UserAccessRoles");
            b.HasKey(x => new { x.UserAccessId, x.RoleId });
        });

        modelBuilder.Entity<UserPermissionOverrideReadModel>(b =>
        {
            b.ToTable("UserAccessOverrides");
            b.HasKey(x => new { x.UserAccessId, x.PermissionId });
        });

        modelBuilder.Entity<UserReadModel>(b =>
        {
            b.ToTable("AspNetUsers");
            b.HasKey(x => x.Id);
        });
    }
}