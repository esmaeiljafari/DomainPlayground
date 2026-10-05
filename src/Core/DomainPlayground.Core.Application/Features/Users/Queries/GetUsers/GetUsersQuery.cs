using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Abstractions.Security;
using DomainPlayground.Core.Application.Common.Models;
using DomainPlayground.Core.Application.Features.Users.Interfaces;
using DomainPlayground.SharedKernel.Abstractions;
using DomainPlayground.SharedKernel.Results;

namespace DomainPlayground.Core.Application.Features.Users.Queries.GetUsers;

public sealed record UserListItemDto(
    Guid UserId, string UserName);

[HasPermission(nameof(ModuleCodes.User), "users.view", "مشاهده کاربران")]
public sealed record GetUsersQuery : PagedQuery, IQuery<PagedResult<UserListItemDto>>
{
    public string? UserName { get; init; }
}

internal sealed class GetUsersQueryHandler(IUsersQueries usersQueries)
    : IQueryHandler<GetUsersQuery, PagedResult<UserListItemDto>>
{
    public async Task<Result<PagedResult<UserListItemDto>>> Handle(GetUsersQuery query, CancellationToken ct) =>
        Result.Success(await usersQueries.GetUsersAsync(query, ct));
}