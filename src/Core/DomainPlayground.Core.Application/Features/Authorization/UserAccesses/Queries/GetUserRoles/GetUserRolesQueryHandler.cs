using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.SharedKernel.Results;

namespace DomainPlayground.Core.Application.Features.Authorization.UserAccesses.Queries.GetUserRoles
{
    public sealed class GetUserRolesQueryHandler(IAuthorizationQueries queries)
        : IQueryHandler<GetUserRolesQuery, List<UserRoleDto>>
    {
        public async Task<Result<List<UserRoleDto>>> Handle(GetUserRolesQuery query, CancellationToken ct)
        {
            return Result.Success(await queries.GetUserRolesAsync(query.UserId, ct));
        }
    }
}
