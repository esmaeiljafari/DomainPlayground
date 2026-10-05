using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Core.Domain.Authorization.Roles;
using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.Infrastructure.Persistence.Commands.Common.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DomainPlayground.Infrastructure.Persistence.Commands.Authorization.Repositories;

public sealed class RoleRepository(ApplicationCommandDbContext db) : IRoleRepository
{
    public Task<Role?> GetByIdAsync(RoleId id, CancellationToken ct = default) =>
        db.Roles.FirstOrDefaultAsync(r => r.Id == id, ct);

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default) =>
        db.Roles.AnyAsync(r => r.Name == name.Trim(), ct);

    public async Task AddAsync(Role role, CancellationToken ct = default) =>
        await db.Roles.AddAsync(role, ct);

    public void Remove(Role role) => db.Roles.Remove(role);

    public async Task<IReadOnlyList<Role>> GetByIdsAsync(IEnumerable<RoleId> ids, CancellationToken ct = default)
    {
        var list = ids.Distinct().ToList();
        return await db.Roles.Where(r => list.Contains(r.Id)).ToListAsync(ct);
    }
}
