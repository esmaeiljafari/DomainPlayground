using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.SharedKernel.Results;

namespace DomainPlayground.Core.Application.Features.Identity.Interfaces;

public interface IIdentityService
{
    Task<Result> CreateUserAsync(
        UserId userId, string userName, string password, CancellationToken ct = default);

    Task<Result<AuthenticatedUser>> ValidateCredentialsAsync(
        string userName, string password, CancellationToken ct = default);
}