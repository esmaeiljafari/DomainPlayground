using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Core.Domain.Authorization.Permissions;
using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using DomainPlayground.Infrastructure.Persistence.Commands.Common.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DomainPlayground.Infrastructure.Persistence.Commands.Authorization.Repositories;

public sealed class PermissionRepository(ApplicationCommandDbContext db) : IPermissionRepository
{
    public Task<Permission?> GetByIdAsync(PermissionId id, CancellationToken ct = default) =>
        db.Permissions.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<Permission?> GetByKeyAsync(PermissionKey key, CancellationToken ct = default) =>
        db.Permissions.FirstOrDefaultAsync(p => p.Key == key, ct);

    public async Task<IReadOnlyList<Permission>> GetByIdsAsync(IEnumerable<PermissionId> ids, CancellationToken ct = default)
    {
        var list = ids.Distinct().ToList();
        return await db.Permissions.Where(p => list.Contains(p.Id)).ToListAsync(ct);
    }

    public async Task AddAsync(Permission permission, CancellationToken ct = default) =>
        await db.Permissions.AddAsync(permission, ct);
}
