using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.UserAccesses;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.Enums;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.Errors;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using FluentAssertions;

namespace DomainPlayground.Core.Domain.UnitTests.Authorization;

public class UserAccessTests
{
    [Fact]
    public void AssignRole_Twice_ReturnsFailureOnSecondCall()
    {
        var access = UserAccess.Create(new UserId(Guid.NewGuid()));
        var roleId = new RoleId(1);

        access.AssignRole(roleId);
        var result = access.AssignRole(roleId);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserAccessErrors.RoleAlreadyAssigned);
    }

    [Fact]
    public void RemoveRole_WhenNotAssigned_ReturnsFailure()
    {
        var access = UserAccess.Create(new UserId(Guid.NewGuid()));

        var result = access.RemoveRole(new RoleId(99));

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserAccessErrors.RoleNotAssigned);
    }

    [Fact]
    public void SetOverride_Allow_AddsToOverrides()
    {
        var access = UserAccess.Create(new UserId(Guid.NewGuid()));

        access.SetOverride(new PermissionId(5), AccessEffect.Allow);

        access.Overrides.Should()
            .ContainSingle(o => o.PermissionId.Value == 5 && o.Effect == AccessEffect.Allow);
    }

    [Fact]
    public void SetOverride_CalledTwiceForSamePermission_ReplacesOldOne()
    {
        var access = UserAccess.Create(new UserId(Guid.NewGuid()));
        var permissionId = new PermissionId(5);

        access.SetOverride(permissionId, AccessEffect.Allow);
        access.SetOverride(permissionId, AccessEffect.Deny);

        access.Overrides.Should().ContainSingle();
        access.Overrides.Single().Effect.Should().Be(AccessEffect.Deny);
    }
}