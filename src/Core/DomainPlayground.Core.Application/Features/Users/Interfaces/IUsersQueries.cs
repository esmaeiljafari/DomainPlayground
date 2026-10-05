using DomainPlayground.Core.Application.Common.Models;
using DomainPlayground.Core.Application.Features.Users.Queries.GetUsers;
namespace DomainPlayground.Core.Application.Features.Users.Interfaces;
public interface IUsersQueries
{
    Task<PagedResult<UserListItemDto>> GetUsersAsync(GetUsersQuery query, CancellationToken ct = default);
}
