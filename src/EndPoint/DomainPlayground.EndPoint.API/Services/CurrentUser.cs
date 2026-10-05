using System.Security.Claims;
using DomainPlayground.Core.Application.Common.Interfaces;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;

namespace DomainPlayground.EndPoint.API.Services;


public sealed class CurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    public UserId? UserId
    {
        get
        {
            var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                        ?? accessor.HttpContext?.User.FindFirst("sub")?.Value;

            return Guid.TryParse(value, out var id) ? new UserId(id) : null;
        }
    }
    public bool IsAuthenticated => accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}