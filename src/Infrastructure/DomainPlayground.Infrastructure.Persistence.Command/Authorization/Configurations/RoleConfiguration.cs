using DomainPlayground.Core.Domain.Authorization.Roles;
using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainPlayground.Infrastructure.Persistence.Commands.Authorization.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        builder.Property(r => r.Name).HasMaxLength(100).IsRequired();
        builder.HasIndex(r => r.Name).IsUnique();
        builder.Property(r => r.IsSystem).IsRequired();

        builder.OwnsMany(r => r.Permissions, b =>
        {
            b.ToTable("RolePermissions");
            b.WithOwner().HasForeignKey("RoleId");
            b.Property<RoleId>("RoleId");
            b.HasKey("RoleId", nameof(RolePermission.PermissionId));
            b.Property(p => p.PermissionId).ValueGeneratedNever();
        });

        builder.Navigation(r => r.Permissions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(r => r.DomainEvents);
    }
}