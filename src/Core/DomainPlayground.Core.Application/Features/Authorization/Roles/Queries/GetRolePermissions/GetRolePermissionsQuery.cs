using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Application.Features.Authorization.Roles.Queries.GetRolePermissions
{
    public sealed record GetRolePermissionsQuery(int RoleId) : IQuery<List<RolePermissionGroupDto>>;

    internal sealed class GetRolePermissionsQueryHandler(IAuthorizationQueries queries)
        : IQueryHandler<GetRolePermissionsQuery, List<RolePermissionGroupDto>>
    {
        public async Task<Result<List<RolePermissionGroupDto>>> Handle(GetRolePermissionsQuery query, CancellationToken ct) =>
            Result.Success(await queries.GetRolePermissionsAsync(query.RoleId, ct));
    }

    public sealed record RolePermissionItemDto(int PermissionId, string Key, string Title, bool IsGranted);

    public sealed record RolePermissionGroupDto(string ModuleCode, string ModuleTitle, List<RolePermissionItemDto> Permissions);
}
