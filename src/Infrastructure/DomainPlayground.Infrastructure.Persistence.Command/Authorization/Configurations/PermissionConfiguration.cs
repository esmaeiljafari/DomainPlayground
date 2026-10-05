using DomainPlayground.Core.Domain.Authorization.Permissions;
using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainPlayground.Infrastructure.Persistence.Commands.Authorization.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(p => p.Key)
            .HasConversion(
                key => key.Value,
                value => PermissionKey.Create(value).Value)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.Title)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasIndex(p => p.Key).IsUnique();
        builder.Ignore(p => p.DomainEvents);
    }
}