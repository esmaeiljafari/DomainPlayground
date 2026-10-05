using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Features.Authorization.Interfaces;
using DomainPlayground.Core.Domain.Authorization.Roles;
using DomainPlayground.Core.Domain.Authorization.Roles.Errors;
using DomainPlayground.SharedKernel.Results;

namespace DomainPlayground.Core.Application.Features.Authorization.Roles.Commands.CreateRole;

internal sealed class CreateRoleCommandHandler(IRoleRepository roles) : ICommandHandler<CreateRoleCommand>
{
    public async Task<Result> Handle(CreateRoleCommand cmd, CancellationToken ct)
    {
        Console.WriteLine($"===== HANDLER HIT: {cmd.Name} =====");

        if (await roles.ExistsByNameAsync(cmd.Name, ct))
            return Result.Failure(RoleErrors.NameAlreadyExists);

        var role = Role.Create(cmd.Name);
        if (role.IsFailure) return Result.Failure(role.Error);

        await roles.AddAsync(role.Value, ct);
        Console.WriteLine("===== SAVED TO DBCONTEXT (before SaveChanges) =====");
        return Result.Success();
    }
}