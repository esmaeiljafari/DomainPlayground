using DomainPlayground.Core.Domain.Authorization.UserAccesses;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainPlayground.Infrastructure.Persistence.Commands.Authorization.Configurations;

internal sealed class UserAccessConfiguration : IEntityTypeConfiguration<UserAccess>
{
    public void Configure(EntityTypeBuilder<UserAccess> builder)
    {
        builder.ToTable("UserAccesses");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).ValueGeneratedNever();

        builder.Property(u => u.UserId).IsRequired();
        builder.HasIndex(u => u.UserId).IsUnique();

        builder.OwnsMany(u => u.Roles, b =>
        {
            b.ToTable("UserAccessRoles");
            b.WithOwner().HasForeignKey("UserAccessId");
            b.Property<UserAccessId>("UserAccessId");
            b.HasKey("UserAccessId", nameof(UserRole.RoleId));
            b.Property(r => r.RoleId).ValueGeneratedNever();
        });

        builder.OwnsMany(u => u.Overrides, b =>
        {
            b.ToTable("UserAccessOverrides");
            b.WithOwner().HasForeignKey("UserAccessId");
            b.Property<UserAccessId>("UserAccessId");
            b.HasKey("UserAccessId", nameof(UserPermissionOverride.PermissionId));
            b.Property(o => o.PermissionId).ValueGeneratedNever();
            b.Property(o => o.Effect).HasConversion<int>().IsRequired();
        });

        builder.Navigation(u => u.Roles).UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Navigation(u => u.Overrides).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(u => u.DomainEvents);
    }
}