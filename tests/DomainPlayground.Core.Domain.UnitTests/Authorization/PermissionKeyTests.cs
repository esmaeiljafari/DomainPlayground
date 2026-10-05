using DomainPlayground.Core.Domain.Authorization.Permissions.Errors;
using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using FluentAssertions;
using Xunit;

namespace DomainPlayground.Core.Domain.UnitTests.Authorization;

public class PermissionKeyTests
{
    [Theory]
    [InlineData("authorization.roles.create")]
    [InlineData("customer.orders.view")]
    public void Create_WithValidFormat_ReturnsSuccess(string value)
    {
        var result = PermissionKey.Create(value);

        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData("Authorization.roles.create")]
    [InlineData("authorization.rolesView")]
    [InlineData("authorization..create")]
    [InlineData("")]
    public void Create_WithInvalidFormat_ReturnsFailure(string value)
    {
        var result = PermissionKey.Create(value);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PermissionErrors.InvalidKey);
    }

    [Fact]
    public void Create_WithNull_ReturnsFailure()
    {
        var result = PermissionKey.Create(null);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Module_Resource_Action_AreParsedCorrectly()
    {
        var key = PermissionKey.Create("authorization.roles.create").Value;

        key.Module.Should().Be("authorization");
        key.Resource.Should().Be("roles");
        key.Action.Should().Be("create");
    }
}