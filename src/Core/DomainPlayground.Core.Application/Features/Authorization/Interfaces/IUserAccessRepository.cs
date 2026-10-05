using DomainPlayground.Core.Domain.Authorization.UserAccesses;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;

namespace DomainPlayground.Core.Application.Features.Authorization.Interfaces
{
    public interface IUserAccessRepository
    {
        Task<UserAccess?> GetByUserIdAsync(UserId userId, CancellationToken ct = default);
        Task AddAsync(UserAccess access, CancellationToken ct = default);
    }
}
