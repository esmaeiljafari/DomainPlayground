using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Core.Domain.Authorization.UserAccesses;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.Infrastructure.Persistence.Commands.Common.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DomainPlayground.Infrastructure.Persistence.Commands.Authorization.Repositories;

public sealed class UserAccessRepository(ApplicationCommandDbContext db) : IUserAccessRepository
{
    public Task<UserAccess?> GetByUserIdAsync(UserId userId, CancellationToken ct = default) =>
        db.UserAccesses.FirstOrDefaultAsync(u => u.UserId == userId, ct);

    public async Task AddAsync(UserAccess access, CancellationToken ct = default) =>
        await db.UserAccesses.AddAsync(access, ct);
}