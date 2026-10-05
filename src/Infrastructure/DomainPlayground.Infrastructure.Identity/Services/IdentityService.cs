using DomainPlayground.Core.Application.Features.Identity.Errors;
using DomainPlayground.Core.Application.Features.Identity.Interfaces;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.Infrastructure.Identity.Models;
using DomainPlayground.SharedKernel.Results;
using Microsoft.AspNetCore.Identity;

namespace CleanLedger.Infrastructure.Identity.Services;

public sealed class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<Result> CreateUserAsync(
        UserId userId, string userName, string password, CancellationToken ct = default)
    {
        var existing = await userManager.FindByNameAsync(userName);
        if (existing is not null)
            return Result.Failure(IdentityErrors.UserNameTaken);

        var user = new ApplicationUser
        {
            Id = userId.Value,
            UserName = userName
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var isWeakPassword = result.Errors.Any(e => e.Code.StartsWith("Password"));
            return Result.Failure(isWeakPassword
                ? IdentityErrors.WeakPassword
                : new(result.Errors.First().Code, DomainPlayground.SharedKernel.Errors.ErrorType.Unauthorized));
        }

        return Result.Success();
    }

    public async Task<Result<AuthenticatedUser>> ValidateCredentialsAsync(
        string userName, string password, CancellationToken ct = default)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user is null)
            return Result.Failure<AuthenticatedUser>(IdentityErrors.InvalidCredentials);

        if (await userManager.IsLockedOutAsync(user))
            return Result.Failure<AuthenticatedUser>(IdentityErrors.LockedOut);

        var passwordValid = await userManager.CheckPasswordAsync(user, password);
        if (!passwordValid)
        {
            await userManager.AccessFailedAsync(user);
            return Result.Failure<AuthenticatedUser>(IdentityErrors.InvalidCredentials);
        }

        await userManager.ResetAccessFailedCountAsync(user);

        return Result.Success(new AuthenticatedUser(new UserId(user.Id), user.UserName!));
    }
}