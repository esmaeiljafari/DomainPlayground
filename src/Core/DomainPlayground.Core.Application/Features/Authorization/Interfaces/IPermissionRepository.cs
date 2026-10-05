using DomainPlayground.Core.Domain.Authorization.Permissions;
using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Authorization.Interfaces
{
    public interface IPermissionRepository
    {
        Task<Permission?> GetByIdAsync(PermissionId id, CancellationToken ct = default);
        Task<Permission?> GetByKeyAsync(PermissionKey key, CancellationToken ct = default);
        Task<IReadOnlyList<Permission>> GetByIdsAsync(IEnumerable<PermissionId> ids, CancellationToken ct = default);
        Task AddAsync(Permission permission, CancellationToken ct = default);
    }
}
