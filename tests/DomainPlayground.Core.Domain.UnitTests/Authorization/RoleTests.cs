using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.Roles;
using DomainPlayground.Core.Domain.Authorization.Roles.Errors;
using FluentAssertions;

namespace DomainPlayground.Core.Domain.UnitTests.Authorization;

public class RoleTests
{
    [Fact]
    public void Create_WithValidName_ReturnsSuccess()
    {
        var result = Role.Create("Editor");

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Editor");
        result.Value.IsSystem.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidName_ReturnsFailure(string name)
    {
        var result = Role.Create(name);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(RoleErrors.NameRequired);
    }

    [Fact]
    public void Rename_OnSystemRole_ReturnsFailure()
    {
        var role = Role.Create("Admin", isSystem: true).Value;

        var result = role.Rename("NotAdmin");

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(RoleErrors.SystemRoleImmutable);
        role.Name.Should().Be("Admin");
    }

    [Fact]
    public void Disable_OnSystemRole_ReturnsFailure()
    {
        var role = Role.Create("Admin", isSystem: true).Value;

        var result = role.Disable();

        result.IsFailure.Should().BeTrue();
        role.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Disable_OnNonSystemRole_SetsIsActiveFalse()
    {
        var role = Role.Create("Editor").Value;

        var result = role.Disable();

        result.IsSuccess.Should().BeTrue();
        role.IsActive.Should().BeFalse();
    }

    [Fact]
    public void SetPermissions_OnSystemRole_ReturnsFailure()
    {
        var role = Role.Create("Admin", isSystem: true).Value;

        var result = role.SetPermissions([new PermissionId(1), new PermissionId(2)]);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(RoleErrors.SystemRoleImmutable);
        role.Permissions.Should().BeEmpty();
    }

    [Fact]
    public void SetPermissions_OnNonSystemRole_AddsAndRemovesCorrectly()
    {
        var role = Role.Create("Editor").Value;
        role.SetPermissions([new PermissionId(1), new PermissionId(2)]);

        var result = role.SetPermissions([new PermissionId(2), new PermissionId(3)]);

        result.IsSuccess.Should().BeTrue();
        role.Permissions.Select(p => p.PermissionId.Value)
            .Should().BeEquivalentTo([2, 3]);
    }
}