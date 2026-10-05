using DomainPlayground.Core.Application.Features.Authorization.Roles.Commands.CreateRole;
using DomainPlayground.Core.Application.Features.Authorization.Roles.Commands.DisableRole;
using DomainPlayground.Core.Application.Features.Authorization.Roles.Commands.UpdateRole;
using DomainPlayground.Core.Application.Features.Authorization.Roles.Queries.GetAllRoles;
using DomainPlayground.Core.Application.Features.Authorization.Roles.Queries.GetRolePermissions;
using DomainPlayground.Core.Application.Features.Authorization.UserAccesses.Commands.UpdateUserRoles;
using DomainPlayground.Core.Application.Features.Authorization.UserAccesses.Queries.GetUserRoles;
using DomainPlayground.Core.Application.Features.Identity.Commands.Login;
using DomainPlayground.Core.Application.Features.Identity.Commands.Logout;
using DomainPlayground.Core.Application.Features.Identity.Commands.RefreshToken;
using DomainPlayground.Core.Application.Features.Identity.Commands.Register;
using DomainPlayground.Core.Application.Features.Users.Queries.GetUsers;
using DomainPlayground.EndPoint.API.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DomainPlayground.EndPoint.API.Controllers;

[Route("api/auth")]
public sealed class AuthController(ISender sender) : BaseController
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        => HandleResult(await sender.Send(command, ct));

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
        => HandleResult(await sender.Send(command, ct));

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken ct)
        => HandleResult(await sender.Send(command, ct));

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] LogoutCommand command, CancellationToken ct)
        => HandleResult(await sender.Send(command, ct));
}

[Route("api/authorization")]
[Authorize]
public sealed class AuthorizationController(ISender sender) : BaseController
{
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles(CancellationToken ct)
        => HandleResult(await sender.Send(new GetAllRolesQuery(), ct));

    [HttpGet("roles/{roleId:int}/permissions")]
    public async Task<IActionResult> GetRolePermissions(int roleId, CancellationToken ct)
        => HandleResult(await sender.Send(new GetRolePermissionsQuery(roleId), ct));

    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command, CancellationToken ct)
        => HandleResult(await sender.Send(command, ct));

    [HttpPut("roles/{roleId:int}")]
    public async Task<IActionResult> UpdateRole(int roleId, [FromBody] UpdateRoleRequest request, CancellationToken ct)
        => HandleResult(await sender.Send(new UpdateRoleCommand(roleId, request.Name, request.PermissionIds), ct));

    [HttpPatch("roles/{roleId:int}/disable")]
    public async Task<IActionResult> DisableRole(int roleId, CancellationToken ct)
        => HandleResult(await sender.Send(new DisableRoleCommand(roleId), ct));

    [HttpGet("users/{userId:guid}/roles")]
    public async Task<IActionResult> GetUserRoles(Guid userId, CancellationToken ct)
        => HandleResult(await sender.Send(new GetUserRolesQuery(userId), ct));

    [HttpPut("users/{userId:guid}/roles")]
    public async Task<IActionResult> UpdateUserRoles(Guid userId, [FromBody] UpdateUserRolesRequest request, CancellationToken ct)
        => HandleResult(await sender.Send(new UpdateUserRolesCommand(userId, request.RoleIds), ct));
}

[Route("api/users")]
[Authorize]
public sealed class UsersController(ISender sender) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? userName, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var query = new GetUsersQuery { UserName = userName, PageNumber = pageNumber, PageSize = pageSize };
        var result = await sender.Send(query, ct);
        return HandleResult(result);
    }
}


public sealed record UpdateRoleRequest(string Name, List<int> PermissionIds);
public sealed record UpdateUserRolesRequest(List<int> RoleIds);