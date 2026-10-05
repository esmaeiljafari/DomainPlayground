using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Abstractions.Security;
using DomainPlayground.Core.Application.Common.Behaviors;
using DomainPlayground.Core.Application.Common.Interfaces;
using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.SharedKernel.Abstractions;
using DomainPlayground.SharedKernel.Results;
using FluentAssertions;
using Moq;
using Xunit;

namespace DomainPlayground.Core.Application.UnitTests.Behaviors;

public class AuthorizationBehaviorTests
{
    [Fact]
    public async Task Handle_WhenNoPermissionAttribute_CallsNext()
    {
        var behavior = CreateBehavior<NoPermissionCommand>(isAuthenticated: false, userPermissions: []);
        var called = false;

        await behavior.Handle(
            new NoPermissionCommand(),
            _ => { called = true; return Task.FromResult(Result.Success()); },
            default);

        called.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenUserNotAuthenticated_ReturnsUnauthenticated()
    {
        var behavior = CreateBehavior<ProtectedCommand>(isAuthenticated: false, userPermissions: []);

        var result = await behavior.Handle(
            new ProtectedCommand(),
            _ => Task.FromResult(Result.Success()),
            default);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.Unauthenticated");
    }

    [Fact]
    public async Task Handle_WhenUserLacksPermission_ReturnsForbidden()
    {
        var behavior = CreateBehavior<ProtectedCommand>(isAuthenticated: true, userPermissions: []);

        var result = await behavior.Handle(
            new ProtectedCommand(),
            _ => Task.FromResult(Result.Success()),
            default);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Auth.Forbidden");
    }

    [Fact]
    public async Task Handle_WhenUserHasPermission_CallsNext()
    {
        var behavior = CreateBehavior<ProtectedCommand>(isAuthenticated: true, userPermissions: ["authorization.roles.create"]);
        var called = false;

        var result = await behavior.Handle(
            new ProtectedCommand(),
            _ => { called = true; return Task.FromResult(Result.Success()); },
            default);

        result.IsSuccess.Should().BeTrue();
        called.Should().BeTrue();
    }

    private static AuthorizationBehavior<TRequest, Result> CreateBehavior<TRequest>(
        bool isAuthenticated, string[] userPermissions)
        where TRequest : notnull
    {
        var currentUser = new Mock<ICurrentUser>();
        currentUser.Setup(x => x.IsAuthenticated).Returns(isAuthenticated);
        currentUser.Setup(x => x.UserId).Returns(isAuthenticated ? new UserId(Guid.NewGuid()) : (UserId?)null);

        var queries = new Mock<IAuthorizationQueries>();
        queries.Setup(x => x.GetUserPermissionKeysAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userPermissions.ToHashSet());

        return new AuthorizationBehavior<TRequest, Result>(currentUser.Object, queries.Object);
    }

    [HasPermission(nameof(ModuleCodes.Authorization), "roles.create", "ایجاد نقش")]
    private sealed record ProtectedCommand : ICommand;

    private sealed record NoPermissionCommand : ICommand;
}