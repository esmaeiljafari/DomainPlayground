using DomainPlayground.Core.Application.Common.Models;
using DomainPlayground.Core.Application.Features.Users.Interfaces;
using DomainPlayground.Core.Application.Features.Users.Queries.GetUsers;
using DomainPlayground.Infrastructure.Persistence.Queries.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DomainPlayground.Infrastructure.Persistence.Queries.Identity.Repositories;

public sealed class UsersQueriesRepository(ApplicationQueryDbContext db) : IUsersQueries
{
    public async Task<PagedResult<UserListItemDto>> GetUsersAsync(GetUsersQuery query, CancellationToken ct = default)
    {
        var baseQuery = db.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.UserName))
            baseQuery = baseQuery.Where(u => u.UserName.Contains(query.UserName));

        var totalCount = await baseQuery.CountAsync(ct);

        var users = await baseQuery
            .OrderBy(u => u.UserName)
            .Skip(query.Skip)
            .Take(query.Take)
            .Select(u => new UserListItemDto(u.Id, u.UserName))
            .ToListAsync(ct);

        return new PagedResult<UserListItemDto>(users, totalCount, query.PageNumber, query.PageSize);
    }
}