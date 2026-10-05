using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.SharedKernel.Results;

namespace DomainPlayground.Core.Application.Features.Authorization.Roles.Queries.GetAllRoles;

public sealed class GetAllRolesQueryHandler(IAuthorizationQueries queries) : IQueryHandler<GetAllRolesQuery, List<RoleDto>>
{
    public async Task<Result<List<RoleDto>>> Handle(GetAllRolesQuery query, CancellationToken ct)
    {
        return Result.Success(await queries.GetAllRolesAsync(ct));
    }

}
